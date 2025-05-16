using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
    public class Athlete
    {
        public string name { get; set; }
        public int year { get; set; }
        public int goldMedals { get; set; }
        public int silverMedals { get; set; }
        public int bronzeMedals { get; set; }

        public override string ToString()
        {
            return $"{name,-30}  {year,-6}  {goldMedals,-5}   {silverMedals,-5}  {bronzeMedals}";
        }
    }
}
