using System;
using BinaryAnalysis.UnidecodeSharp;

namespace Groups
{
    public static class StringExtension
    {
        public static string GetEmail(this string name)
        {
            return String.Format("{0}@gmail.com", name.Replace(' ', '.')).Unidecode();
        }
        public static Gender GetGender(this string name)
        {
            return name.Contains("вна") ? Gender.Woman : Gender.Men;
        }
        public static string GetNameXml(this string name)
        {
            return String.Format("{0}{1}", name,".xml");
        } 
    }
}