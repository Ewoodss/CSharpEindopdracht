using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class SoftwareSearch
    {
        public static Dictionary<string, string> Result()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<string> inkFiles = new List<string>();

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string localDataStart = appData + "\\Microsoft\\Windows\\Start Menu\\Programs";
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            inkFiles.AddRange(Directory.GetFiles("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\", "*.lnk", SearchOption.AllDirectories));
            inkFiles.AddRange(Directory.GetFiles(localDataStart, "*.lnk", SearchOption.AllDirectories));
            inkFiles.AddRange(Directory.GetFiles(desktopFolder, "*.lnk", SearchOption.AllDirectories));

            inkFiles = inkFiles.Distinct().ToList();

            WshShell shell = new WshShell();

            foreach (string inkFile in inkFiles)
            {
                WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(inkFile);

                if (shortcut.TargetPath.ToLower().Contains(".exe"))
                {
                    result.TryAdd(inkFile.Split('\\').Last().Split('.').First(), shortcut.TargetPath);
                }
            }

            return result;
        }
    }
}
