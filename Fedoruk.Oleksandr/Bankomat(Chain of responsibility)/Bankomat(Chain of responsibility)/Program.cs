using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat_Chain_of_responsibility_
{
    public class Dispenser
    {
        protected Dispenser successor;

        public void SetSuccessor(Dispenser successor)
        {
            this.successor = successor;
        }

        public virtual void GetMoney(int nMoney)
        {
            if (nMoney % 10 != 0)
            {
                Console.WriteLine("Invalid data");
                return;
            }
            else if (successor != null)
            {
                successor.GetMoney(nMoney);
            }
        }
    }

    class Nominal : Dispenser
    {
        int _nominal = 50;

        public Nominal(int Nominal){
            _nominal = Nominal;
        }

        public override void GetMoney(int nMoney)
        {
            var nNominal = (int)(nMoney / _nominal);
            if (nNominal > 0)
            {
                nMoney -= nNominal * _nominal;
                Console.WriteLine("{0} : {1}",
                  _nominal, nNominal);
            }
            if (nMoney > 0 && successor != null)
            {
                successor.GetMoney(nMoney);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            int nMoney = 280;

            Dispenser h1 = new Nominal(50);
            Dispenser h2 = new Nominal(20);
            Dispenser h3 = new Nominal(10);

            var dispenser = new Dispenser();
            dispenser.SetSuccessor(h1);
            h1.SetSuccessor(h2);
            h2.SetSuccessor(h3);


            dispenser.GetMoney(nMoney);
        }
    }
}
