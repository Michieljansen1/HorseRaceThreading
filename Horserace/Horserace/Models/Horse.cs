﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Horserace.Common;
using Horserace.Events;

namespace Horserace.Models
{

    /**
    *
    */
    class Horse : INotifyPropertyChanged
    {

        private string _name;
        private string _url;
        private int _totalPings;
        private int _distance;
        private Ping _ping;
        public event EventHandler<HorseChangedEventArgs> _horseChanged;

        public Horse(string name, int totalPings, string url)
        {
            _name = name;
            _totalPings = totalPings;
            _url = url;
            _ping = new Ping(url, totalPings);
            _ping._pingReceived += PingReceived;
        }

        private void PingReceived(object sender, HorseProgressReport e)
        {
            Distance = e.TotalTime;
        }

        public void Start()
        {
            _ping.StartPing();
        }

        public string Name => _name;

        public int Distance
        {
            get { return _distance; }
            set { 
                _distance = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = "") {
            _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                new DispatchedHandler(() => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                    OnHorseChanged(new HorseChangedEventArgs(this));
                }));
        }

        protected virtual void OnHorseChanged(HorseChangedEventArgs e) {
            EventHandler<HorseChangedEventArgs> handler = _horseChanged;
            if (handler != null) {
                handler(this, e);
            }
        }
    }
}
