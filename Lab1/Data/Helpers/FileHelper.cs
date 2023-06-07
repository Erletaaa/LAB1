using Lab1.Models;

namespace Lab1.Data.Helpers
{
    public class FileHelper
    {
        private string _rootFolder;
        public FileHelper(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _rootFolder = hostingEnvironment.WebRootPath;
        }

        public string DownloadFile(PictureGroupEnum fileType, IFormFile file)
        {
            var writeFolder = "";
            if (fileType == PictureGroupEnum.USER)
            {
                writeFolder = "users";
            }
            else if(fileType == PictureGroupEnum.PRODUCT)
            {
                writeFolder = "products";
            }

            string uploads = Path.Combine(_rootFolder, writeFolder);
            if (file.Length > 0)
            {
                var rootFileName = file.FileName.Split(".");
                var fileNameStamp = $"{rootFileName.First()}-{DateTime.Now.ToString().GetHashCode().ToString()}.{rootFileName.Last()}";
                string filePath = Path.Combine(uploads, fileNameStamp);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }

                var splitFilePath = filePath.Split("wwwroot").Last();

                return splitFilePath;
            }

            return null;
        }
    }
}
