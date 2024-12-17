using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Bird : Animal, IFlyable
    {
        protected IFlyable _bird; 

        public void Fly()
        {
            _bird.Fly();
        }

        public override string ToString()
        {
            return base.ToString() + "\nSubclass: Bird";
        }

    }
}
