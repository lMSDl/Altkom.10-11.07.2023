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
        private readonly ILogger _logger;

        public Garden(int size, ILogger logger)
        {
            _logger = logger;
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
            {
                _logger.Log($"Brak miejsca na {plantName}");
                return false;
            }

            if(Items.Contains(plantName))
            {
                var newName = plantName + (Items.Count(x => x.StartsWith(plantName)) + 1);
                _logger.Log($"Roślina {plantName} zmienia nazwę na {newName}");
                plantName = newName;
            }

            Items.Add(plantName);
            _logger.Log($"Roślina {plantName} została zasadzona");
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
            _logger.Log($"Roślina {name} została usunięta z ogrodu");
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

        public string GetLastLogFromLastHour()
        {
            var log = _logger.GetLogs(DateTime.Now.AddHours(-1), DateTime.Now);
            return log.Split("\n").Last();
        }
    }
}
