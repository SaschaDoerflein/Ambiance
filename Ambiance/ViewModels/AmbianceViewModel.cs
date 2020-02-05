using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Ambiance.IOService;
using Ambiance.Models;

namespace Ambiance.ViewModels
{
    public class AmbianceViewModel
    {
        private GlobalSave _globalSave;

        public Campaign CurrentCampaign { get; set; }

        public MenuViewModel MenuViewModel { get; }
        public CampaignViewModel CampaignViewModel { get; }
        public AmbianceViewModel()
        {
            var globalSavePath = Path.Combine(Directory.GetCurrentDirectory(), "Save.dat");
            _globalSave = GetGlobalSave(globalSavePath);

            CurrentCampaign = FileHandler.ReadFromXmlFile<Campaign>(_globalSave.LastOpenedCampaignPath);
            CampaignViewModel = new CampaignViewModel(CurrentCampaign);

            MenuViewModel = new MenuViewModel(this);
        }

        private GlobalSave GetGlobalSave(string globalSavePath)
        {
            if (File.Exists(globalSavePath))
            {
                var globalSave = FileHandler.ReadFromXmlFile<GlobalSave>(globalSavePath);
                return globalSave;
            }
            else
            {
                var campaignDirectoryPath = Path.GetDirectoryName(globalSavePath);
                var newCampaign = new Campaign(campaignDirectoryPath, "NewCampaign");
                FileHandler.WriteToXmlFile(newCampaign.FullPath, newCampaign);

                var newGlobalSave = new GlobalSave();
                newGlobalSave.LastOpenedCampaignPath = newCampaign.FullPath;
                FileHandler.WriteToXmlFile(globalSavePath, newGlobalSave);

                return newGlobalSave;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ExecuteOnWindowClosing();
        }

        private void ExecuteOnWindowClosing()
        {
            FileHandler.WriteToXmlFile(CurrentCampaign.FullPath, CurrentCampaign);
            FileHandler.WriteToXmlFile(_globalSave.GlobalSavePath, _globalSave);
        }

    }
}
