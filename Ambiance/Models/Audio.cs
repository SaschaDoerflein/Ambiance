using System;
using System.Windows.Input;
using Ambiance.AudioHandler;

namespace Ambiance.Models
{
    [Serializable]
    public class Audio

    {
        private NAudioHandler _audioHandler;

        public Audio()
        {
           
        }
        
        public Audio(NAudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
            Path = audioHandler.Path;
            Name = audioHandler.AudioName;
            FileExtension = audioHandler.FileExtension;
            State = AudioState.Stopped;
            
        }

        public double WaitingTime
        {
            get
            {
                if (_audioHandler != null)
                {
                    return AudioHandler.WaitingTime;
                }
                else
                {
                    return 0;
                }
                
            }
            set
            {
                if (_audioHandler != null)
                {
                    AudioHandler.WaitingTime = value;
                }
            }
        }

        public double RandomWaitTime {
            get
            {
                if (_audioHandler != null)
                {
                    return AudioHandler.RandomWaitTime;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (_audioHandler != null)
                {
                    AudioHandler.RandomWaitTime = value;
                }
            }
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public Key Shortcut { get; set; }

        public float Volume
        {
            get
            {
                return AudioHandler.Volume;
            }
            set { AudioHandler.Volume = value; }
        }

        public bool IsStopped
        {
            get
            {
                return AudioHandler.IsStopped;
            }
            set { AudioHandler.IsStopped = value; }
        }

        private NAudioHandler AudioHandler
        {
            get
            {
                if (_audioHandler == null)
                {
                    _audioHandler = new NAudioHandler(FullPath);
                }else if (!_audioHandler.IsInitialized)
                {
                    _audioHandler = new NAudioHandler(FullPath);
                }
                
                return _audioHandler;
            }

            set
            {
                _audioHandler = value;
                Name = _audioHandler.AudioName;
                Path = _audioHandler.Path;
                FileExtension = _audioHandler.FileExtension;
            } 
        }
 
        public string FullPath
        {
            get
            {
                var fullPath = Path + @"\\" + Name+"."+FileExtension;
                return fullPath;
            }
        }
        
        public AudioState State { get; set; }

        public FileExtension FileExtension { get; set; }
        public TimeSpan TimePosition => _audioHandler.TimePosition;
        public TimeSpan TotalDuration => _audioHandler.TotalDuration;

        public bool IsRepeating
        {
            get
            {
                return AudioHandler.IsRepeating;
            }
            set { AudioHandler.IsRepeating = value; }
        }

        public void Play()
        {
            State = AudioState.Play;
            AudioHandler.Play();
        }

        public void Stop()
        {
            State = AudioState.Stopped;
            AudioHandler.Stop();
        }

        public void Pause()
        {
            State = AudioState.Paused;
            AudioHandler.Pause();
        }
       
    }

    public enum AudioState
    {
        Play,
        Paused,
        Stopped
    }
   
}
