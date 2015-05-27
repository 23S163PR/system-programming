using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    public static class XSerializer<T>
    {
        private static readonly XmlSerializer XmlSerializer;
        static XSerializer()
        {
            XmlSerializer = new XmlSerializer(typeof(T));
        }

        public static void XSerilizer(T obj, string fileName)
        {
            try
            {
                if (obj == null) return;
                using (var fs = new FileStream(string.Format(@"../../Groups Employee/{0}",fileName), FileMode.OpenOrCreate))
                {
                    XmlSerializer.Serialize(fs, obj);
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
