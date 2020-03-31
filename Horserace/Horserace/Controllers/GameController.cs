using System;
using System.Diagnostics;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace Horserace.Controllers
{
    class GameController
    {

        public GameController()
        {
            for (int i = 0; i < 10; i++)
            {
                Ping("google.com");
                Ping("www.facebook.com");
                Ping("www.docs.microsoft.com");
            }

         
            
        }

 



    }
}
