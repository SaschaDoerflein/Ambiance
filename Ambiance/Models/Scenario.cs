using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ambiance.Annotations;

namespace Ambiance.Models
{
    public class Scenario { 
        private string _name;

        public Scenario()
        {
            Atmos = new List<Audio>();
            Sounds = new List<Audio>();
        }

        public string Name { get; set; }

        public List<Audio> AllAudios
        {
            get
            {
                List<Audio> allAudios = new List<Audio>();
                allAudios.AddRange(Atmos);
                allAudios.AddRange(Sounds);
                return allAudios;
            }
        }

        public List<Audio> Atmos { get; set; }

        public List<Audio> Sounds { get; set; }

        public void PlayAtmos()
        {
            foreach (var atmo in Atmos)
            {
                atmo.Play();
            }
        }

        public void StopAtmos()
        {
            foreach (var atmo in Atmos)
            {
                atmo.Stop();
            }
        }

    }
}
