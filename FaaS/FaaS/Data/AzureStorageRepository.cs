using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace FaaS.Data
{
    public class AzureStorageRepository : IStorageRepository
    {
        

        public async Task Delete(string connectionString, string container, string blobName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer containerRef = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = containerRef.GetBlockBlobReference(blobName);
            await blob.DeleteAsync();            
        }
        
        public async Task ListAll(string connectionString)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            BlobContinuationToken continuationToken = null;
            List<CloudBlobContainer> containers = new List<CloudBlobContainer>();
            do
            {
                ContainerResultSegment segment = await blobClient.ListContainersSegmentedAsync(continuationToken);
                continuationToken = segment.ContinuationToken;
                containers.AddRange(segment.Results);
            }
            while (continuationToken != null);
        }
        
        public async Task Persist(string connectionString, string container, string blobName, FileStream fileStream)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer containerRef = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = containerRef.GetBlockBlobReference(blobName);
            await blob.UploadFromStreamAsync(fileStream);
        }        

        public void Search(string connectionString, string container, string blobName)
        {
            throw new NotImplementedException();
        }
    }
}
