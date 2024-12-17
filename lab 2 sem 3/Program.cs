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
            
            Chiken chiken = new Chiken();
            Owl owl = new Owl();
            Сrocodile crocodile = new Сrocodile();
            Lion lion = new Lion();


            owl.Fly();
            Console.WriteLine(owl.ToString());
            chiken.Fly();
            Console.WriteLine(chiken.ToString());
            crocodile.Move();
            crocodile.Swim();
            Console.WriteLine(crocodile.ToString());
            lion.Move();
            lion.Swim();
            Console.WriteLine(lion.ToString());


        }
    }
}
