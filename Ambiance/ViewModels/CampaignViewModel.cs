using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;
using Ambiance.Models;

namespace Ambiance.ViewModels
{
    public class CampaignViewModel : ViewModel
    {
        private Campaign _campaign;

        public ObservableCollection<ScenarioViewModel> ScenarioViewModels { get; set; }

        public ScenarioViewModel CurrentScenarioViewModel { get; set; }

        public string Name
        {
            get => _campaign.Name;
            set => _campaign.Name = value;
        }

        public ObservableCollection<AudioViewModel> Audios { get; set; }

        public CampaignViewModel(Campaign campaign)
        {
            _campaign = AddScenarioIfEmpty(campaign);

            SynchronizeScenarios();

            SynchronizeAudios();

            ScenarioViewModels.CollectionChanged += ContentCollectionChanged;
        }

        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var tmp = (ObservableCollection<ScenarioViewModel>)sender;
            var scenarioViewModel = tmp[0];
            _campaign.Scenarios.Add(scenarioViewModel.Scenario);
        }

        private void SynchronizeAudios()
        {
            Audios = new ObservableCollection<AudioViewModel>();
            foreach (var audio in _campaign.Audios)
            {
                Audios.Add(new AudioViewModel(audio, this));
            }
        }

        private Campaign AddScenarioIfEmpty(Campaign campaign)
        {
            if (campaign.Scenarios == null || campaign.Scenarios.Count == 0)
            {
                Scenario scenario = new Scenario();
                scenario.Name = "New Scenario";
                campaign.Scenarios.Add(scenario);
                campaign.CurrentScenario = scenario;
            }

            return campaign;
        }

        private void SynchronizeScenarios()
        {
            ScenarioViewModels = new ObservableCollection<ScenarioViewModel>();
            foreach (var scenario in _campaign.Scenarios)
            {
                var scenarioViewModel = new ScenarioViewModel(scenario,this);
                ScenarioViewModels.Add(scenarioViewModel);
            }
            CurrentScenarioViewModel = new ScenarioViewModel(_campaign.CurrentScenario,this);
        }

        

      

    }
}
