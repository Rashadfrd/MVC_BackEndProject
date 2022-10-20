namespace Riode.Utilities.Extensions
{
    public static class FileExtensions
    {
        public static bool CheckFileExtension(this IFormFile file, string key)
        {
            return file.ContentType.Contains(key);
        }
        //public static string CutFileName(this IFormFile file, int maxSize = 60)
        //{
        //    if (file.FileName.Length > maxSize)
        //    {
        //        return file.FileName.Substring(file.FileName.Length - maxSize);
        //    }
        //    return file.FileName;
        //}
        public static bool CheckFileSize(this IFormFile file, int mb)
        {
            return file.Length > mb * 1024 * 1024;
        }

        public static string SaveImage(this IFormFile file, string root, string folder)
        {
            string fileName = Guid.NewGuid() + file.FileName;
            string path = Path.Combine(root, folder, fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

    }
}
