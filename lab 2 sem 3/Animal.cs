using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Animal
    {
        protected IMoveable _animal;
        protected ISwimable __animal;

        public void Move()
        {
            _animal.Move();
        }

        public void Swim()
        {
            __animal.Swim();
        }
        public override string ToString()
        {
            return base.ToString() + "\nSubclass: Animal";
        }
    }
}
