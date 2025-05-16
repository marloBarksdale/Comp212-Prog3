
namespace Question1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    public class ObservableCollectionExtended<T>
    {



       private ObservableCollection<T> observableList;
        public ObservableCollectionExtended()
        {
            observableList = new ObservableCollection<T>();
        }




        public void AddAll(List<T> list )
        {
            foreach(var item in list)
            {

               observableList.Add(item);
            }
        }


        public void RemoveAll(List<T> list)
        {
            foreach (var item in list)
            {
                while (observableList.Contains(item))
                {
                    observableList.Remove(item);
                }
            }
        }


        public override string ToString()
        {
            String s = "";
            foreach (var item in observableList)
            {
                s+= item.ToString() + " ";

            }

            return s;
        }

    }
}