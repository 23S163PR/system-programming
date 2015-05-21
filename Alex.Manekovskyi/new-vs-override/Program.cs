using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_vs_override
{
	class A
	{
		public virtual void Do()
		{
			Console.WriteLine("A");
		}
	}

	class B : A
	{
		public override void Do()
		{
			Console.WriteLine("B");
		}
	}

	class C : B
	{
		public new void Do()
		{
			Console.WriteLine("C");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			A a = new A();
			A b = new B();
			A c = new C();

			a.Do();
			b.Do();
			c.Do();
		}
	}
}
