using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Transport_D_2
{
    class TextDescriptor
    {
        private String[] options = { "all" };
        String text;
        public TextDescriptor()
        {

        }
        public void SetText(String text)
        {
            this.text = text;
        }
        public void DescriptAndRun()
        {
            StringReader reader = new StringReader(text);
            String[] words = reader.ReadToEnd().Split(' ');
            switch (words[0])
            {
                case "transport":
                    if (words[1].Equals("-info"))
                    {

                    }
                    else
                    {
                        DescriptTransport(words);
                    }
                    break;
                case "drivers":
                    if (words.Length == 1) Method.drivers(false); else if (words[1].Equals("-details")) Method.drivers(true); else Console.WriteLine("Zła dyrektywa.\nSpróbuj: drives -details");
                    break;
                case "show":
                    if (words.Length == 2) Method.show(words[1]); else Console.WriteLine("Wrong arguments number.\n Try: show [source_folder_name]");
                    break;
                case "episodes":
                    if (words.Length == 3) Method.episodes(words[1], words[2]); else Console.WriteLine("Wrong arguments number.\n Try: episodes [all|nazwa] [source_folder_name]");
                    break;
                case "help":
                    Method.help();
                    break;
                default:
                    Console.WriteLine("Wrong word");
                    break;
            }
        }

        private void DescriptTransport(string[] words)
        {
            if (words.Length == 4)
            {
                if (words[1].Equals("-all"))
                {
                    Method.transport(Regex.Replace(words[1],"-",""), words[2], words[3],true,0,0);
                }
                else
                {
                    Method.transport(words[1], words[2], words[3], true, 0, 0);
                }
            }
            else if (words.Length == 6)
            {
                Method.transport(words[1], words[2], words[3], false, Int32.Parse(Regex.Replace(words[2], "-", "")),Int32.Parse(Regex.Replace(words[3], "-", "")));
            }
            else
            {
                Console.WriteLine("Wrong arguments number.\n Try: transport [nazwa]|[-all]|[nazwa --INT --INT] -source_folder_name -destination_path");
            }
        }
    }
}
