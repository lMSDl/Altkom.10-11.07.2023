using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; }
        private ICollection<string> Items { get; }

        public Garden(int size)
        {
            Size = size < 0 ? 0 : size;
            Items = new List<string>();
        }

        public bool Plant(string plantName)
        {
            if (plantName == null)
                throw new ArgumentNullException(nameof(plantName));
            if (string.IsNullOrWhiteSpace(plantName))
                throw new ArgumentException("Roślina musi posiadać nazwę", nameof(plantName));

            if (Items.Count() >= Size)
                return false;

            if(Items.Contains(plantName))
            {
                plantName = plantName + (Items.Count(x => x.StartsWith(plantName)) + 1);
            }

            Items.Add(plantName);
            return true;
        }

        public IEnumerable<string> GetPlants()
        {
            return Items.ToList();
        }

        public bool Remove(string name)
        {
            if (!Items.Contains(name))
                return false;

            Items.Remove(name);
            return true;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public int Count()
        {
            return Items.Count();
        }
    }
}
