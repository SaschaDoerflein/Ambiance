using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Ambiance.AudioHandler;
using Ambiance.Models;

namespace Ambiance.ViewModels
{
    public class MenuViewModel
    {
        private AmbianceViewModel _ambianceViewModel;
        public MenuViewModel(AmbianceViewModel ambianceViewModel)
        {
            _ambianceViewModel = ambianceViewModel;
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(p => ExecuteRefreshCommand());
                }
                return _refreshCommand;
            }
        }

        private void ExecuteRefreshCommand()
        {
            //TODO: How to change viewModel also?
            _ambianceViewModel.CurrentCampaign.Audios = _ambianceViewModel.CurrentCampaign.GetAudiosFromDirectory(_ambianceViewModel.CurrentCampaign.DirectoryPath);
        }
    }
}
