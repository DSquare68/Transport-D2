using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport_D_2
{
    class Program
    {
        static void Main(string[] args)
        {
            TextDescriptor text = new TextDescriptor();
            String line="";
            while (true)
            {
                line = Console.ReadLine();
                if (line.ToLower().Equals("q") || line.ToLower().Equals("quit")) break;
                text.setText(line);
                text.descriptAndRun();
            }
        }
    }
}
