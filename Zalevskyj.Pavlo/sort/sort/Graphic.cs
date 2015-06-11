using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort
{
    public class Graphic
    {
        private int _xStartPos = 20;
        private int _yStartPos = 100;

        private int _yPositionNameSord = 104;
        private int _yPositionTimeSord = 105;
        private int _yPositionInterest = 102;

        private int _xPositionScale = 10;
        private int _xPositionNumberScale = 7;

        private int _distanceBetweenColumns = 18;
        private int _columnWidth = 3;

        private Random _rnd;
        private List<string> _nameSort;
        private int _numberNameSortList = 0;

        public double TotalTime { set; get; }

        public Graphic(Random rnd)
        {
            _rnd = rnd;
            _nameSort = new List<string> { "Buble Sort", "Quick Sort", "Selection Sort", "Merge Sort" };

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 51; i++)
            {
                Console.SetCursorPosition(_xPositionScale, _yStartPos - i);
                Console.Write("-"); // шкала
                Console.SetCursorPosition(_xPositionNumberScale, _yStartPos - i);
                Console.Write("{0}", i * 2);  // цифри шкали
            }
        }

        public void PaintingGraphic(double timeSort)
        {
            Console.ForegroundColor = RandColor();

            double interestSort = (timeSort / TotalTime) * 100/* % */;  // знаходжу відсоток числа від

            for (int y = 0; y < ((int)interestSort / 2) + 1; y++)
            {
                for (int i = 0; i < _columnWidth; i++)
                {
                    Console.SetCursorPosition(_xStartPos + i, _yStartPos - y);
                    Console.Write(Encoding.GetEncoding(437 /* █ */).GetChars(new byte[] { 219 })[0]); 
                }
            }

            Console.SetCursorPosition(_xStartPos, _yPositionInterest);
            Console.WriteLine("{0} %", interestSort.ToString("0.00"));

            Console.SetCursorPosition(_xStartPos - (_nameSort[_numberNameSortList].Length / 2) + 1, _yPositionNameSord);
            Console.Write(_nameSort[_numberNameSortList]); // водиться імя сортіровкі

            Console.SetCursorPosition(_xStartPos - (_nameSort[_numberNameSortList].Length / 2) + 1, _yPositionTimeSord);
            Console.WriteLine("{0} мs", timeSort); // виводиться час в мs

            _numberNameSortList++;

            _xStartPos += _distanceBetweenColumns;

        }
        List<int> randomList = new List<int>();

        private ConsoleColor RandColor()
        {
            int numberColor = 0;
            ConsoleColor color;

            do
            {
                numberColor = _rnd.Next(9, 16);
            } while (randomList.Contains(numberColor));

            randomList.Add(numberColor);
            Enum.TryParse(numberColor.ToString(), out color);

            return color;
        }
    }
}
