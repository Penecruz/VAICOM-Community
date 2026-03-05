using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Win32;
using VAICOM.Client;
using VAICOM.Static;

namespace VAICOM
{

    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            private const int DwmwaUseImmersiveDarkMode = 20;
            private const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
            private const int DwmwaBorderColor = 34;
            private const int DwmwaCaptionColor = 35;
            private const int DwmwaTextColor = 36;
            private const int DwmColorDefault = unchecked((int)0xFFFFFFFF);
            private const bool ForceWindowDarkMode = true;
            private bool windowDarkModeInitialized;
            private Brush tabItemBackground;
            private Brush tabItemForeground;
            private Brush tabItemSelectedBackground;
            private Brush tabItemSelectedForeground;
            private readonly bool? useDarkModeOverride;

            [DllImport("dwmapi.dll")]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

            [DllImport("dwmapi.dll")]
            private static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out int attrValue, int attrSize);

            // ------------ WINDOW HANDLERS -------------------------------

            public ConfigWindow(bool? useDarkModeOverride = null)
            {
                this.useDarkModeOverride = useDarkModeOverride;
                InitializeComponent();

                this.ResizeMode = ResizeMode.CanMinimize;
                this.WindowStyle = WindowStyle.SingleBorderWindow;

                if (State.activeconfig.Windowrestorelocation != null)
                {
                    this.Left = State.activeconfig.Windowrestorelocation.X;
                    this.Top = State.activeconfig.Windowrestorelocation.Y;
                }

                this.Width = 500;
                this.Height = 340;

                //this.WindowStyle = WindowStyle.None;
                this.Topmost = false;

                //this.Opacity = 1.0;

                UpdateAllbugs();

                // Hook up the Voice Access Priority checkbox
                UseVoiceAccessPriority.Checked += EnableVoiceAccessPriority;
                UseVoiceAccessPriority.Unchecked += DisableVoiceAccessPriority;
                UseVoiceAccessPriority.Loaded += SetCurrentValueVoiceAccessPriority;

                ApplyVoiceAttackTheme();
            }

            private void ApplyVoiceAttackTheme()
            {
                bool useDarkMode;
                bool resolved = TryResolveDarkModeFromVoiceAttackWindow(out useDarkMode);

                if (!resolved && useDarkModeOverride.HasValue)
                {
                    useDarkMode = useDarkModeOverride.Value;
                    resolved = true;
                }

                if (!resolved)
                {
                    resolved = TryResolveVoiceAttackDarkMode(State.Proxy, out useDarkMode);
                }

                if (!resolved && ForceWindowDarkMode)
                {
                    useDarkMode = true;
                }

                ApplyThemeResources(useDarkMode);
                ApplyWindowDarkMode(useDarkMode);
            }

            private void ApplyWindowDarkMode(bool useDarkMode)
            {
                if (windowDarkModeInitialized)
                {
                    return;
                }

                try
                {
                    var windowHandle = new WindowInteropHelper(this).Handle;
                    if (windowHandle == IntPtr.Zero)
                    {
                        SourceInitialized += (sender, args) => ApplyWindowDarkMode(useDarkMode);
                        return;
                    }

                    windowDarkModeInitialized = true;
                    int useDarkValue = useDarkMode ? 1 : 0;
                    if (DwmSetWindowAttribute(windowHandle, DwmwaUseImmersiveDarkMode, ref useDarkValue, sizeof(int)) != 0)
                    {
                        DwmSetWindowAttribute(windowHandle, DwmwaUseImmersiveDarkModeBefore20H1, ref useDarkValue, sizeof(int));
                    }

                    ApplyWindowCaptionColors(windowHandle, useDarkMode);
                }
                catch
                {
                }
            }

            private void ApplyWindowCaptionColors(IntPtr windowHandle, bool useDarkMode)
            {
                int borderColor = useDarkMode ? ColorToColorRef(Color.FromRgb(45, 45, 45)) : DwmColorDefault;
                int captionColor = useDarkMode ? ColorToColorRef(Color.FromRgb(32, 32, 32)) : DwmColorDefault;
                int textColor = useDarkMode ? ColorToColorRef(Color.FromRgb(230, 230, 230)) : DwmColorDefault;

                DwmSetWindowAttribute(windowHandle, DwmwaBorderColor, ref borderColor, sizeof(int));
                DwmSetWindowAttribute(windowHandle, DwmwaCaptionColor, ref captionColor, sizeof(int));
                DwmSetWindowAttribute(windowHandle, DwmwaTextColor, ref textColor, sizeof(int));
            }

            private static int ColorToColorRef(Color color)
            {
                return color.R | (color.G << 8) | (color.B << 16);
            }

            internal static bool TryResolveVoiceAttackDarkMode(dynamic proxy, out bool isDarkMode)
            {
                isDarkMode = false;
                if (proxy == null)
                {
                    return false;
                }

                if (TryResolveDarkModeFromVoiceAttackWindow(out isDarkMode))
                {
                    return true;
                }

                if (TryGetSessionStateBool(proxy, "VA_DARK_MODE", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateBool(proxy, "VA_DARKMODE", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateBool(proxy, "VA_DARK_THEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateBool(proxy, "VA_THEME_DARK", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateBool(proxy, "VA_USE_DARK_THEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateBool(proxy, "VA_DARKMODE", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_THEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_THEME_NAME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_UI_THEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_UITHEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_UI_THEME_NAME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_APPTHEME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_APPTHEME_NAME", out isDarkMode))
                {
                    return true;
                }
                if (TryGetSessionStateTheme(proxy, "VA_INTERFACE_THEME", out isDarkMode))
                {
                    return true;
                }

                if (TryGetTokenBool(proxy, "{VA_DARK_MODE}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenBool(proxy, "{VA_DARKMODE}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenBool(proxy, "{VA_DARK_THEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenBool(proxy, "{VA_USE_DARK_THEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_THEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_THEME_NAME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_UI_THEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_UITHEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_UI_THEME_NAME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_APPTHEME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_APPTHEME_NAME}", out isDarkMode))
                {
                    return true;
                }
                if (TryGetTokenTheme(proxy, "{VA_INTERFACE_THEME}", out isDarkMode))
                {
                    return true;
                }

                if (TryResolveDarkModeFromSessionState(proxy, out isDarkMode))
                {
                    return true;
                }

                if (TryResolveDarkModeFromProxyProperties(proxy, out isDarkMode))
                {
                    return true;
                }

                if (TryResolveDarkModeFromVoiceAttackRegistry(out isDarkMode))
                {
                    return true;
                }

                return false;
            }

            private static bool TryResolveDarkModeFromProxyProperties(dynamic proxy, out bool isDarkMode)
            {
                isDarkMode = false;
                try
                {
                    if (TryResolveDarkModeFromObject(proxy, out isDarkMode))
                    {
                        return true;
                    }

                    var utility = proxy?.Utility;
                    if (utility != null && TryResolveDarkModeFromObject(utility, out isDarkMode))
                    {
                        return true;
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryResolveDarkModeFromObject(object target, out bool isDarkMode)
            {
                isDarkMode = false;
                if (target == null)
                {
                    return false;
                }

                try
                {
                    var targetType = target.GetType();
                    foreach (var property in targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (property.Name.IndexOf("theme", StringComparison.OrdinalIgnoreCase) < 0
                            && property.Name.IndexOf("dark", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            continue;
                        }

                        var value = property.GetValue(target, null);
                        if (value != null && TryParseTheme(value.ToString(), out isDarkMode))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryResolveDarkModeFromVoiceAttackWindow(out bool isDarkMode)
            {
                isDarkMode = false;
                try
                {
                    var process = Process.GetProcessesByName("VoiceAttack")
                        .FirstOrDefault(candidate => candidate.MainWindowHandle != IntPtr.Zero);
                    if (process == null)
                    {
                        return false;
                    }

                    if (TryGetDwmColor(process.MainWindowHandle, DwmwaCaptionColor, out Color captionColor))
                    {
                        isDarkMode = IsDarkColor(captionColor);
                        return true;
                    }

                    if (TryGetDwmColor(process.MainWindowHandle, DwmwaBorderColor, out Color borderColor))
                    {
                        isDarkMode = IsDarkColor(borderColor);
                        return true;
                    }

                    int darkModeValue;
                    if (DwmGetWindowAttribute(process.MainWindowHandle, DwmwaUseImmersiveDarkMode, out darkModeValue, sizeof(int)) != 0)
                    {
                        if (DwmGetWindowAttribute(process.MainWindowHandle, DwmwaUseImmersiveDarkModeBefore20H1, out darkModeValue, sizeof(int)) != 0)
                        {
                            return false;
                        }
                    }

                    if (darkModeValue == 0)
                    {
                        return false;
                    }

                    isDarkMode = true;
                    return true;
                }
                catch
                {
                }

                return false;
            }

            private static bool TryGetDwmColor(IntPtr windowHandle, int attribute, out Color color)
            {
                color = default(Color);
                try
                {
                    int colorRef;
                    if (DwmGetWindowAttribute(windowHandle, attribute, out colorRef, sizeof(int)) != 0)
                    {
                        return false;
                    }

                    if (colorRef == DwmColorDefault)
                    {
                        return false;
                    }

                    byte r = (byte)(colorRef & 0xFF);
                    byte g = (byte)((colorRef >> 8) & 0xFF);
                    byte b = (byte)((colorRef >> 16) & 0xFF);
                    color = Color.FromRgb(r, g, b);
                    return true;
                }
                catch
                {
                }

                return false;
            }

            private static bool IsDarkColor(Color color)
            {
                int brightness = (color.R * 299 + color.G * 587 + color.B * 114) / 1000;
                return brightness < 128;
            }

            private static bool TryResolveDarkModeFromVoiceAttackRegistry(out bool isDarkMode)
            {
                isDarkMode = false;
                try
                {
                    var keyPaths = new[]
                    {
                        @"Software\VoiceAttack",
                        @"Software\VoiceAttack\Settings"
                    };

                    foreach (var keyPath in keyPaths)
                    {
                        using (var registryKey = Registry.CurrentUser.OpenSubKey(keyPath))
                        {
                            if (registryKey == null)
                            {
                                continue;
                            }

                            foreach (var valueName in registryKey.GetValueNames())
                            {
                                if (valueName.IndexOf("theme", StringComparison.OrdinalIgnoreCase) < 0
                                    && valueName.IndexOf("dark", StringComparison.OrdinalIgnoreCase) < 0)
                                {
                                    continue;
                                }

                                var value = registryKey.GetValue(valueName);
                                if (value != null && TryParseTheme(value.ToString(), out isDarkMode))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryResolveDarkModeFromSessionState(dynamic proxy, out bool isDarkMode)
            {
                isDarkMode = false;
                try
                {
                    if (!(proxy?.SessionState is IDictionary sessionState))
                    {
                        return false;
                    }

                    foreach (DictionaryEntry entry in sessionState)
                    {
                        if (!(entry.Key is string key))
                        {
                            continue;
                        }

                        if (key.IndexOf("THEME", StringComparison.OrdinalIgnoreCase) < 0
                            && key.IndexOf("DARK", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            continue;
                        }

                        if (entry.Value != null && TryParseTheme(entry.Value.ToString(), out isDarkMode))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryGetSessionStateBool(dynamic proxy, string key, out bool value)
            {
                value = false;
                if (!TryGetSessionStateValue(proxy, key, out string rawValue))
                {
                    return false;
                }

                return TryParseBoolean(rawValue, out value);
            }

            private static bool TryGetSessionStateTheme(dynamic proxy, string key, out bool value)
            {
                value = false;
                if (!TryGetSessionStateValue(proxy, key, out string rawValue))
                {
                    return false;
                }

                return TryParseTheme(rawValue, out value);
            }

            private static bool TryGetSessionStateValue(dynamic proxy, string key, out string value)
            {
                value = null;

                try
                {
                    var sessionState = proxy.SessionState;
                    if (sessionState == null)
                    {
                        return false;
                    }

                    if (sessionState.ContainsKey(key))
                    {
                        var rawValue = sessionState[key];
                        if (rawValue != null)
                        {
                            value = rawValue.ToString();
                            return true;
                        }
                    }
                }
                catch
                {
                }

                try
                {
                    var rawValue = proxy.SessionState[key];
                    if (rawValue != null)
                    {
                        value = rawValue.ToString();
                        return true;
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryGetTokenBool(dynamic proxy, string token, out bool value)
            {
                value = false;
                if (!TryGetTokenValue(proxy, token, out string rawValue))
                {
                    return false;
                }

                return TryParseBoolean(rawValue, out value);
            }

            private static bool TryGetTokenTheme(dynamic proxy, string token, out bool value)
            {
                value = false;
                if (!TryGetTokenValue(proxy, token, out string rawValue))
                {
                    return false;
                }

                return TryParseTheme(rawValue, out value);
            }

            private static bool TryGetTokenValue(dynamic proxy, string token, out string value)
            {
                value = null;
                try
                {
                    var tokenValue = proxy.Utility.ParseTokens(token);
                    if (!string.IsNullOrWhiteSpace(tokenValue))
                    {
                        value = tokenValue;
                        return true;
                    }
                }
                catch
                {
                }

                return false;
            }

            private static bool TryParseBoolean(string value, out bool parsed)
            {
                parsed = false;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                if (bool.TryParse(value, out parsed))
                {
                    return true;
                }

                if (int.TryParse(value, out int intValue))
                {
                    parsed = intValue != 0;
                    return true;
                }

                return false;
            }

            private static bool TryParseTheme(string value, out bool isDarkMode)
            {
                isDarkMode = false;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                if (TryParseBoolean(value, out isDarkMode))
                {
                    return true;
                }

                if (value.IndexOf("dark", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    isDarkMode = true;
                    return true;
                }

                if (value.IndexOf("light", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    isDarkMode = false;
                    return true;
                }

                return false;
            }

            private void ApplyThemeResources(bool useDarkMode)
            {
                if (useDarkMode)
                {
                    SetBrushResource("UiWindowBackgroundBrush", Color.FromRgb(30, 30, 30));
                    SetBrushResource("UiWindowForegroundBrush", Color.FromRgb(230, 230, 230));
                    SetBrushResource("UiTabControlBackgroundBrush", Color.FromRgb(42, 42, 42));
                    SetBrushResource("UiTabControlBorderBrush", Color.FromRgb(90, 90, 90));
                    SetBrushResource("UiTabItemBackgroundBrush", Color.FromRgb(42, 42, 42));
                    SetBrushResource("UiTabItemSelectedBackgroundBrush", Color.FromRgb(60, 60, 60));
                    SetBrushResource("UiTabItemForegroundBrush", Color.FromRgb(230, 230, 230));
                    SetBrushResource("UiTabItemSelectedForegroundBrush", Color.FromRgb(255, 255, 255));
                    SetBrushResource("UiComboBoxBackgroundBrush", Color.FromRgb(45, 45, 45));
                    SetBrushResource("UiComboBoxForegroundBrush", Color.FromRgb(230, 230, 230));
                    SetBrushResource("UiComboBoxBorderBrush", Color.FromRgb(110, 110, 110));
                    SetBrushResource("UiComboBoxDropdownBackgroundBrush", Color.FromRgb(45, 45, 45));
                    SetBrushResource("UiComboBoxDropdownForegroundBrush", Color.FromRgb(230, 230, 230));
                    SetBrushResource("UiComboBoxDropdownHighlightBackgroundBrush", Color.FromRgb(70, 70, 70));
                    SetBrushResource("UiComboBoxDropdownHighlightForegroundBrush", Color.FromRgb(255, 255, 255));
                    SetBrushResource(SystemColors.WindowBrushKey, Color.FromRgb(45, 45, 45));
                    SetBrushResource(SystemColors.WindowTextBrushKey, Color.FromRgb(230, 230, 230));
                }
                else
                {
                    SetBrushResource("UiWindowBackgroundBrush", Color.FromRgb(240, 240, 240));
                    SetBrushResource("UiWindowForegroundBrush", System.Windows.Media.Colors.Black);
                    SetBrushResource("UiTabControlBackgroundBrush", Color.FromRgb(238, 238, 238));
                    SetBrushResource("UiTabControlBorderBrush", Color.FromRgb(140, 154, 164));
                    SetBrushResource("UiTabItemBackgroundBrush", Color.FromRgb(238, 238, 238));
                    SetBrushResource("UiTabItemSelectedBackgroundBrush", Color.FromRgb(217, 217, 217));
                    SetBrushResource("UiTabItemForegroundBrush", System.Windows.Media.Colors.Black);
                    SetBrushResource("UiTabItemSelectedForegroundBrush", System.Windows.Media.Colors.Black);
                    SetBrushResource("UiComboBoxBackgroundBrush", System.Windows.Media.Colors.White);
                    SetBrushResource("UiComboBoxForegroundBrush", System.Windows.Media.Colors.Black);
                    SetBrushResource("UiComboBoxBorderBrush", Color.FromRgb(140, 154, 164));
                    SetBrushResource("UiComboBoxDropdownBackgroundBrush", System.Windows.Media.Colors.White);
                    SetBrushResource("UiComboBoxDropdownForegroundBrush", System.Windows.Media.Colors.Black);
                    SetBrushResource("UiComboBoxDropdownHighlightBackgroundBrush", Color.FromRgb(60, 127, 177));
                    SetBrushResource("UiComboBoxDropdownHighlightForegroundBrush", System.Windows.Media.Colors.White);
                    SetBrushResource(SystemColors.WindowBrushKey, System.Windows.Media.Colors.White);
                    SetBrushResource(SystemColors.WindowTextBrushKey, System.Windows.Media.Colors.Black);
                }

                Background = Resources["UiWindowBackgroundBrush"] as Brush;
                ApplyTabStyles();
            }

            private void ApplyTabStyles()
            {
                if (tabControl == null)
                {
                    return;
                }

                tabItemBackground = Resources["UiTabItemBackgroundBrush"] as Brush;
                tabItemForeground = Resources["UiTabItemForegroundBrush"] as Brush;
                tabItemSelectedBackground = Resources["UiTabItemSelectedBackgroundBrush"] as Brush;
                tabItemSelectedForeground = Resources["UiTabItemSelectedForegroundBrush"] as Brush;

                tabControl.Background = Resources["UiTabControlBackgroundBrush"] as Brush;
                tabControl.BorderBrush = Resources["UiTabControlBorderBrush"] as Brush;

                tabControl.SelectionChanged -= TabControl_SelectionChanged;
                tabControl.SelectionChanged += TabControl_SelectionChanged;
                UpdateTabItemThemes();
            }

            private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            {
                UpdateTabItemThemes();
            }

            private void UpdateTabItemThemes()
            {
                foreach (var item in tabControl.Items)
                {
                    if (item is System.Windows.Controls.TabItem tabItem)
                    {
                        UpdateTabItemTheme(tabItem);
                    }
                }
            }

            private void UpdateTabItemTheme(System.Windows.Controls.TabItem tabItem)
            {
                if (tabItem == null)
                {
                    return;
                }

                tabItem.BorderBrush = Resources["UiTabControlBorderBrush"] as Brush;
                if (tabItem.IsSelected)
                {
                    tabItem.Background = tabItemSelectedBackground;
                    tabItem.Foreground = tabItemSelectedForeground;
                }
                else
                {
                    tabItem.Background = tabItemBackground;
                    tabItem.Foreground = tabItemForeground;
                }
            }

            private void SetBrushResource(string key, Color color)
            {
                SetBrushResource((object)key, color);
            }

            private void SetBrushResource(object key, Color color)
            {
                if (Resources[key] is SolidColorBrush brush)
                {
                    brush.Color = color;
                }
                else
                {
                    Resources[key] = new SolidColorBrush(color);
                }
            }

            private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {

                //if (State.configwindowthread.IsAlive)
                //{
                try
                {
                    State.activeconfig.Windowrestorelocation = this.RestoreBounds.Location;
                    Settings.ConfigFile.WriteConfigToFile(true);

                    if (State.trainerrunning)
                    {
                        Log.Write("Training mode closed.", Static.Colors.Message);
                        Log.Write("----------------------------------", Static.Colors.Message);
                        /* try
                        {
                            AliasEditor.SpeechTrainer.Terminate();
                        }
                        catch
                        {
                        } */
                        UI.Playsound.Commandcomplete();
                        State.trainerrunning = false;
                    }
                    Message.Text = "";
                    //}

                    State.configwindowopen = false;
                }
                catch
                {

                }
            }
            private void Window_Closed(object sender, EventArgs e)
            {
                State.configwindowopen = false;
            }

            // ------------------------------------------------------------

            // ------------  CONFIG UPDATE HELPERS ------------------------

            // write config file only
            private void ConfigUpdate(object sender, RoutedEventArgs e)
            {
                Settings.ConfigFile.WriteConfigToFile(true);
            }
            // write config and send
            private void ConfigAndServerUpdate(object sender, RoutedEventArgs e)
            {
                DcsClient.SendSettingsChange();
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            // ------------------------------------------------------------


            public string setversionnumber()
            {
                return " Version " + State.versionstring;
            }

            private void setversionnumber1(object sender, EventArgs e)
            {
                labelsub1.Content = setversionnumber();
            }
            private void setversionnumber2(object sender, EventArgs e)
            {
                labelsub2.Content = setversionnumber();
            }
            private void setversionnumber3(object sender, EventArgs e)
            {
                labelsub3.Content = setversionnumber();
            }
            private void setversionnumber4(object sender, EventArgs e)
            {
                labelsub4.Content = setversionnumber();
            }
            private void setversionnumber5(object sender, EventArgs e)
            {
                labelsub5.Content = setversionnumber();
            }
            private void setversionnumber6(object sender, EventArgs e)
            {
                labelsub6.Content = setversionnumber();
            }
            private void setversionnumber7(object sender, EventArgs e)
            {
                labelsub7.Content = setversionnumber();
            }
            private void setversionnumber8(object sender, EventArgs e)
            {
                labelsub8.Content = setversionnumber();
            }
            private void setversionnumber9(object sender, EventArgs e)
            {
                labelsub9.Content = setversionnumber();
            }

            private void setversionnumber10(object sender, EventArgs e)
            {
                labelsub10.Content = setversionnumber();
            }


            private void Alternateupdatebug(object sender, EventArgs e)
            {
                Alternateupdatebug();
            }

            private void SwitchPage_Release(object sender, MouseEventArgs e)
            {

            }

            private void SendDebugCode(object sender, RoutedEventArgs e)
            {
                SendDebugMsg();
            }

            public void SendDebugMsg()
            {
                Client.DcsClient.Message.DebugMsg newdebug = new DcsClient.Message.DebugMsg();
                newdebug.exec = CodeBlock.Text;
                DcsClient.SendDebugCode(newdebug);
            }

            private void InitCodeBlock(object sender, EventArgs e)
            {
                if (State.clientmode.Equals(ClientModes.Debug))
                {
                    CodeBlock.IsEnabled = true;
                    CodeBlock.Visibility = Visibility.Visible;
                    CodeBlock.Text = Properties.Resources.Debug_code;
                }
                else
                {
                    CodeBlock.IsEnabled = false;
                    CodeBlock.Visibility = Visibility.Hidden;
                    CodeBlock.Text = "";
                }
            }

            private void InitDebugBtn(object sender, EventArgs e)
            {
                if (State.clientmode.Equals(ClientModes.Debug))
                {
                    SendButton.IsEnabled = true;
                    SendButton.Visibility = Visibility.Visible;
                }
                else
                {
                    SendButton.IsEnabled = false;
                    SendButton.Visibility = Visibility.Hidden;
                }
            }

            private void InitDebugTab(object sender, EventArgs e)
            {
                if (State.clientmode.Equals(ClientModes.Debug))
                {
                    debugpage.IsEnabled = true;
                    debugpage.Visibility = Visibility.Visible;
                }
                else
                {
                    debugpage.IsEnabled = false;
                    debugpage.Visibility = Visibility.Hidden;
                }
            }

            public void DebugViewState()
            {
                if (State.clientmode.Equals(ClientModes.Debug))
                {
                    debugpage.IsEnabled = true;
                    debugpage.Visibility = Visibility.Visible;
                    SendButton.IsEnabled = true;
                    SendButton.Visibility = Visibility.Visible;
                    CodeBlock.IsEnabled = true;
                    CodeBlock.Visibility = Visibility.Visible;
                    CodeBlock.Text = Properties.Resources.Debug_code;
                }
                else
                {
                    debugpage.IsEnabled = false;
                    debugpage.Visibility = Visibility.Hidden;
                    SendButton.IsEnabled = false;
                    SendButton.Visibility = Visibility.Hidden;

                    CodeBlock.IsEnabled = false;
                    CodeBlock.Visibility = Visibility.Hidden;
                    CodeBlock.Text = "";
                }
            }

            private void DumpState(object sender, MouseButtonEventArgs e)
            {
                Servers.Server.DumpStateToLog();
            }

            private void OpenVaicomLogFile(object sender, MouseButtonEventArgs e)
            {
                try
                {
                    // Construct the path to the VaicomPro.log file
                    string logFilePath = Path.Combine(State.VA_APPS, "VAICOMPRO", "logs", "VAICOMPRO.log");

                    // Ensure the directory exists
                    if (File.Exists(logFilePath))
                    {
                        Log.Write($"Opening log file at: {logFilePath}", Static.Colors.Message);

                        // Open the file location in File Explorer and select the log file
                        Process.Start("explorer.exe", $"/select,\"{logFilePath}\"");
                    }
                    else
                    {
                        Log.Write("The VaicomPro.log file does not exist.", Static.Colors.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write($"Error while opening log file: {ex.Message}", Static.Colors.Warning);
                }
            }

            private void OpenVaicomManual(object sender, MouseButtonEventArgs e)
            {
                try
                {
                    // Construct the path to the VaicomPro manual
                    string manualPath = Path.Combine(State.VA_APPS, "VAICOMPRO", "Documentation", "VAICOM PRO Community User Manual.pdf");

                    Log.Write($"Opening Vaicom manual at: {manualPath}", Static.Colors.Message);

                    // Ensure the file exists
                    if (File.Exists(manualPath))
                    {
                        try
                        {
                            // Use ProcessStartInfo to open the PDF file
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = manualPath,
                                UseShellExecute = true, // Ensures the default PDF viewer is used
                                Verb = "open" // Explicitly instructs the OS to open the file
                            };

                            Process.Start(startInfo);
                            Log.Write("Manual opened successfully.", Static.Colors.Text);
                        }
                        catch (Exception ex)
                        {
                            Log.Write($"Failed to open manual. Possible issue with default PDF viewer. Error: {ex.Message}", Static.Colors.Warning);
                        }
                    }
                    else
                    {
                        Log.Write("The VaicomPro manual does not exist.", Static.Colors.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write($"Unexpected error while attempting to open manual: {ex.Message}", Static.Colors.Warning);
                }
            }

            private void OpenYouTubeTutorials(object sender, MouseButtonEventArgs e)
            {
                try
                {
                    Log.Write("Opening YouTube tutorials playlist.", Static.Colors.Message);

                    // Open the YouTube tutorials link in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://www.youtube.com/playlist?list=PLLkY2GByvtC2JxgURCFnobqz8iz3Vqooe",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Log.Write($"Error while opening YouTube tutorials: {ex.Message}", Static.Colors.Warning);
                }
            }
        }

    }
}

