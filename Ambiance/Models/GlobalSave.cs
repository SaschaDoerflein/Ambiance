using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ambiance.Models
{
    public class GlobalSave
    {
        public string GlobalSavePath => Path.Combine(Directory.GetCurrentDirectory(),"Save.dat");
        public string LastOpenedCampaignPath { get; set; }

        public GlobalSave()
        {
            
        }
    }
}
