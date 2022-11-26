using Microsoft.Extensions.Hosting.Internal;

namespace EcommerceMVC.Library
{
    public static class ImageFunctions
    {
        public static string SaveImage(byte[] data, string folderPath)
        {
            HostingEnvironment hostingEnvironment = new HostingEnvironment();
            string fileName = $"{DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ff")}-Image.jpg";
            string path = Path.Combine(folderPath, "Images", fileName);
            File.WriteAllBytes(path, data);
            return fileName;
        }
    }
}
