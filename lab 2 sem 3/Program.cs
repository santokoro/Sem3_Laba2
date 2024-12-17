using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Chiken chicken = new Chiken();
            Owl owl = new Owl();
            Сrocodile crocodile = new Сrocodile();
            Lion lion = new Lion();


            List<Animal> Animals = new List<Animal>();
            Animals.Add(crocodile);
            Animals.Add(lion);
            Animals.Add(chicken);
            Animals.Add(owl);

            foreach (Animal a in Animals)
            {
                Console.WriteLine(a.ToString());
                a.Move();

            }


        }
    }
}
