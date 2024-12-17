using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public abstract class Animal: IMoveable
    {
        protected  IMoveable moveable;

        public void Move()
        {
            moveable.Move();
        }

        public override string ToString()
        {
            return "\nClass: Animal";
        }
    }
}
