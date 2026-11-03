using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using VAICOM.Static;

namespace VAICOM
{
    namespace Interfaces
    {
        // Simulates a mouse click on the Jester wheel text entry field (drawn at the centre of the
        // DCS window) so that the DCS browser page gives it native keyboard focus.
        public static class WSOActivateKeyboardInput
        {
            [StructLayout(LayoutKind.Sequential)]
            private struct NativeRect
            {
                public int Left, Top, Right, Bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct NativePoint
            {
                public int X, Y;
            }

            [DllImport("user32.dll")]
            private static extern bool GetClientRect(IntPtr hWnd, out NativeRect rect);

            [DllImport("user32.dll")]
            private static extern bool ClientToScreen(IntPtr hWnd, ref NativePoint point);

            [DllImport("user32.dll")]
            private static extern bool GetCursorPos(out NativePoint point);

            [DllImport("user32.dll")]
            private static extern bool SetCursorPos(int x, int y);

            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll")]
            private static extern void mouse_event(uint flags, uint dx, uint dy, uint data, UIntPtr extraInfo);

            private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
            private const uint MOUSEEVENTF_LEFTUP = 0x0004;

            private static int clickPending;

            public static void ClickAsync(int delayMs = 150)
            {
                // Ignore repeated requests while a click is already scheduled
                if (Interlocked.Exchange(ref clickPending, 1) == 1)
                {
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        Thread.Sleep(delayMs);
                        Click();
                    }
                    catch (Exception ex)
                    {
                        Log.Write("Jester wheel text entry click failed: " + ex.Message, Colors.Warning);
                    }
                    finally
                    {
                        Interlocked.Exchange(ref clickPending, 0);
                    }
                });
            }

            private static void Click()
            {
                IntPtr hWnd = IntPtr.Zero;
                foreach (Process p in Process.GetProcessesByName("DCS"))
                {
                    if (p.MainWindowHandle != IntPtr.Zero)
                    {
                        hWnd = p.MainWindowHandle;
                        break;
                    }
                }

                // Only click when DCS is the active window, never into another application
                if (hWnd == IntPtr.Zero || GetForegroundWindow() != hWnd)
                {
                    Log.Write("Jester wheel text entry click skipped: DCS is not the foreground window.", Colors.Warning);
                    return;
                }

                NativeRect rect;
                if (!GetClientRect(hWnd, out rect))
                {
                    return;
                }

                NativePoint centre = new NativePoint { X = rect.Right / 2, Y = rect.Bottom / 2 };
                ClientToScreen(hWnd, ref centre);

                NativePoint original;
                bool hasOriginal = GetCursorPos(out original);

                SetCursorPos(centre.X, centre.Y);
                Thread.Sleep(30);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                Thread.Sleep(30);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                Thread.Sleep(30);

                if (hasOriginal)
                {
                    SetCursorPos(original.X, original.Y);
                }
            }
        }
    }
}
