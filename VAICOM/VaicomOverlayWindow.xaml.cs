using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using VAICOM.Helpers;
using VAICOM.Products;
using VAICOM.Static;

namespace VAICOM
{
    namespace UI
    {
        public partial class VaicomOverlayWindow : Window
        {
            private const int GWL_EXSTYLE = -20;
            private const int WS_EX_NOACTIVATE = 0x08000000;
            private const int WS_EX_TOOLWINDOW = 0x80;

            [DllImport("user32.dll", SetLastError = true)]
            private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
            [DllImport("user32.dll")]
            private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();
            [DllImport("user32.dll")]
            private static extern bool IsIconic(IntPtr hWnd);
            [DllImport("user32.dll")]
            private static extern bool IsWindow(IntPtr hWnd);
            [DllImport("user32.dll")]
            private static extern bool GetWindowRect(IntPtr hWnd, out NativeRect lpRect);

            [StructLayout(LayoutKind.Sequential)]
            private struct NativeRect
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            public static VaicomOverlayWindow Instance { get; private set; }

            // Shared with the PTT/receive code paths so the overlay works without the config window open
            public static volatile bool TxInfoVisible;
            public static volatile string TxInfo1Text = "";
            public static volatile string TxInfo2Text = "";
            public static volatile bool RcvVisible;

            private int okbBlinkTick;
            private DispatcherTimer trackTimer;
            private DispatcherTimer refreshTimer;

            public VaicomOverlayWindow()
            {
                InitializeComponent();
                Instance = this;

                trackTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
                trackTimer.Tick += (s, e) => UpdateOverlay();
                trackTimer.Start();

                refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
                refreshTimer.Tick += (s, e) => RefreshAll();
                refreshTimer.Start();
            }

            // ---------------- DCS window tracking ----------------

            private IntPtr dcsHandle = IntPtr.Zero;
            private bool userVisible;
            // overlay position relative to the top-right corner of the DCS window, in DIPs
            private double offsetRight = 20;
            private double offsetTop = 20;

            private IntPtr FindDcsHandle()
            {
                if (dcsHandle != IntPtr.Zero && IsWindow(dcsHandle)) return dcsHandle;

                dcsHandle = IntPtr.Zero;
                try
                {
                    foreach (string name in new[] { "DCS", "DCS.openbeta" })
                    {
                        foreach (var proc in Process.GetProcessesByName(name))
                        {
                            if (dcsHandle == IntPtr.Zero && proc.MainWindowHandle != IntPtr.Zero)
                            {
                                dcsHandle = proc.MainWindowHandle;
                            }
                            proc.Dispose();
                        }
                        if (dcsHandle != IntPtr.Zero) break;
                    }
                }
                catch
                {
                }
                return dcsHandle;
            }

            private void GetDpiScale(out double scaleX, out double scaleY)
            {
                scaleX = 1.0;
                scaleY = 1.0;
                PresentationSource source = PresentationSource.FromVisual(this);
                if (source != null)
                {
                    scaleX = source.CompositionTarget.TransformToDevice.M11;
                    scaleY = source.CompositionTarget.TransformToDevice.M22;
                }
            }

            // Keeps the overlay glued to the DCS window and visible only while DCS is the foreground window
            private void UpdateOverlay()
            {
                try
                {
                    IntPtr dcs = FindDcsHandle();
                    bool dcsActive = dcs != IntPtr.Zero && !IsIconic(dcs) && GetForegroundWindow() == dcs;

                    if (!userVisible || !dcsActive)
                    {
                        if (IsVisible) Hide();
                        return;
                    }

                    if (!GetWindowRect(dcs, out NativeRect rect)) return;

                    if (!IsVisible)
                    {
                        Show();
                        UpdateLayout();
                    }

                    GetDpiScale(out double scaleX, out double scaleY);
                    PlaceWithinDcs(rect, scaleX, scaleY);
                }
                catch
                {
                    // non-fatal; keep last known position
                }
            }

            private void PlaceWithinDcs(NativeRect rect, double scaleX, double scaleY)
            {
                double dcsLeft = rect.Left / scaleX, dcsRight = rect.Right / scaleX;
                double dcsTop = rect.Top / scaleY, dcsBottom = rect.Bottom / scaleY;

                offsetRight = Math.Max(0, Math.Min(offsetRight, (dcsRight - dcsLeft) - ActualWidth));
                offsetTop = Math.Max(0, Math.Min(offsetTop, (dcsBottom - dcsTop) - ActualHeight));

                Left = dcsRight - ActualWidth - offsetRight;
                Top = dcsTop + offsetTop;
            }

            // ---------------- dragging ----------------
            // The position is stored relative to the DCS window so the overlay can't leave it.

            private bool dragging;
            private Point dragStartMouse;
            private double dragStartOffsetRight, dragStartOffsetTop;

            private void DragHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                try
                {
                    dragging = true;
                    dragStartMouse = PointToScreen(e.GetPosition(this));
                    dragStartOffsetRight = offsetRight;
                    dragStartOffsetTop = offsetTop;
                    Mouse.Capture(this, CaptureMode.Element);
                    e.Handled = true;
                }
                catch { }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                if (!dragging) return;

                IntPtr dcs = FindDcsHandle();
                if (dcs == IntPtr.Zero || !GetWindowRect(dcs, out NativeRect rect)) return;

                // screen coordinates are in device pixels; convert to DIPs
                GetDpiScale(out double scaleX, out double scaleY);
                Point now = PointToScreen(e.GetPosition(this));
                offsetRight = dragStartOffsetRight - (now.X - dragStartMouse.X) / scaleX;
                offsetTop = dragStartOffsetTop + (now.Y - dragStartMouse.Y) / scaleY;
                PlaceWithinDcs(rect, scaleX, scaleY);
            }

            protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
            {
                base.OnMouseLeftButtonUp(e);
                if (!dragging) return;
                dragging = false;
                Mouse.Capture(null);
            }

            protected override void OnSourceInitialized(EventArgs e)
            {
                base.OnSourceInitialized(e);

                // no activation (keeps DCS focused) and tool window (no taskbar button, not listed in alt-tab)
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
            }
            // ---------------- LCD subset refresh ----------------

            public void RefreshAll()
            {
                try
                {
                    RefreshModuleInfo();
                    RefreshBugs();
                }
                catch
                {
                    // avoid crashing the timer loop on transient state issues
                }
            }

            private void RefreshModuleInfo()
            {
                ModuleInfo.Text = Common.GetCurrentModuleDisplayText();
            }

            private void RefreshBugs()
            {
                string modebug1 = State.currentstate.multiplayer ? "MP" : "SP";
                string modebug2 = State.currentstate.vrmode ? "VR" : "DP";
                Databug.Text = modebug1 + " | " + modebug2;

                Easycommsbug.Text = State.currentstate.easycomms ? "AUTO" : "MAN";

                int counter = 0;
                foreach (var set in State.currentstate.availablerecipients)
                {
                    if (!set.Key.Equals("Player"))
                    {
                        counter += set.Value.Count;
                    }
                }
                Counterbug.Text = counter.ToString();

                okbBlinkTick++;
                bool blinkOn = (okbBlinkTick / 4) % 2 == 0;
                Beaconbug.Visibility = State.beaconactive && (State.beaconlocked || blinkOn) ? Visibility.Visible : Visibility.Hidden;
                Transmitbug.Visibility = State.transmitting ? Visibility.Visible : Visibility.Hidden;
                Chatterbug.Visibility = State.chatteractive ? Visibility.Visible : Visibility.Hidden;

                bool okbRunning = State.activeconfig.OpenKneeboard_Out && Extensions.Kneeboard.OpenKneeboardBridge.IsHostRunning;
                // blink (~1s) while the OKB host runs without an active client connection
                bool okbBlinkOn = Extensions.Kneeboard.OpenKneeboardBridge.HasActiveConnection || blinkOn;
                OK_Host.Visibility = (okbRunning && okbBlinkOn) ? Visibility.Visible : Visibility.Hidden;

                RCV_Bug.Visibility = RcvVisible ? Visibility.Visible : Visibility.Hidden;

                bool showTx = TxInfoVisible;
                TXInfo1.Text = TxInfo1Text;
                TXInfo2.Text = TxInfo2Text;
                TXInfo1.Visibility = showTx ? Visibility.Visible : Visibility.Hidden;
                TXInfo2.Visibility = showTx ? Visibility.Visible : Visibility.Hidden;
                ModuleInfo.Visibility = showTx ? Visibility.Hidden : Visibility.Visible;
                EasyCommsInfo.Visibility = showTx ? Visibility.Hidden : Visibility.Visible;
                EasyCommsInfo.Text = "Easy Communication: " + (State.currentstate.easycomms ? "ON" : "OFF");

                bool singleMode = State.currentmodule.Singlehotkey
                    ? !State.activeconfig.ForceMultiHotkey
                    : State.activeconfig.ForceSingleHotkey;
                MultiMode.Visibility = singleMode ? Visibility.Collapsed : Visibility.Visible;
                SingleMode.Visibility = singleMode ? Visibility.Visible : Visibility.Collapsed;
                S2Mbug.Visibility = State.activeconfig.ForceMultiHotkey ? Visibility.Visible : Visibility.Collapsed;
                M2Sbug.Visibility = State.activeconfig.ForceSingleHotkey ? Visibility.Visible : Visibility.Collapsed;

                SRSbug.Text = State.activeconfig.UseSRSmapping ? "SRS" : "";

                Iselbug.Text = (State.activeconfig.UseInstantSelect &&
                    !(State.currentmodule.Equals(DCSmodules.LookupTable["F-5E-3"]) ||
                      State.currentmodule.Equals(DCSmodules.LookupTable["F-5E-3_FC"])))
                    ? "I-SEL" : "";

                Dialbug.Text = (State.currentmodule.Havedial && (State.currentstate.easycomms || State.activeconfig.OperateDial))
                    ? "SLCTR" : "";

                Squelchbug.Visibility = State.activeconfig.DisableSquelch ? Visibility.Hidden : Visibility.Visible;
            }

            // ---------------- lifecycle helpers ----------------

            // Safe to call from any thread. Runs on the config window's UI thread and does nothing while it is closed.
            public static void ShowOverlay()
            {
                try
                {
                    if (!State.configwindowopen || State.configurationwindow == null) return;

                    State.configurationwindow.Dispatcher.BeginInvoke((Action)delegate
                    {
                        if (!State.configwindowopen) return;
                        if (Instance == null)
                        {
                            Instance = new VaicomOverlayWindow();
                        }
                        // the tracking timer shows the window once DCS is the foreground window
                        Instance.userVisible = true;
                        Instance.UpdateOverlay();
                    });
                }
                catch (Exception ex)
                {
                    Log.Write("VAICOM overlay: " + ex.Message, Colors.Warning);
                }
            }

            public static void HideOverlay()
            {
                var overlay = Instance;
                overlay?.Dispatcher.BeginInvoke((Action)delegate
                {
                    overlay.userVisible = false;
                    overlay.Hide();
                });
            }

            public static void CloseOverlay()
            {
                var overlay = Instance;
                if (overlay == null) return;
                if (overlay.Dispatcher.CheckAccess())
                {
                    overlay.Close();
                }
                else
                {
                    overlay.Dispatcher.BeginInvoke((Action)(() => overlay.Close()));
                }
            }

            protected override void OnClosed(EventArgs e)
            {
                trackTimer?.Stop();
                refreshTimer?.Stop();

                if (Instance == this)
                {
                    Instance = null;
                }
                
                base.OnClosed(e);
            }
        }
    }
}