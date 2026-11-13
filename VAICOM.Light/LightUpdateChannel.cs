using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using VAICOM.FileManager;

namespace VAICOM.Light
{
    internal static class LightUpdateChannel
    {
        // Light-specific update feed. Keep this separate from Full VAICOM update endpoints.
        internal const string FeedUrl = "https://github.com/Penecruz/VAICOM-Community/releases/latest/download/updates.light.json";

        internal static Uri GetFeedUri()
        {
            return new Uri(FeedUrl, UriKind.Absolute);
        }

        internal static void TryCheckForUpdatesAtStartup()
        {
            try
            {
                if (State.activeconfig != null && State.activeconfig.DisableAutomaticUpdates)
                {
                    return;
                }

                string json;
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString(GetFeedUri());
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    return;
                }

                FileHandler.Updater.UpdateDescriptions description = JsonConvert.DeserializeObject<FileHandler.Updater.UpdateDescriptions>(json);
                if (description?.updates == null || description.updates.Count == 0)
                {
                    return;
                }

                Version current = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

                var latest = description.updates
                    .Where(u => u != null && u.isrelease && !string.IsNullOrWhiteSpace(u.versionstring) && !string.IsNullOrWhiteSpace(u.downloadurl))
                    .Select(u => new { Item = u, Version = ParseVersion(u.versionstring) })
                    .Where(x => x.Version != null)
                    .OrderByDescending(x => x.Version)
                    .FirstOrDefault();

                if (latest == null || latest.Version <= current)
                {
                    return;
                }

                string title = string.IsNullOrWhiteSpace(latest.Item.title) ? "VAICOM Light Update" : latest.Item.title;
                string notes = string.IsNullOrWhiteSpace(latest.Item.releasenotes) ? "A new VAICOM Light update is available." : latest.Item.releasenotes;
                string message = "New VAICOM Light update available (" + latest.Version + ").\n\n"
                    + notes + "\n\n"
                    + "Open download page now?";

                MessageBoxResult choice = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (choice == MessageBoxResult.Yes)
                {
                    Process.Start(latest.Item.downloadurl);
                }
            }
            catch
            {
            }
        }

        private static Version ParseVersion(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            string normalized = raw.Trim();
            if (normalized.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                normalized = normalized.Substring(1);
            }

            return Version.TryParse(normalized, out Version parsed) ? parsed : null;
        }
    }
}
