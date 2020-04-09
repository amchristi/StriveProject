using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services
{
    public class FileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileService(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        private string GetMimeType(string fileName)
        {
            // Make Sure Microsoft.AspNetCore.StaticFiles Nuget Package is installed
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public FileContentResult GetFile(string filename)
        {
            var filepath = Path.Combine($"{this._hostingEnvironment.WebRootPath}\\AssignmentFiles\\{filename}");

            var mimeType = this.GetMimeType(filename);

            byte[] fileBytes = null;

            if (File.Exists(filepath))
            {
                fileBytes = File.ReadAllBytes(filepath);
            }
            else
            {
                // Code to handle if file is not present
                Console.WriteLine("error");
            }

            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = filename
            };
        }
    }
}
