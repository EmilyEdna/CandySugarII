namespace CandySugar.Library.Common.Audio
{
    public class RawResources
    {
        private static byte[] _icon;
        public static byte[] Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = LoadIconAsync().Result;
                }
                return _icon;
            }
        }

        private static byte[] _head;
        public static byte[] Head
        {
            get
            {
                if (_head == null)
                {
                    _head = LoadHeadAsync().Result;
                }
                return _head;
            }
        }

        private static async Task<byte[]> LoadIconAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("icon.png");
            using var reader = new StreamReader(stream);
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        private static async Task<byte[]> LoadHeadAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("head.png");
            using var reader = new StreamReader(stream);
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
