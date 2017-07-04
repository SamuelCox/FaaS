using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Data
{
    interface IStorageRepository
    {

        Task Persist(string connectionString, string container, string blobName, FileStream fileStream);
        

        Task Delete(string connectionString, string container, string blobName);
        

        void Search(string connectionString, string container, string blobName);

        Task ListAll(string connectionString);


    }
}
