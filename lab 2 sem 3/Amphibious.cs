using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Amphibious : IMoveable
    {
        public Amphibious() 
        {
            walkable = new walking();
            swimable = new Swiming();
        }
        protected IWalkable walkable;
        protected ISwimable swimable;

        public void Move()
        {
            walkable.Move();
            swimable.Move();
        }
        
        public override string ToString()
        {
            return  "\nSubclass: Amphibious";
        }
    }
}
