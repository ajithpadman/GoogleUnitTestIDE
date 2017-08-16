using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gunit.Utils
{
    public class IOUtils
    {
        public static string getReleativePathWith(string SelectedPath, string rootPath)
        {
            string relPath = "";
            try
            {

                System.Uri path = new Uri(SelectedPath);
                System.Uri cur = new Uri(rootPath + "\\");
                relPath = cur.MakeRelativeUri(path).ToString();
                if (string.IsNullOrWhiteSpace(relPath))
                {
                    relPath = ".";
                }
            }
            catch
            {

            }

            return relPath;
        }
    }
}
