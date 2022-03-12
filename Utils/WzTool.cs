namespace NeoMS.Wz.Utils
{
    public class WzTool
    {
        /// <summary> </summary>
        public static string GetImageFilePath(string path) {
            int splitIndex = path.IndexOf(".img");
            return splitIndex == -1 ? "" : path.Substring(0, splitIndex + 4);
        }

        /// <summary> </summary>
        public static string GetPropertyPath(string path) {
            int splitIndex = path.IndexOf(".img");
            return splitIndex == -1 ? "" : path.Substring(splitIndex + 5);
        }
    }
}
