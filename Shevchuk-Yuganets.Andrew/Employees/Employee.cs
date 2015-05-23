using System;
using System.Runtime.Serialization;

namespace Employees
{
	[DataContract]
	public class Employee
	{
		[DataMember]
		public Guid Identity { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public int AgeInYears { get; set; }

		[DataMember]
		public Gender Gender { get; set; }

		[DataMember]
		public decimal Salary { get; set; }
	}

	public enum Gender
	{
		Men,
		Woman,
		Transgender
	}
}