using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ambiance.Models;
using Ambiance.ViewModels;

namespace Ambiance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AmbianceViewModel _ambianceViewModel;

        public MainWindow()
        {
            InitializeComponent();
            SetupBindings();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        private void SetupBindings()
        {
            _ambianceViewModel = new AmbianceViewModel();
            fileMenu.DataContext = _ambianceViewModel.MenuViewModel;
            campaignView.DataContext = _ambianceViewModel.CampaignViewModel;
            campaignView.scenarioView.DataContext = _ambianceViewModel.CampaignViewModel.CurrentScenarioViewModel;

            Closing += _ambianceViewModel.OnWindowClosing;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var sounds = _ambianceViewModel.CurrentCampaign.CurrentScenario.Sounds;
            foreach (var sound in sounds)
            {
                if (e.Key == sound.Shortcut)
                {
                    sound.Play();
                }
            }
        }
    }
}
