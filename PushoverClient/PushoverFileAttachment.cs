using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushoverClient
{
    public class PushoverFileAttachment
    {
        public PushoverFileAttachment(string fileName, string contentType, Stream stream)
        {
            FileName = fileName;
            ContentType = contentType;
            FileStream = stream;
            Validate();
        }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream FileStream { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                throw new ArgumentException("File attachment FileName is required");
            }            
            if (string.IsNullOrWhiteSpace(ContentType))
            {
                throw new ArgumentException("File attachment ContentType is required");
            }
            if (FileStream == null)
            {
                throw new ArgumentException("File attachment FileStream is required");
            }
            if (!FileStream.CanRead)
            {
                throw new ArgumentException("File attachment FileStream must be readable.");
            }
        }
    }
}