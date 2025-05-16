using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question1
{
   public class MainClass
    {
        public static void Main(string[] args)
        {
            
            ObservableCollectionExtended<int> intCollection = new ObservableCollectionExtended<int>();
      
            intCollection.AddAll(new List<int> { 1, 2, 3, 4, 5, 2,3,4,5,4,1,32,32,4,5,34,2,3,4,5,3,2,1,2,3,4,5,6,5,4,3,2,1 });
       
            intCollection.RemoveAll(new List<int> { 2, 4 ,6});
     
            Console.WriteLine("Remaining integers in the collection: " +  intCollection);


           




        }
    }
}
