using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Transport_D_2
{
    class Method
    {
        static string[] mediaExtensions = { ".AVI", ".MP4", ".DIVX", ".WMV", ".MKV", ".avi", ".mp4", ".divx", ".wmv", ".mkv" };

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

        internal static void transport(string name, string source, string dest,bool showTransported,int startEpisode, int endEpisode)
        {
            String path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo d = new DirectoryInfo(path + "\\" + source);
            FileInfo[] filesInfo = d.GetFiles();
            DirectoryInfo[] directoryInfo = d.GetDirectories();

            //Counting items to transfer
            if (name.Equals("all"))
            {
                DirectoryInfo dSeriale = new DirectoryInfo(path + "\\" + "Seriale");
                DirectoryInfo dFilmy = new DirectoryInfo(path + "\\" + "Filmy");
                DirectoryInfo dAnime = new DirectoryInfo(path + "\\" + "Anime");

                int k = 0;
                k += sumVideoFrom(dSeriale);
                k += sumVideoFrom(dFilmy);
                k += sumVideoFrom(dAnime);
                Counter.setData(k);


            }
            else
            {
                
                

                if (filesInfo.Length == 0)
                {
                    int k = 0;
                    foreach (DirectoryInfo info in directoryInfo)
                    {
                        if (info.Name.Contains(name))
                        {
                            k++;
                        }
                    }
                    Counter.setData(k);
                }
                else
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
                
            }
            
            Thread thread = new Thread(new ThreadStart(Counter.printPercent));
            thread.Start();
            if(!System.IO.Directory.Exists(dest))
            {
                System.IO.Directory.CreateDirectory(dest);
            }
            if (name.Equals("all"))
            {
               //TODO complete this task
            }
            else if (filesInfo.Length == 0)
            {

                foreach (DirectoryInfo info in directoryInfo)
                {
                    if (info.Name.Contains(name))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(info.FullName, dest + "\\"+info.Name);
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
                        Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(info.FullName, dest + "\\" + info.Name);
                        Counter.countNumber++;
                    }
                }
            }
            thread.Join();
            thread.Abort();
            Console.WriteLine();
        }

        private static int sumVideoFrom(DirectoryInfo d)
        {
            FileInfo[] filesInfo = d.GetFiles();
            DirectoryInfo[] directoryInfo = d.GetDirectories();
            int k = 0;
            if (directoryInfo.Length > 0)
            {
           
                foreach (DirectoryInfo info in directoryInfo)
                {
                    k += sumVideoFrom(info);
                }
            }
            if(filesInfo.Length>0)
            {
                foreach (FileInfo info in filesInfo)
                {
                    if (mediaExtensions.Contains(Path.GetExtension(info.Name), StringComparer.OrdinalIgnoreCase))
                    {
                        k++;
                    }
                }
            }
            return k;
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
            if (filesInfo.Length == 0)
            {
                String[] names = new String[directoryInfo.Length];
                foreach (DirectoryInfo info in directoryInfo)
                    names[i++] = info.Name;
                namesFile = showEpisodes(names);
                foreach (String s in namesFile)
                {
                    Console.WriteLine("\t" + s);
                }
            }
            else
            {
                String[] names = new String[filesInfo.Length];
                foreach (FileInfo info in filesInfo)
                    names[i++] = info.Name;
                namesFile = showEpisodes(names);

                foreach (String s in namesFile)
                {
                    Console.WriteLine("\t" + s);
                }
            }
            if (name.Equals("all"))
            {
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
            if (d == null)
                Console.WriteLine("Directory is missing");
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
            Console.WriteLine("trasport <name> <source_forder_name> <destination_path> \n" +
                                "\t-all - wszytkie pliki z folderów: Anime, Filmy, Seriale " +
                                "\ndrivers [-details] " +
                                "\nshow <source_forder_name> - pokazuje nazwy seriali " +
                                "\nepisodes <name> <source_folder_name> - pokazuje listę odcinków");
        }
    }
}
