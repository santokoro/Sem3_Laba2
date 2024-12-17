using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Chiken : Bird
    {
        
        public Chiken()
        {
            _bird = new NotFly();
        }
        public override string ToString()
        {
            return base.ToString() + "\nrepresentative: chiken";
        }
    }
}
