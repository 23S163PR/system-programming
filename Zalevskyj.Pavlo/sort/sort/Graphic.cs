using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    public class Graphic
    {
        private int _startposx;
        private int _startposy;
        private Random _rnd;
        private List<string> _nameSort;
        private int _index;
        public double time { set; get; }

        public Graphic(Random rnd)
        {
            _index = 0;
            _rnd = rnd;
            _startposx = 20;
            _startposy = 100;
            _nameSort = new List<string> { "Buble Sort", "Quick Sort", "Selection Sort", "Merge Sort" };

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 51; i++)
            {
                Console.SetCursorPosition(10, _startposy - i);
                Console.Write("-");
            }
            for (int i = 0; i < 51; i++)
            {
                Console.SetCursorPosition(7, _startposy - i);
                Console.Write("{0}", i * 2);
            }
        }

        public void PaintingGraphic(double Result)
        {
            Console.ForegroundColor = RandColor();
            double x = (Result / time) * 100; // знахожу відцоток 2 числв від 1 

            Console.SetCursorPosition(_startposx - (_nameSort[_index].Length / 2) + 1, _startposy + 4);
            Console.Write(_nameSort[_index]); // водиться імя сортіровкі
            Console.SetCursorPosition(_startposx - (_nameSort[_index].Length / 2) + 1, _startposy + 5);
            Console.Write("{0} мм", Result); // виводиться час в мм
            _index++;

            if (x > 2)
            {
                for (int y = 0; y < ((int)x / 2) + 1; y++)
                {
                    Console.SetCursorPosition(_startposx, _startposy - y);
                    Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
                    Console.SetCursorPosition(_startposx + 1, _startposy - y);
                    Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
                    Console.SetCursorPosition(_startposx + 2, _startposy - y);
                    Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
                }
            }
            else
            {
                Console.SetCursorPosition(_startposx, _startposy);
                Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
                Console.SetCursorPosition(_startposx + 1, _startposy);
                Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
                Console.SetCursorPosition(_startposx + 2, _startposy);
                Console.Write(Encoding.GetEncoding(437).GetChars(new byte[] { 219 })[0]);
            }

            Console.SetCursorPosition(_startposx, _startposy + 2);
            Console.WriteLine("{0} %", x.ToString("0.00"));

            _startposx += 18;
        }

        private ConsoleColor RandColor()
        {
            ConsoleColor color;

            int i = _rnd.Next(0, 5);
            switch (i)
            {
                case 0:
                    color = ConsoleColor.Magenta;
                    break;
                case 1:
                    color = ConsoleColor.Cyan;
                    break;
                case 2:
                    color = ConsoleColor.Green;
                    break;
                case 3:
                    color = ConsoleColor.Red;
                    break;
                case 4:
                    color = ConsoleColor.Yellow;
                    break;
                case 5:
                    color = ConsoleColor.White;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;

            }

            return color;
        }
    }
}
