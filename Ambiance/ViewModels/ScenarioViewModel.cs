using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Ambiance.Annotations;
using Ambiance.AudioHandler;
using Ambiance.Models;

namespace Ambiance.ViewModels
{
    public class ScenarioViewModel : INotifyPropertyChanged
    {
        private CampaignViewModel _campaignViewModel;
        public Scenario Scenario { get; set; }

        public string Name
        {
            get { return Scenario.Name; }
            set
            {
                if (this.Scenario.Name != value)
                {
                    Scenario.Name = value;
                    this.OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _buttonText;
        private bool _isAtmoPlaying;
        public string ButtonText
        {
            get { return _buttonText; }

            set
            {
                if (_buttonText != value)
                {
                    _buttonText = value;
                    this.OnPropertyChanged(nameof(ButtonText));
                }
            }
        }

        private ICommand _switchAtmosCommand;

        public ICommand SwitchAtmosCommand
        {
            get
            {
                if (_switchAtmosCommand == null)
                {
                    _switchAtmosCommand = new RelayCommand(p => ExecuteSwitchAtmosCommand());
                }
                return _switchAtmosCommand;
            }
        }

        private void ExecuteSwitchAtmosCommand()
        {
            if (_isAtmoPlaying)
            {
                foreach (var audio in Atmos)
                {
                    audio.Audio.IsStopped = true;
                    audio.Audio.Stop();
                }
                _isAtmoPlaying = false;
                ButtonText = "Start";
            }
            else
            {

                foreach (var audio in Atmos)
                {
                    audio.Audio.IsStopped = false;
                    audio.Audio.Play();
                }
                _isAtmoPlaying = true;
                ButtonText = "Stop";
            }
        }

        public ObservableCollection<AudioViewModel> Atmos
        {
            get;
        }

        public ObservableCollection<AudioViewModel> Sounds
        {
            get;
        }

        public ScenarioViewModel(Scenario scenario, CampaignViewModel campaignViewModel)
        {
            _buttonText = "Start";
            _isAtmoPlaying = false;
            Scenario = scenario;
            _campaignViewModel = campaignViewModel;

            Atmos = new ObservableCollection<AudioViewModel>();
            foreach (var atmo in scenario.Atmos)
            {
                var audioViewModel = new AudioViewModel(atmo,_campaignViewModel);
                Atmos.Add(audioViewModel);
            }

            Sounds = new ObservableCollection<AudioViewModel>();
            foreach (var sound in scenario.Sounds)
            {
                var audioViewModel = new AudioViewModel(sound, _campaignViewModel);
                Sounds.Add(audioViewModel);
            }

            Atmos.CollectionChanged += AtmoCollectionChanged;
            Sounds.CollectionChanged += SoundsCollectionChanged;
        }
        

        
        private void AtmoCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var tmp = (ObservableCollection<AudioViewModel>)sender;
            var atmo = tmp[0];
            Scenario.Atmos.Add(atmo.Audio);
        }

        private void SoundsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var tmp = (ObservableCollection<AudioViewModel>)sender;
            var sound = tmp[0];
            Scenario.Sounds.Add(sound.Audio);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
