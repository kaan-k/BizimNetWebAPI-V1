using System.IO;

namespace Business.Constants
{
    public class PathConstant
    {
        public static string DocumentFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "Uploads" + Path.DirectorySeparatorChar + "DocumentFile" + Path.DirectorySeparatorChar;
    }
}
