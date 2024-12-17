using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Mammals : Animal, IMoveable, ISwimable
    {
        protected IMoveable _mammals;
        protected ISwimable __mammals;

        public void Move()
        {
            _mammals.Move();
        }

        public void Swim()
        {
            __mammals.Swim();
        }
        public override string ToString()
        {
            return base.ToString() + "\nSubclass: Mammals";
        }
    }
}
