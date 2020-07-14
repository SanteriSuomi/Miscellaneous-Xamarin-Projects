using System.IO;

namespace TodoList.Services.ImagePicker
{
    public class ImagePickerData
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}