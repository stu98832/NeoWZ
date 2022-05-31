namespace NeoWZ.Utils
{
    public class WzPath
    {
        public static string GetImageFilePath(string path) {
            int splitIndex = path.IndexOf(".img");
            return splitIndex == -1 ? "" : path.Substring(0, splitIndex + 4).Replace('\\', '/').Trim('/').Trim();
        }

        public static string GetPropertyPath(string path) {
            int splitIndex = path.IndexOf(".img");
            return splitIndex == -1 ? "" : path.Substring(splitIndex + 5).Replace('\\', '/').Trim('/').Trim();
        }
    }
}
