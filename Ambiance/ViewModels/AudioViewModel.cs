using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Ambiance.Annotations;
using Ambiance.AudioHandler;
using Ambiance.Models;

namespace Ambiance.ViewModels
{
    public class AudioViewModel : INotifyPropertyChanged
    {
        private ICommand _playAudioCommand;
        private ICommand _pauseAudioCommand;
        private ICommand _stopAudioCommand;
        private CampaignViewModel _campaignViewModel;

        public Audio Audio { get; set; }

       

        public string Key
        {
            get
            {
                return Audio.Shortcut.ToString();
            }
            set
            {
                var charArray = value.ToLower().ToCharArray();

                if (charArray.Length > 0)
                {
                    var firstInputChar = charArray[0];

                    Audio.Shortcut = ConvertCharToKey(firstInputChar);
                }
                else
                {
                    Audio.Shortcut = System.Windows.Input.Key.Y;
                }
            }
        }

        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);

        private Key ConvertCharToKey(char inputChar)
        {
            return KeyInterop.KeyFromVirtualKey(VkKeyScan(inputChar));
        }


        public float Volume
        {
            get
            {
                return this.Audio.Volume;
            }
            set
            {
                if (this.Audio.Volume != value)
                {
                    this.Audio.Volume = value;
                    OnPropertyChanged();
                }
            }
        }

        public string WaitingTime
        {
            get
            {
                if (Audio.WaitingTime == 0)
                {
                    return "";
                }
                else
                {
                    return Audio.WaitingTime.ToString().Substring(0, Audio.WaitingTime.ToString().Length - 2);
                }
            }
            set
            {
                var input = $"{value}00";
                Audio.WaitingTime = Double.Parse(input);
            }
        }

        public string RandomWaitingTime
        {
            get
            {
                if (Audio.RandomWaitTime == 0)
                {
                    return "";
                }
                else
                {
                    return Audio.RandomWaitTime.ToString().Substring(0, Audio.RandomWaitTime.ToString().Length - 2);
                }
                
            }
            set
            {
                var input = $"{value}00";
                Audio.RandomWaitTime = Double.Parse(input);
            }
        }



        public string GetIntWithZeors(string number, int zeros)
        {
            string newNumber = "";

            return newNumber;
        }
        
        public string Name { get; set; }

        public bool IsRepeating
        {
            get { return Audio.IsRepeating; }
            set
            {
                if (Audio.IsRepeating == value) return;

                Audio.IsRepeating = value;
                
                this.OnPropertyChanged(nameof(IsRepeating));
            }
        }

        public AudioViewModel(Audio audio, CampaignViewModel campaignViewModel)
        {
            Audio = audio;
            Name = Audio.Name;
            _campaignViewModel = campaignViewModel;
        }

        

        public ICommand PlayAudioCommand
        {
            get
            {
                if (_playAudioCommand == null)
                {
                    _playAudioCommand = new RelayCommand(p => ExecutePlayAudioCommand());
                }
                return _playAudioCommand;
            }
        }

        public ICommand PauseAudioCommand
        {
            get
            {
                if (_pauseAudioCommand == null)
                {
                    _pauseAudioCommand = new RelayCommand(p => ExecutePauseAudioCommand());
                }
                return _pauseAudioCommand;
            }
        }

        public ICommand StopAudioCommand
        {
            get
            {
                if (_stopAudioCommand == null)
                {
                    _stopAudioCommand = new RelayCommand(p => ExecuteStopAudioCommand());
                }
                return _stopAudioCommand;
            }
        }

        private ICommand _addSoundToCurrentScenarioCommand;

        public ICommand AddSoundToCurrentScenarioCommand
        {
            get
            {
                if (_addSoundToCurrentScenarioCommand == null)
                {
                    _addSoundToCurrentScenarioCommand = new RelayCommand(p => ExecuteAddSoundsToCurrentScenarioCommand());
                }
                return _addSoundToCurrentScenarioCommand;
            }
        }

        private void ExecuteAddSoundsToCurrentScenarioCommand()
        {
            var audio = new Audio(new NAudioHandler(this.Audio.FullPath));
            var audioViewModel = new AudioViewModel(audio,this._campaignViewModel);
            _campaignViewModel.CurrentScenarioViewModel.Sounds.Add(audioViewModel);
        }

        private ICommand _addAtmoToCurrentScenarioCommand;

        public ICommand AddAtmoToCurrentScenarioCommand
        {
            get
            {
                if (_addAtmoToCurrentScenarioCommand == null)
                {
                    _addAtmoToCurrentScenarioCommand = new RelayCommand(p => ExecuteAddAtmoToCurrentScenarioCommand());
                }
                return _addAtmoToCurrentScenarioCommand;
            }
        }

        private void ExecuteAddAtmoToCurrentScenarioCommand()
        {
            var audio = new Audio(new NAudioHandler(this.Audio.FullPath));
            var audioViewModel = new AudioViewModel(audio, this._campaignViewModel);
            _campaignViewModel.CurrentScenarioViewModel.Atmos.Add(audioViewModel);
        }




        private void ExecutePlayAudioCommand()
        {
            Audio.Play();
        }

        private void ExecutePauseAudioCommand()
        {
            Audio.Pause();
        }

        private void ExecuteStopAudioCommand()
        {
            Audio.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
