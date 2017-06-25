using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Data
{
    interface IStorageRepository
    {

        void Persist();
        

        void Delete();
        

        void Search();

        void ListAll();


    }
}
