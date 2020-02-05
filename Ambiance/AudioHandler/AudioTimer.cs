using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Ambiance.AudioHandler
{
    public class AudioTimer
    {
        private Timer _timer;
        private Stopwatch _stopwatch;

        public TimeSpan CurrentTime => _stopwatch.Elapsed;

        public bool IsFinished { get; set; }

        public double TotalDuration { get; }

        public AudioTimer(double totalDuration)
        {
            TotalDuration = totalDuration;
            InitializeTimer();
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            
            _timer.Start();

            if (Paused)
            {
                InitializeTimer(CurrentTime.TotalMilliseconds);
            } else if(IsFinished)
            {
                InitializeTimer();
                IsFinished = false;
            }
            Paused = false;
            _stopwatch.Start();
        }

        public void InitializeTimer(double passedTime = 0)
        {
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(FinishedEvent);
            var interval = TotalDuration - passedTime;
            if (interval > 0)
            {
                _timer.Interval = interval;
            }
            else
            {
                _timer.Interval = 1;
            }
            
        }

        public bool Paused { get; set; }

        public void Pause()
        {
            Paused = true;
            _stopwatch.Stop();
            InitializeTimer();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            _timer.Stop();
        }

        public void FinishedEvent(object source, ElapsedEventArgs e)
        {
            IsFinished = true;
        }

    }
}
