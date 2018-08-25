using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transport_D_2
{
    class Counter
    {
        public static int countNumber = 0;
        static int allNumber = 0;
        public static void printPercent()
        {
            int a = -1;
            while (true)
            {

                if (a != countNumber)
                {
                    int currentLineCursor = Console.CursorTop;
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("Completed: " + Math.Round((countNumber / (double)allNumber) * 100, 2) + "%", Console.WindowWidth);
                    Console.SetCursorPosition(0, currentLineCursor);
                    a = countNumber;
                    if (countNumber == allNumber) break;
                }
                Thread.Sleep(100);
            }
        }
        public static void setData(int allNumber)
        {
            Counter.countNumber = 0;
            Counter.allNumber = allNumber;
        }
    }
}
