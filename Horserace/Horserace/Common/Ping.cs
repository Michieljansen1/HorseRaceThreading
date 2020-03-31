using System;
using System.Diagnostics;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace Horserace.Common
{
    class Ping 
    {
        public Ping(string url)
        {
            
        }

        public async void Ping(string url)
        {

            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            StreamSocket socket = new StreamSocket();
            await socket.ConnectAsync(new HostName(url), "80");
            stopwatch.Stop();

            Debug.WriteLine($"url {url} time: {stopwatch.ElapsedMilliseconds}");

        }
    }
}
