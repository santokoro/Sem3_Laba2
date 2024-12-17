using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Сrocodile : Amphibious
    {
        public Сrocodile()
        {
            _amphibious = new walking();
            __amphibious = new Swiming();
        }

        public override string ToString()
        {
           return  base.ToString() + "\nrepresentative: Crocodile";
        }
    }
}
