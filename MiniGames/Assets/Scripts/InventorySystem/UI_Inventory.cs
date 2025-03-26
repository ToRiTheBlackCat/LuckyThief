using NUnit.Framework.Interfaces;
using System;
using System.Collections;
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
    private Transform weightCounter;
    private Image warningImage;

    private Color itemSelectedColor;
    private Color warnColorDefault;
    private Color warnColorFade;
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

        weightCounter = transform.Find("WeightCounter");
        warningImage = transform.Find("ImageWarn").GetComponent<Image>();
        warnColorDefault = warningImage.color;
        warnColorFade = new Color(0, 0, 0, 0);
    }


    public void SetInventory(InventoryController controller)
    {
        inventoryController = controller;
        inventoryController.ToolIndexChanged.RemoveAllListeners();
        inventoryController.ToolIndexChanged.AddListener(index => DisplaySelected(index));
        DisplayItems();
        DisplayWeight();
    }

    private Coroutine blinkCoroutine;
    private void DisplayWeight()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        warningImage.enabled = false;

        #region Set weightCounter

        var weightSlider = weightCounter.GetComponent<Slider>();
        weightSlider.value = inventoryController.WeightRatio;

        var fillSprite = weightSlider.transform
            .Find("Fill Area").Find("Fill")
            .GetComponent<Image>();

        var color = Color.white;
        if (weightSlider.value <= .5f)
        {
            color = Color.green;
        }
        else if (weightSlider.value <= .8f)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.red;
            blinkCoroutine = StartCoroutine(Blink());
        }

        fillSprite.color = color;
        #endregion
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
            if (itemData != null)
            {
                RectTransform itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlot.gameObject.SetActive(true);
                itemSlot.anchoredPosition = Vector2.zero;

                Image icon = itemSlot.Find("Icon").GetComponent<Image>();
                icon.sprite = itemData.resource.sprite;

                TextMeshProUGUI textAmount = itemSlot.Find("TextAmount").GetComponent<TextMeshProUGUI>();
                textAmount.text = itemData.amount > 1 ? $"{itemData.amount}" : "";
            }

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
                    //Destroy(clickHandler.gameObject);
                    Debug.Log($"Inventort: {removeAmount} {itemData.Name} Removed. ");

                    ItemWorld.SpawnItemIntoWorld(inventoryController.transform.position, itemData);
                }
                else
                {
                    Debug.LogError($"{name}: Unable to delete Item slot, RemoveItem returns {remove}");
                }
            });

            if (inventoryController.ToolIndexCurrent == i)
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

    private void DisplaySelected(int selectIndex)
    {
        selectIndex++;

        if (!isColapsed)
        {
            for (int index = 1; index < itemSlotContainer.childCount; index++)
            {
                var itemSlot = itemSlotContainer.GetChild(index);
                Image backGround = itemSlot.Find("Background").GetComponent<Image>();

                if (selectIndex == index)
                {
                    backGround.color = itemSelectedColor;
                }
                else
                {
                    backGround.color = Color.white;
                }
            }
        }
        else
        {
            var itemData = inventoryController.CurrentItem;
            var itemSlot = itemSlotContainer.GetChild(1);

            Image icon = itemSlot.Find("Icon").GetComponent<Image>();
            icon.sprite = itemData.resource.sprite;
        }
    }


    IEnumerator Blink()
    {
        warningImage.enabled = true;
        var blinkCounter = 0;

        while (blinkCounter <= 5)
        {
            warningImage.color = warnColorFade;
            yield return new WaitForSeconds(.15f);
            warningImage.color = warnColorDefault;
            yield return new WaitForSeconds(.15f);

            blinkCounter++;
        }
    }
}
