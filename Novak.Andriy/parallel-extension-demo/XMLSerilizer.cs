using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    public static class XSerializer
    {
        public static void XSerilizer<T>(T obj, string fileName)
        {
            try
            {
                if (obj == null) return;
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var fs = new FileStream(string.Format(@"../../Groups Employee/{0}",fileName), FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, obj);
                    fs.Flush();
                }
            }
            catch (SerializationException xe)
            {
                Console.WriteLine(xe.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
