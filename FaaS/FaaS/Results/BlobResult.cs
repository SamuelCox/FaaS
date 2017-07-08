using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Results
{
    public class BlobResult
    {
        public BlobResult(string contentType, MemoryStream stream)
        {
            ContentType = contentType;
            Stream = stream;
        }

        public readonly string ContentType;

        public readonly MemoryStream Stream;

    }
}
