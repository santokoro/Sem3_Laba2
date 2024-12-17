using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Fish : Animal, ISwimable
    {
        ISwimable _fish;

        public void Swim()
        {
            _fish.Swim();
        }
        public override string ToString()
        {
            return base.ToString() + "\nSubclass: Fish";
        }
    }
}
