using NUnit.Framework.Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class UI_Inventory : MonoBehaviour
{
    private InventoryController inventoryController;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Toggle toggle;

    private Color itemSelectedColor;
    private bool isColapsed = false;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");

        Color color;
        if (ColorUtility.TryParseHtmlString("#FFF600", out color))
        {
            itemSelectedColor = color;
        }

        toggle = GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener((toggleVal) =>
        {
            isColapsed = !toggleVal;

            var rectBackground = transform.Find("Background");
            var rectColapsed = transform.Find("BackgroundColapsed");
            rectBackground.gameObject.SetActive(!isColapsed);
            rectColapsed.gameObject.SetActive(isColapsed);

            DisplayItems();
        });
    }

    public void SetInventory(InventoryController controller)
    {
        inventoryController = controller;
        DisplayItems();
    }

    private void DisplayItems()
    {
        // Reset inventory UI
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        if (isColapsed)
        {
            var itemData = inventoryController.CurrentItem;
            RectTransform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlot.gameObject.SetActive(true);
            itemSlot.anchoredPosition = Vector2.zero;

            Image icon = itemSlot.Find("Icon").GetComponent<Image>();
            icon.sprite = itemData.resource.sprite;

            TextMeshProUGUI textAmount = itemSlot.Find("TextAmount").GetComponent<TextMeshProUGUI>();
            textAmount.text = itemData.amount > 1 ? $"{itemData.amount}" : "";

            return;
        }

        int x = 0;
        int y = 0;
        float cellSize = 55;
        for (int i = 0; i < inventoryController.Items.Count; i++)
        {
            var itemData = inventoryController.Items[i];

            RectTransform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlot.gameObject.SetActive(true);
            itemSlot.anchoredPosition = new Vector2(x * cellSize, -y * cellSize);

            var clickHandler = itemSlot.GetComponent<ItemSlot>();
            clickHandler.OnLeftClick.AddListener(() =>
            {
                int removeAmount = 0;
                var remove = inventoryController.RemoveItem(itemData, out removeAmount);

                if (remove)
                {
                    Destroy(clickHandler.gameObject);
                    Debug.Log($"Inventort: {removeAmount} {itemData.Name} Removed. ");
                }
                else
                {
                    Debug.LogError($"{name}: Unable to delete Item slot, RemoveItem returns {remove}");
                }
            });

            if (inventoryController.toolBeltIndex == i)
            {
                Image backGround = itemSlot.Find("Background").GetComponent<Image>();
                backGround.color = itemSelectedColor;
            }

            Image icon = itemSlot.Find("Icon").GetComponent<Image>();
            icon.sprite = itemData.resource.sprite;

            TextMeshProUGUI textAmount = itemSlot.Find("TextAmount").GetComponent<TextMeshProUGUI>();
            textAmount.text = itemData.amount > 1 ? $"{itemData.amount}" : "";

            x++;
            if (x % 4 == 0)
            {
                x = 0;
                y++;
            }
        }
    }
}
