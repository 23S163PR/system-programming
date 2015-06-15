using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace Groups
{
    public static class ManageNames
    {
        private static string _nameXmlName = "Names2.xml";
        private static string _pathSite = @"http://abit-poisk.org.ua/rate2014/direction/{0}";
        private static string _predicateStudentSuccess = "//tr[@class='success']//td[2]";
        private static string _predicateStudentInfo = "//tr[@class='info']//td[2]";

        public static void SeserializingNamesToXml(List<string> listName)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<string>));
            using (var fs = new FileStream(_nameXmlName, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                xmlSerializer.Serialize(fs, listName);
            }
        }

        public static bool CheckFileNames()
        {
            return File.Exists(_nameXmlName); 
        }

        public static List<string> ParseSiteForNamesList(int countPage)
        {
            var nameList = new HashSet<string>();
            Parallel.For(0, countPage+1, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                var doc = new HtmlWeb().Load(string.Format(_pathSite, i));
                var nodesSuccess = doc.DocumentNode.SelectNodes(_predicateStudentSuccess) ;
                var nodesInfo = doc.DocumentNode.SelectNodes(_predicateStudentInfo);
                if (nodesSuccess != null)
                {
                    foreach (var node in nodesSuccess)
                    {
                        nameList.Add(node.InnerText);
                    }
                }
                if (nodesInfo != null)
                {
                    foreach (var node in nodesInfo)
                    {
                        nameList.Add(node.InnerText);
                    }
                }
            });
            return nameList.ToList();
        }

        public static List<string> DeserializingNamesFromXml()
        {
            List<string> names;
            var mySerializer = new XmlSerializer(typeof(List<string>));
            using (var fs = new FileStream(_nameXmlName, FileMode.Open))
            {
                names = (List<string>)mySerializer.Deserialize(fs);
            }
            while (names.Contains(null)) names.Remove(null);
            return names.ToList();
        }
    }
}