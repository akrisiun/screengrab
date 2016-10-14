using System.Threading;
using System.Windows.Forms;

namespace ScreenGrab.AsyncTasks
{
    class IdScreenTask
    {

        public Screen TheScreen { get; private set; }

        public IdScreenTask(Screen theScreen)
        {
            TheScreen = theScreen;
        }

        public void Run()
        {
            Thread t = new Thread(DoTask);
            
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        private void DoTask()
        {
            var win = new Windows.IdentifyScreen();

            win.Width = TheScreen.Bounds.Width;
            win.Height = TheScreen.Bounds.Height;
            win.Left = TheScreen.Bounds.Left;
            win.Top = TheScreen.Bounds.Top;
            win.Show();
            Thread.Sleep(1000);
        }
    }
}