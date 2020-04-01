using Horserace.Events;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace Horserace.Common
{
    class Ping
    {

        int _totalPings;
        private string _url;
        private int _previousPingTime;
        private int _totalTime = 0;
        private bool _isRunning;
        private IAsyncAction _pingAction;

        public event EventHandler<HorseProgressReport> _pingReceived;
        public event EventHandler<FinishedEventArgs> _threadFinished;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">URL to ping</param>
        /// <param name="totalPings">Total amount of pings to be done</param>
        public Ping(string url, int totalPings)
        {
            _url = url;
            _totalPings = totalPings;
        }

        /// <summary>
        /// Initializes a thread and starts pinging the specified server
        /// Calculates the ping and delta of each ping in ms
        /// </summary>
        public void StartPing()
        {
            _isRunning = true;

            _pingAction = Windows.System.Threading.ThreadPool.RunAsync(async (workItem) =>
            {
                int previousPingTime = 0;
                int delta = 0;
                int i = 0;
                while (i < _totalPings && _isRunning)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    StreamSocket socket = new StreamSocket();
                    stopwatch.Start();
                    await socket.ConnectAsync(new HostName(_url), "80");
                    stopwatch.Stop();

                    int ms = (int)stopwatch.ElapsedMilliseconds;

                    if (i > 0)
                    {
                        delta = Math.Abs(ms - previousPingTime);
                        previousPingTime = ms;
                    }
                    _totalTime += ms + delta;

                    HorseProgressReport horseProgressReport = new HorseProgressReport(_totalTime);
                    OnPingReceived(horseProgressReport);

                    Debug.WriteLine($"url {_url} time: {ms} systemTime: {System.DateTime.Now} delta: {delta} total time: {_totalTime}");
                    Thread.Sleep(1000);
                    i++;
                }

                if (!_isRunning)
                {
                    onThreadFinished(FinishedEventArgs.FinishType.CANCELED);
                    return;
                }

                onThreadFinished(FinishedEventArgs.FinishType.FINISHED);
            });
        }

        /// <summary>
        /// Signals the ping thread to stop
        /// </summary>
        public void StopPing()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Event triggers whenever a ping is finished
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPingReceived(HorseProgressReport e) {
            EventHandler<HorseProgressReport> handler = _pingReceived;
            if (handler != null) {
                handler(this, e);
            }
        }

        /// <summary>
        /// Event triggers whenever the thread is finished by itself or canceled by the user
        /// </summary>
        /// <param name="finishType"></param>
        protected virtual void onThreadFinished(FinishedEventArgs.FinishType finishType) {
            EventHandler<FinishedEventArgs> handler = _threadFinished;
            if (handler != null) {
                handler(this, new FinishedEventArgs(finishType));
            }
        }
    }
}
