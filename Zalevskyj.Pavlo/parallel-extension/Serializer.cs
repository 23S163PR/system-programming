using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace parallel_extension_demo
{
    public static class Serializer
    {
        public static void Serilizer(Group group, string Name)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(Group));
                using (var fs = new FileStream(string.Format(@"../../Groups/{0}", Name), FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, group);
                    fs.Flush();
                }
            }
            catch (SerializationException xe)
            {
                Console.WriteLine(xe.Message);
            }
        }
    }
}
