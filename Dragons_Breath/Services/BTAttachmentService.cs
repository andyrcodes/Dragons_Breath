using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Services
{
    public class BTAttachmentService : IBTAttachmentService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        //Antonio called this ConvertFileToByteArrayAsync
        public async Task<byte[]> EncodeAttachment(IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var output = ms.ToArray();

            ms.Close();
            ms.Dispose();

            return output;
        }

        //ConvertByteArrayToFile
        public string DecodeAttachment(byte[] data, string fileName)
        {
            var base64Data = Convert.ToBase64String(data);
            var ext = Path.GetExtension(fileName);

            return string.Format($"data:image/{ext};base64,{base64Data}");

        }

        public string GetFileIcon(string file)
        {
            string ext = Path.GetExtension(file).Replace(".", "");
            return $"../../img/png/{ext}.png";
        }

        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    }
}
