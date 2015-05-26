
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
namespace ConsoleApplication2
{

    class Employee
    {
            Random rnd;
            public Guid Identity { get; set; }
            public String Name { get; set; }
            public String Email { get; set; }
            public int AgeInYears { get; set; }
            public Gender GenderVal { get; set; }
            public decimal Salary { get; set; }

            public Employee(Guid id, String name, String email, int age, Gender gender, decimal salary)
            {
                this.Identity = id;
                this.Name = name;
                this.Email = email;
                this.AgeInYears = age;
                this.GenderVal = gender;
                this.Salary = salary;
                rnd = new Random();

            }
        public bool Serialize(int group)
        {
            try
            {
              
              System.IO.File.AppendAllText(@"path" + group.ToString() + ".txt", Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                return false;
            }
            return true;

        }

        public enum Gender
        {
            Men,
            Woman,
            Transgender
        }
        public static Gender GetGender()
        {
            int val = new Random().Next(0, 3);
            switch (val)
            {
                case 0: return Gender.Men;
                    break;
                case 1: return Gender.Woman;
                    break;
                case 2: return Gender.Transgender;
                    break;
               
            }
            return Gender.Woman;
        }

        public class HelpTools
        {
            public static List<Employee> GetEmployees(int count)
            {
                List<Employee> list = new List<Employee>();
                Random rnd = new Random();
                for (int i = 0; i < count; i++)
                {
                    list.Add(new Employee(Guid.NewGuid(), Faker.Name.FullName(), Faker.Internet.Email(), rnd.Next(18, 70), Employee.GetGender(), ((decimal)rnd.Next(1000, 50000))));
                }
                return list;
            } // get list of random empolyees 
        }
        public static bool ClearJSON(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                System.IO.File.WriteAllText(@"path" + (i).ToString() + ".txt", String.Empty); // fill all json files with empty string
            }
            return true;
        }


        static void Main(string[] args)
        {

            Stopwatch timer1 = new Stopwatch(); //timer for list w/o AsParalell()
            Stopwatch timerMain = new Stopwatch(); // timer for list GetEmployees w/o AsParalell()
            Stopwatch timer2 = new Stopwatch(); // timer for list w/ AsParalell()

            timerMain.Start();

            List<Employee> list = HelpTools.GetEmployees(10000); // create list of 10000 random employees

            timerMain.Stop();

            Console.WriteLine((double)timerMain.ElapsedTicks / TimeSpan.TicksPerSecond); // show time in seconds that we required to fill 10000 random employess

            timer1.Start();

            List<Employee> firstGroup = list.Where(x => x.AgeInYears > 21 && x.Salary > 15000).OrderBy(x => x.AgeInYears).ToList();
            List<Employee> secondGroup = list.Where(x => x.GenderVal == Gender.Woman && x.Name[0] == 'A').OrderBy(x => x.AgeInYears).ToList();
            List<Employee> thirdGroup = list.Where(x => x.GenderVal == Gender.Men && x.Salary > 20000).OrderBy(x => x.AgeInYears).ToList();
            List<Employee> fourthGroup = list.Where(x => x.GenderVal == Gender.Transgender && x.AgeInYears > 50).OrderBy(x => x.AgeInYears).ToList();
            // grouping w/o AsParalell()
            timer1.Stop();
            
            Console.WriteLine((double)timer1.ElapsedTicks / TimeSpan.TicksPerSecond); // time in seconds to group w/o asParalell()
            
            timer2.Start();
            firstGroup = list.AsParallel().Where(x => x.AgeInYears > 21 && x.Salary > 15000).OrderBy(x => x.AgeInYears).ToList();
            secondGroup = list.AsParallel().Where(x => x.GenderVal == Gender.Woman && x.Name[0] == 'A').OrderBy(x => x.AgeInYears).ToList();
            thirdGroup = list.AsParallel().Where(x => x.GenderVal == Gender.Men && x.Salary > 20000).OrderBy(x => x.AgeInYears).ToList();
            fourthGroup = list.AsParallel().Where(x => x.GenderVal == Gender.Transgender && x.AgeInYears > 50).OrderBy(x => x.AgeInYears).ToList();
            timer2.Stop();

            Console.WriteLine((double)timer2.ElapsedTicks / TimeSpan.TicksPerSecond); // time to group w/ AsParalell()

            List<List<Employee>> FinalList = new List<List<Employee>>()
            {
                firstGroup, secondGroup, thirdGroup, fourthGroup
            };
            Employee.ClearJSON(FinalList.Count);
            for (int i = 0; i < FinalList.Count; i++)
            {
                for (int k = 0; k < FinalList[i].Count; k++)
                {
                    FinalList[i][k].Serialize(i + 1); 
                }
            }

               





        }
    }
}
