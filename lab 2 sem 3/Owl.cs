﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2_sem_3
{
    public class Owl : Bird
    {
        public override string ToString()
        {
            return base.ToString() + "\nRepresentative: Owl\n";
        }
    }
}