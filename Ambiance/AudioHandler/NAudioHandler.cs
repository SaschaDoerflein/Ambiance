using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Timers;
using NAudio.Wave;

namespace Ambiance.AudioHandler
{
    public class NAudioHandler
    {
        private IWavePlayer _waveOutDevice;
        private WaveChannel32 _outputChannel;
        private Timer _timer;
        private Random _randomGenerator;

        public NAudioHandler()
        {
            _randomGenerator = new Random();
        }

        public NAudioHandler(string audioFilePath)
        {
            Path = System.IO.Path.GetDirectoryName(audioFilePath);
            AudioName = System.IO.Path.GetFileNameWithoutExtension(audioFilePath);

            Initialize(audioFilePath);
        }

        public void Initialize(string audioFilePath)
        {
            _randomGenerator = new Random();
            
            var fileExtensionString = System.IO.Path.GetExtension(audioFilePath);

            foreach (var supportedFileExtension in SupportedFileExtensions)
            {
                if (string.Equals(fileExtensionString, supportedFileExtension.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    FileExtension = supportedFileExtension;
                    break;
                }
            }

            this._outputChannel = InitInputStream(audioFilePath);
            this._waveOutDevice = InitOutDevice(this._outputChannel);

            _audioTimer = new AudioTimer(TotalDuration.TotalMilliseconds);
            IsInitialized = true;
        }

        public double WaitingTime { get; set; }
        public double RandomWaitTime { get; set; }

        public string Path { get; }
        public string AudioName { get; }
        public FileExtension FileExtension { get; set; }

        public ImmutableArray<FileExtension> SupportedFileExtensions
        {
            get
            {
                return ImmutableArray.Create(new FileExtension[] { FileExtension.Mp3, FileExtension.Wav });
            }
        }

        public TimeSpan TotalDuration => _outputChannel?.TotalTime ?? TimeSpan.Zero;

        public TimeSpan TimePosition
        {
            get
            {
                return _audioTimer.CurrentTime;
            }
        }

        private AudioTimer _audioTimer;

        public void Play()
        {
            _audioTimer.Start();
            _waveOutDevice?.Play();

        }

        public void Pause()
        {
            _audioTimer.Pause();
            _waveOutDevice?.Pause();
        }

        public void Stop()
        {
            _audioTimer.Stop();
            _waveOutDevice?.Stop();
            _outputChannel.CurrentTime = TimeSpan.Zero;
        }

        public bool IsRepeating { get; set; }
        public bool IsStopped { get; set; }
        public bool IsInitialized { get; set; }

        private WaveChannel32 InitInputStream(string audioFilePath)
        {
            var readerStream = CreateReaderStream(audioFilePath);
            if (readerStream == null)
            {
                throw new InvalidOperationException("Unsupported extension");
            }

            var volumeStream = new WaveChannel32(readerStream);
            volumeStream.PadWithZeroes = false;
            return volumeStream;
        }

        private WaveStream CreateReaderStream(string audioFilePath)
        {
            WaveStream readerStream = null;
            if (audioFilePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new WaveFileReader(audioFilePath);
                if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
                {
                    readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                    readerStream = new BlockAlignReductionStream(readerStream);
                }
            }
            else if (audioFilePath.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                readerStream = new Mp3FileReader(audioFilePath);
            }
            return readerStream;
        }

        private IWavePlayer InitOutDevice(IWaveProvider volumeStream)
        {
            var waveOutDevice = new WaveOut();
            waveOutDevice.Init(volumeStream);
            waveOutDevice.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlaybackStoppedEvent);
           

        return waveOutDevice;
        }

        private void PlaybackStoppedEvent(object sender, StoppedEventArgs e)
        {
            Stop();
            if (IsRepeating && _audioTimer.IsFinished || !IsStopped && IsRepeating)
            {
                _timer = new Timer();
                _timer.Elapsed += new ElapsedEventHandler(PlayAudio);
                _timer.Interval = WaitingTime + _randomGenerator.Next(0, (int)RandomWaitTime+1) +1;
                _timer.Enabled = true;
            }
        }

        public float Volume
        {
            get { return _outputChannel.Volume; }
            set { _outputChannel.Volume = value; }
        }


        public void PlayAudio(object source, ElapsedEventArgs e)
        {
            Play();
            _timer.Dispose();
        }

        public void Dispose()
        {
            _waveOutDevice?.Stop();
            _outputChannel?.Close();
            _outputChannel = null;
            _waveOutDevice?.Dispose();
            _waveOutDevice = null;

            GC.Collect();
        }

        
    }

    public enum FileExtension
    {
        Mp3,
        Wav
    }
}

