using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Employees
{
	public static class Extension
	{
		private static string ToJson<T>(this T obj) where T : List<Employee>
		{
			var serializer = new DataContractJsonSerializer(typeof (T));
			using (var stream = new MemoryStream())
			{
				serializer.WriteObject(stream, obj);
				return Encoding.Default.GetString(stream.ToArray());
			}
		}

		public static void SaveToJsonFile<T>(this T obj, string path) where T : List<Employee>
		{
			using (var streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(obj.OrderBy(employee => employee.AgeInYears).ToList().ToJson());
				Console.WriteLine("write: {0, -30} - done", path);
			}
		}
	}
}