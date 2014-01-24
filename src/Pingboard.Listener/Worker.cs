using System;
using System.Threading.Tasks;
using PingdomClient;

namespace Pingboard.Listener
{
    public static class Worker
    {
        private static bool _isCancelled;

        private static async void SetInterval(TimeSpan interval, Action actionCallback)
        {
            while (_isCancelled)
            {
                actionCallback.Invoke();
                await Task.Delay(interval);
            }
        }

        private static async void ListenToChecksChanges()
        {
            Context.Checks = await Pingdom.Client.Checks.GetChecksList();
        }

        public static void Start()
        {
            _isCancelled = false;
            Task.Factory.StartNew(() => SetInterval(TimeSpan.FromSeconds(30), ListenToChecksChanges), TaskCreationOptions.LongRunning);
        }
    }
}
