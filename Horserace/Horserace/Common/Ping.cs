using System;
using System.Diagnostics;
using System.Threading;
using Windows.AI.MachineLearning;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;

namespace Horserace.Common
{
    class Ping
    {

        int _totalPings;
        private string _url;
        private int _previousPingTime;
        private int _totalTime;

        public Ping(string url, int totalPings)
        {
            _url = url;
            _totalPings = totalPings;
        }
        

        public void StartPing()
        {

            IAsyncAction pingAction = Windows.System.Threading.ThreadPool.RunAsync(async (workItem) =>
            {
                int i = 0;
                while (i < _totalPings)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    StreamSocket socket = new StreamSocket();
                    stopwatch.Start();
                    await socket.ConnectAsync(new HostName(_url), "80");
                    stopwatch.Stop();
                    Debug.WriteLine($"url {_url} time: {stopwatch.ElapsedMilliseconds} systemTime: {System.DateTime.Now}");
                    Thread.Sleep(1000);
                    i++;
                }
            });
        }
    }
}
