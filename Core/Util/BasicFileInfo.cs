namespace Core.Util
{
    /// <summary>
    /// Abstraction for file that needs to be saved on file server
    /// </summary>
    public class BasicFileInfo
    {
        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public BasicFileInfo(string fileName, byte[] fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }

        public BasicFileInfo() { }
    }
}
