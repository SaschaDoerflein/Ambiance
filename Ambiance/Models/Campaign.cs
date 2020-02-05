using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ambiance.AudioHandler;
using Ambiance.ViewModels;

namespace Ambiance.Models
{
    public class Campaign
    { 

        private List<Audio> _playedAudios;

        public List<Audio> PlayedAudios
        {
            get
            {
                var playedAudios = new List<Audio>();
                foreach (var scenario in Scenarios)
                {
                    foreach (var audio in scenario.AllAudios)
                    {
                        if (audio.State == AudioState.Play)
                        {
                            playedAudios.Add(audio);
                        }
                    }
                }

                return playedAudios;
            }

            set => _playedAudios = value;
        }

        public List<Audio> Audios { get; set; }

        public string DirectoryPath { get; 
            set; }
        public string Name { get; set; }

        public string FullPath => Path.Combine(DirectoryPath, $"{Name}.campaign");

        public Campaign()
        {
            PlayedAudios = new List<Audio>();
            Scenarios = new List<Scenario>();
        }

        public Campaign(string directoryPath, string name)
        {
            DirectoryPath = directoryPath;
            Name = name;
            PlayedAudios = new List<Audio>();
            Scenarios = new List<Scenario>();
            Audios = GetAudiosFromDirectory(directoryPath);
        }
        
        public List<Scenario> Scenarios { get; set; }

        public Scenario CurrentScenario { get; set; }
        
        public List<Audio> GetAudiosFromDirectory(string path)
        {
            var filePaths = GetFilePaths(path, new NAudioHandler().SupportedFileExtensions.ToList());

            var audios = CreateAudios(filePaths);

            return audios;
        }

        private static List<Audio> CreateAudios(List<string> filePaths)
        {
            var audios = new List<Audio>();
            foreach (var filePath in filePaths)
            {

                var audio = new Audio(new NAudioHandler(filePath));
                audios.Add(audio);
            }

            return audios;
        }

        private List<string> GetFilePaths(string path, List<FileExtension> supportedFileExtensions)
        {
            var filePaths = new List<string>();
            foreach (var supportedFileExtension in supportedFileExtensions)
            {
                filePaths.AddRange(Directory.GetFiles(path, $"*.{supportedFileExtension.ToString()}", SearchOption.AllDirectories));
            }

            return filePaths;
        }

        

    }
}
