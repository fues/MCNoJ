using System;
using System.IO;

namespace MCNoJ
{
    class RelPath
    {
        public static string GetRootedPathByFile(string origen,string rel)
        {
            if (Path.IsPathRooted(rel))
            {
                return rel;
            }
            else
            {
                Uri cd = new Uri(origen);
                return new Uri(cd, rel).LocalPath;
            }
        }
    }
}
