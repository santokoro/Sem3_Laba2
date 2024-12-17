using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Сrocodile : Animal
    {
       

        public Сrocodile()
        {
            moveable = new Amphibious();
        }

        public override string ToString()
        {
           return  base.ToString() + "\nrepresentative: Crocodile";
        }
    }
}
