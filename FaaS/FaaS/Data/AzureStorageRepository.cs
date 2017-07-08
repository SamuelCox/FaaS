using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using FaaS.Results;

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
        
        public async Task<List<IListBlobItem>> ListAll(string connectionString)
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

            List<IListBlobItem> blobResults = new List<IListBlobItem>();
            foreach (CloudBlobContainer container in containers)
            {
                continuationToken = null;
                do
                {
                    BlobResultSegment segment = await container.ListBlobsSegmentedAsync(continuationToken);
                    continuationToken = segment.ContinuationToken;
                    blobResults.AddRange(segment.Results);
                }
                while (continuationToken != null);
                
            }
            return blobResults;
        }
        
        public async Task Persist(string connectionString, string container, string blobName, MemoryStream memoryStream)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer containerRef = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = containerRef.GetBlockBlobReference(blobName);
            await blob.UploadFromStreamAsync(memoryStream);
        }        

        public async Task<BlobResult> Search(string connectionString, string container, string blobName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer containerRef = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = containerRef.GetBlockBlobReference(blobName);
            MemoryStream stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream);
            return new BlobResult(blob.Properties.ContentType, stream);
        }
    }
}
