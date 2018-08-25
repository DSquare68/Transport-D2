using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Transport_D_2
{
    class Method
    {
        public static void drivers(bool details)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true && details)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }

        internal static void transport(string name, string source, string dest)
        {
            String path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path + "\\" + source);
            FileInfo[] filesInfo = d.GetFiles();
            DirectoryInfo[] directoryInfo = d.GetDirectories();
            if (filesInfo.Length == 0)
            {
                int k = 0;
                foreach(DirectoryInfo info in directoryInfo)
                {
                    if (info.Name.Contains(name))
                    {
                        k++;
                    }
                }
                Counter.setData(k);
            }else
            {
                int k = 0;
                foreach (FileInfo info in filesInfo)
                {
                    if (info.Name.Contains(name))
                    {
                        k++;
                    }
                }
                Counter.setData(k);
            }
            Thread thread = new Thread(new ThreadStart(Counter.printPercent));
            thread.Start();
            if (name.Equals("all"))
            {
                DirectoryInfo dI = new DirectoryInfo(path + "\\" + source);
                DirectoryInfo[] destDir = dI.GetDirectories();
                if (filesInfo.Length == 0)
                {
                    Counter.setData(directoryInfo.Length);
                    foreach (DirectoryInfo info in directoryInfo)
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(info.FullName, dest + findDest(name,info.Name, destDir) + info.Name);
                        Counter.countNumber++;
                    }
                }
                else
                {
                    Counter.setData(filesInfo.Length);
                    foreach (FileInfo info in filesInfo)
                    {
                        info.MoveTo(path);
                    }
                }
            }
            else if (filesInfo.Length == 0)
            {

                foreach(DirectoryInfo info in directoryInfo)
                {
                    if (info.Name.Contains(name))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(info.FullName, dest+info.Name);
                        Counter.countNumber++;
                    }
                }
            }
            else
            {
                foreach (FileInfo info in filesInfo)
                {
                    if (info.Name.Contains(name))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(info.FullName, dest + info.Name);
                        Counter.countNumber++;
                    }
                }
            }
            thread.Abort();
        }

        private static string findDest(string name, string nameFile, DirectoryInfo[] destDir)
        {
            foreach(DirectoryInfo info in destDir)
            {
                if (info.Name.Contains(name))
                {
                    DirectoryInfo[] seasons = info.GetDirectories();
                    String season = getSeason(nameFile);
                }
                    
            }
            return nameFile + "\\";
        }

        private static string getSeason(string nameFile)
        {
            if (nameFile[0] == '[')
            {
                Console.WriteLine("Anime. I don't know the Season of it. Added in [destinacion]\\[file_name]");
            }
            else
            {
                for (int i = 0; i < nameFile.Length; i++)
                {
                    if (nameFile[i] == 'S' || nameFile[i] == 's')
                    {
                        i++;
                        {
                            return " " + Int32.Parse(nameFile.Substring(i, 2));
                        }
                    }
                }
                return "";
            }
            return "";
        }

        internal static void episodes(string name, string source)
        {
            String path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path + "\\" + source);
            FileInfo[] filesInfo = d.GetFiles();
            DirectoryInfo[] directoryInfo = d.GetDirectories();
            String[] namesFile;
            int i = 0;
            if (filesInfo.Length == 0) { String[] names = new String[directoryInfo.Length]; foreach (DirectoryInfo info in directoryInfo) names[i++] = info.Name; namesFile = showEpisodes(names); } else { String[] names = new String[filesInfo.Length]; foreach (FileInfo info in filesInfo) names[i++] = info.Name; namesFile = showEpisodes(names); }
            if (name.Equals("all"))
            {
                foreach (String s in namesFile)
                {
                    Console.WriteLine("\t" + s);
                }
            }
            else
            {
                foreach (String s in namesFile)
                {
                    if (s.Contains(name)) Console.WriteLine("\t" + s);
                }
            }
        }

        public static void show(String name)
        {
            
            String path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path+"\\"+name);
            FileInfo[] filesInfo = d.GetFiles();
            DirectoryInfo[] directoryInfo = d.GetDirectories();
            String[] namesFile;
            
            if (filesInfo.Length==0)
            {
                String[] names = new String[directoryInfo.Length];
                int i = 0;
                foreach (DirectoryInfo info in directoryInfo) names[i++] = info.Name;
                namesFile = showNames(names);
            }
            else
            {
                String[] names = new String[filesInfo.Length];
                int i = 0;
                foreach (FileInfo info in filesInfo) names[i++] = info.Name;
                namesFile = showNames(names);
            }
            namesFile = namesFile.Distinct().ToArray();
            foreach (String file in namesFile)
            {
                Console.WriteLine("\t"+file);
            }
        }

        private static string[] showNames(String[] filesInfo)
        {
            String[] namesFile = new String[filesInfo.Length];
            int namesCount = 0;
            foreach (String info in filesInfo)
            {
                StringReader reader = new StringReader(info);
                if (reader.Read() == '[')
                {
                    while (reader.Read() != ']')
                    {

                    }
                    String[] names = reader.ReadToEnd().Split(' ');
                    int k = 0;
                    while (!names[k].Equals("-"))
                    {
                        if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                    }
                    namesCount++;

                }
                else
                {
                    if (info.Contains(" "))
                    {
                        reader = new StringReader(info);
                        String[] names = reader.ReadToEnd().Split(' ');
                        int k = 0;
                        while (!(k == names.Length) && !isYear2(names[k]) && !isSeason(names[k]) && !isYear(names[k]))
                        {
                            if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                        }
                        namesCount++;
                    }
                    else
                    {
                        reader = new StringReader(info);
                        String[] names = reader.ReadToEnd().Split('.');
                        int k = 0;
                        while (!(k == names.Length) && !isYear2(names[k]) && !isSeason(names[k]) && !isYear(names[k]))
                        {
                            if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                        }
                        namesCount++;
                    }
                }
            }
            return namesFile;
        }

        private static string[] showEpisodes(String[] filesInfo)
        {
            String[] namesFile = new String[filesInfo.Length];
            int namesCount = 0;
            foreach (String info in filesInfo)
            {
                StringReader reader = new StringReader(info);
                if (reader.Read() == '[')
                {
                    while (reader.Read() != ']')
                    {

                    }
                    String[] names = reader.ReadToEnd().Split(' ');
                    int k = 0;
                    while (!names[k].Equals("-"))
                    {
                        if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                    }
                    namesFile[namesCount] += " "+names[k + 1];
                    namesCount++;

                }
                else
                {
                    bool isSeasonB = false;
                    String episodeNumber = "";
                    if (info.Contains(" "))
                    {
                        reader = new StringReader(info);
                        String[] names = reader.ReadToEnd().Split(' ');
                        int k = 0;
                        while (!(k == names.Length) && !isYear2(names[k]) && !isSeason(names[k]) && !isYear(names[k]))
                        {
                            if (!(k + 1 == names.Length) && isSeason(names[k + 1]))
                            {
                                episodeNumber = getEpisode(names[k + 1]);
                            }
                            if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                        }
                        namesCount++;
                    }
                    else
                    {
                        reader = new StringReader(info);
                        String[] names = reader.ReadToEnd().Split('.');
                        int k = 0;
                       
                        while (!(k == names.Length) && !isYear2(names[k]) && !isSeason(names[k]) && !isYear(names[k]))
                        {
                            if (!(k+1 == names.Length) && isSeason(names[k+1])){
                                episodeNumber = getEpisode(names[k + 1]);
                            }
                            if (k == 0) namesFile[namesCount] = names[k++]; else namesFile[namesCount] += " " + names[k++];
                        
                        }
                        namesFile[namesCount] += episodeNumber;
                        namesCount++;
                    }
                    
                }
            }
            return namesFile;
        }

        private static string getEpisode(string v)
        {
            for(int i=0;i<v.Length;i++)
            {
                if(v[i]=='E'||v[i] == 'e')
                {
                    i++;
                    {
                        return " " + Int32.Parse(v.Substring(i, v.Length-i));
                    }
                }
            }
            return "";
        }

        private static bool isSeason(string names)
        {
            Regex rgx = new Regex(@"[Ss]\d{2}[Ee]\d{2}");
            if (names.Length >= 4)
            {
                return rgx.IsMatch(names);

            }
            else
            {
                return false;
            }
        }

        private static bool isYear2(string names)
        {
            Regex rgx = new Regex(@"\((\d{4})\)");
            if (names.Length >= 4)
            {
                return rgx.IsMatch(names);

            }
            else
            {
                return false;
            }
        }

        private static bool isYear(string names)
        {
            Regex rgx = new Regex(@"\d{4}");
            if (names.Length>=4)
            {
                    return rgx.IsMatch(names);

            }
            else
            {
                return false;
            }
        }

        public static void help()
        {
            Console.WriteLine("trasport-przenosi pliki z source_forder_name do destination_path nazwa- nazwy lub nazwa do przeniesienia w przypadku serialu wszystkie episody all - wszytkie \ndrives - lista urządzeń \nshow - pokazuje nazwy seriali \nepisodes - pokazuje listę odcinków");
        }
    }
}
