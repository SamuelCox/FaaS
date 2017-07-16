using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Models
{
    public class FileIndexModel
    {
        public string ConnectionString { get; set; }

        private Dictionary<string,List<CloudBlob>> ContainerFiles { get; set; }

        public FileIndexModel()
        {
            ContainerFiles = new Dictionary<string, List<CloudBlob>>();
        }

        public void AddItem(string key, CloudBlob blob)
        {
            if(ContainerFiles.ContainsKey(key))
            {
                ContainerFiles[key].Add(blob);

            }
            else
            {
                ContainerFiles[key] = new List<CloudBlob>();
                ContainerFiles[key].Add(blob);
            }
        }

        public List<CloudBlob> GetContainerItems(string key)
        {
            return ContainerFiles[key];
        }

        public Dictionary<string, List<CloudBlob>> GetAllContainers()
        {
            return ContainerFiles;
        }
    }
}
