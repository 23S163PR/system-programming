using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class ConvertMemory
    {
        public const float _1KB = 1024;
        public const float _1MB = 1048576;
        public const float _1GB = 1073741824;
        public static string ShorteningMemory(float MemoryInBytes)
        {
            float m = MemoryInBytes;
            string result = m > _1KB && m < _1MB ? (m / _1KB).ToString("F2") + " KB"
                            : m > _1MB && m < _1GB ? (m / _1MB).ToString("F2") + " MB"
                            : (m / _1GB).ToString("F2") + " GB";
            return result;
        }
    }

}
