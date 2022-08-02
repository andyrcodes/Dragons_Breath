using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public interface IBTAttachmentService
    {
        public Task<byte[]> EncodeAttachment(IFormFile file);

        public string DecodeAttachment(byte[] file, string fileName);

        public string GetFileIcon(string file);

        public string FormatFileSize(long bytes);

    }
}
