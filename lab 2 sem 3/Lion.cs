using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Lion : Mammals
    {
        public Lion() 
        {
            _animal = new walking();
            __animal = new Swiming();
        }
        public override string ToString()
        {
            return base.ToString() + "\nrepresentative: Lion";
        }
    }
}
