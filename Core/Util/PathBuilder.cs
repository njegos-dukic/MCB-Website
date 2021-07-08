using System.IO;

namespace Core.Util
{
    public static class PathBuilder
    {
        public const string DefaultProductImage = "defaultProductImage.png";
        public const string DefaultEventImage = "defaultEventImage.png";

        public const string Separator = "_";
        private const string rootImageDirectory = "images";
        private const string productImageDirectoryName = "products";
        private const string eventImageDirectoryName = "events";
        private const string galleryImageDirectoryName = "gallery";

        public static string BuildRelativePathForProductImage(string oldFileName)
            => Path.Combine(rootImageDirectory,
                         productImageDirectoryName,
                         GetRandomFileName() + Path.GetExtension(oldFileName));

        public static string BuildRelativePathForEventImage(string oldFileName)
            => Path.Combine(rootImageDirectory,
                          eventImageDirectoryName,
                          GetRandomFileName() + Path.GetExtension(oldFileName));

        public static string BuildRelativePathOfMediaFileImage(string oldFileName)
            => Path.Combine(rootImageDirectory,
                            galleryImageDirectoryName,
                            GetRandomFileName() + Path.GetExtension(oldFileName));

        public static string BuildApsolutePathForFile(string fileStorageLocation, string relativePath)
            => Path.Combine(fileStorageLocation, relativePath);

        public static string BuildRandomFileNameFromBase64(string base64Content)
        {
            string extension = base64Content.Substring(0, 5) switch
            {
                "IVBOR" => ".png",
                "AAABA" => ".ico",
                _ => ".jpg"
            };

            return $"{GetRandomFileName()}{extension}";
        }

        public static string GetRandomFileName()
            => Path.GetRandomFileName().Replace(".", Separator);
    }
}