using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScreenGrab.AsyncTasks
{
    class GrabDesktopImageTask : IDisposable
    {

        public Screen SelectedScreen { get; private set; }

        public GrabDesktopImageTask(Screen selectedScreen)
        {
            SelectedScreen = selectedScreen;
        }

        private Dispatcher _dispatcher;
        public BitmapSource Image { get; private set; }
        public event EventHandler Finished;

        public void Run()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            Thread t = new Thread(DoTask) { IsBackground = true };
            t.Start();
        }

        void DoTask()
        {
            Thread.Sleep(250);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap screen = new Bitmap(SelectedScreen.Bounds.Width, SelectedScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screen)) {

                //g.CopyFromScreen(0, 0, 0, 0, screen.Size);
                g.CopyFromScreen(SelectedScreen.Bounds.X, SelectedScreen.Bounds.Y, 0, 0, screen.Size);
            }

            Image = BitmapToSource(screen);
            sw.Stop();

            _dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => Finished(this, EventArgs.Empty)));

        }

        private BitmapSource BitmapToSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();
            // System.Windows.Interop.
            BitmapSource destination = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, sizeOptions);
            destination.Freeze();
            return destination;
        }


        public void Dispose()
        {
            Image = null;
        }
    }
}