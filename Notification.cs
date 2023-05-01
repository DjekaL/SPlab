using System;
using System.Windows;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Don_tKnowHowToNameThis
{
    public class Notification
    {
        Window window;
        public Notification(Window obj)
        {
            window = obj;
        }
        public Notifier Notifier()
        {
            return new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: window,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        }

/*        public void NotifDispose()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0, 15, 5);

            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            notifier.Dispose();
        }*/
    }
}
