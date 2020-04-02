using Horserace.Events;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Horserace.Enums;
using ThreadPool = Windows.System.Threading.ThreadPool;

namespace Horserace.Common
{
    /// <summary>
    ///     Ping is used to calculate the total time sum of x pings to a given server
    /// </summary>
    class Ping
    {
        private readonly string _url; // URL to ping
        private bool _isRunning; // Used to signal the thread to stop
        private int _totalTime; // The time for each ping with the delta from each ping in ms

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="url">URL to ping</param>
        public Ping(string url)
        {
            _url = url;
        }

        public event EventHandler<HorseProgressEventArgs> PingReceived;
        public event EventHandler<FinishedEventArgs> ThreadFinished;

        /// <summary>
        ///     Initializes a thread and starts pinging the specified server
        ///     Calculates the ping and delta of each ping in ms
        /// </summary>
        public void StartPing(int numberOfPings)
        {
            _isRunning = true;
            _totalTime = 0;

            IAsyncAction _pingAction = ThreadPool.RunAsync(async workItem => {
                var previousPingTime = 0;
                var delta = 0;
                var pingIteration = 1;
                while (pingIteration <= numberOfPings && _isRunning)
                {
                    var stopwatch = new Stopwatch();
                    var socket = new StreamSocket();
                    stopwatch.Start();
                    try
                    {
                        await socket.ConnectAsync(new HostName(_url), "80");
                    } catch (Exception e)
                    {
                        OnThreadFinished(FinishType.ERROR);
                        OnThreadFinished(FinishType.FINISHED);
                        return;
                    }

                    stopwatch.Stop();

                    var ms = (int)stopwatch.ElapsedMilliseconds;

                    // First time there is no delta
                    if (pingIteration > 0)
                    {
                        delta = Math.Abs(ms - previousPingTime);
                        previousPingTime = ms;
                    }

                    Interlocked.Add(ref _totalTime, ms + delta);

                    HorseProgressEventArgs horseProgressEventArgs = new HorseProgressEventArgs(_totalTime, pingIteration);
                    OnPingReceived(horseProgressEventArgs);

                    Thread.Sleep(1000);
                    pingIteration++;
                }

                if (!_isRunning)
                {
                    OnThreadFinished(FinishType.CANCELED);
                    return;
                }

                OnThreadFinished(FinishType.FINISHED);
            });
        }


        /// <summary>
        ///     Event triggers whenever a ping is finished
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPingReceived(HorseProgressEventArgs e)
        {
            EventHandler<HorseProgressEventArgs> handler = PingReceived;
            handler?.Invoke(this, e);
        }

        /// <summary>
        ///     Event triggers whenever the thread is finished by itself or canceled by the user
        /// </summary>
        /// <param name="finishType"></param>
        protected virtual void OnThreadFinished(FinishType finishType)
        {
            ThreadFinished?.Invoke(this, new FinishedEventArgs(finishType));
        }

        /// <summary>
        ///     Signals the ping thread to stop
        /// </summary>
        public void StopPing()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Adds extra time to the total ping time (thread safe)
        /// </summary>
        /// <param name="time">time in ms to add</param>
        public void AddTime(int time)
        {
            Interlocked.Add(ref _totalTime, time);
        }
    }
}