using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ambiance.AudioHandler;

namespace Ambiance.IOService
{
    public class FileHandler
    {
        public List<string> GetFilePaths(List<FileExtension> allowedFileExtentions, string path)
        {
            List<string> filePaths = new List<string>();
            foreach (FileExtension allowedFileExtention in allowedFileExtentions)
            {
                filePaths.AddRange(Directory.GetFiles(@path, allowedFileExtention.ToString()));
            }

            return filePaths;
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Directory.GetCurrentDirectory();
            }


            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
