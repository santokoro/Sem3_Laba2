using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Amphibious : IMoveable, ISwimable
    {
        protected IMoveable _amphibious;
        protected ISwimable __amphibious;

        public void Move()
        {
            _amphibious.Move();
        }

        public void Swim()
        {
            __amphibious.Swim();
        }
        public override string ToString()
        {
            return base.ToString() + "\nSubclass: Amphibious";
        }
    }
}
