using System;
using System.Threading;
using System.Threading.Tasks;
using PingdomClient;

namespace Pingboard.Listener
{
    public static class Worker
    {
        private static bool _isRunning;

        private static async Task RunInterval(TimeSpan interval, Action actionCallback)
        {
            var failMultiplier = 0;

            const int failIncrement = 10;

            while (_isRunning)
            {
                failMultiplier = await TryInvokeCallback(interval, actionCallback, failIncrement, failMultiplier);
            }
        }

        private static async Task<int> TryInvokeCallback(TimeSpan interval, Action actionCallback, int failIncrement, int failMultiplier)
        {
            try
            {
                actionCallback.Invoke();
                failMultiplier = 0;
            }
            catch (Exception)
            {
                failMultiplier++;
                Thread.Sleep(TimeSpan.FromSeconds(failMultiplier * failIncrement));
            }

            await Task.Delay(interval);

            return failMultiplier;
        }

        private static async void ListenToChecksChanges()
        {
            Context.Checks = await Pingdom.Client.Checks.GetChecksList();
        }

        public static void Start()
        {
            _isRunning = true;
            Task.Factory.StartNew(() => RunInterval(TimeSpan.FromSeconds(30), ListenToChecksChanges), TaskCreationOptions.LongRunning);
        }
    }
}
