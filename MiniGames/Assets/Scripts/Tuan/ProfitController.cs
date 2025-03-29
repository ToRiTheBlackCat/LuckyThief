using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tuan
{
    public class ProfitController: InteractableScript
    {
        [SerializeField] private int totalProfit = 0;
        [SerializeField] private int MaxProfit;
        private PlayerUIScript _playerUI;

        [SerializeField] private Transform _itemTransform;

        private void Awake()
        {
            _playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUIScript>();
            
            // Get total level's item value
            for (int i = 1; i < _itemTransform.childCount;i++)
            {
                var worldItem = _itemTransform.GetChild(i).GetComponent<ItemWorld>();

                MaxProfit += (int)worldItem.Resource.Value;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var thief = collision.gameObject.GetComponent<ThiefScript>();

            if (thief != null)
            {
                totalProfit += thief.Inventory.ClearInventory();
                _playerUI.ItemValueUpdate(totalProfit * 1f / MaxProfit);
            }
        }

        public override void OnAttachedMinigameSuccess()
        {
            return;
            //base.OnAttachedMinigameSuccess();
        }
    }
}
