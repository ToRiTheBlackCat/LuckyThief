using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InventorySystem
{
    [Serializable]
    public class Item
    {
        public InventoryItemResource resource;
        public int amount;
        public Item()
        {

        }

        public WeightClass weight => resource.Weight;
        public ItemType type => resource.Type;
        public string Name => resource.name.ToString();

        public bool IsStackable()
        {
            switch (resource.Type)
            {
                case ItemType.Normal:
                    return false;

                case ItemType.Throwable:
                case ItemType.Consumable:
                    return true;
            }

            return false;
        }
    }
}
