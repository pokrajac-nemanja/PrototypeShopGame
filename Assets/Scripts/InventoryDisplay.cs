using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject inventoryDisplaySlot;

    public int X_START;
    public int Y_START;
    public int NUMBER_OF_COLUMNS;
    public int HORIZONTAL_SPACING;
    public int VERTICAL_SPACING;

    Dictionary<TradableItem, GameObject> itemsDisplayed = new Dictionary<TradableItem, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var slot = Instantiate(inventoryDisplaySlot, transform);
            slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
            slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Container[i].getDisplayIcon();
            slot.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Container[i].getDisplayColor();
            slot.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].basePrice.ToString();
            itemsDisplayed.Add(inventory.Container[i], slot);
        }
    }

    public Vector2 GetPosition(int i)
    {
        return new Vector2(X_START + (HORIZONTAL_SPACING * (i % NUMBER_OF_COLUMNS)), Y_START + (-VERTICAL_SPACING * (i / NUMBER_OF_COLUMNS)));
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (!itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                var slot = Instantiate(inventoryDisplaySlot, transform);
                slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
                slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Container[i].getDisplayIcon();
                slot.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Container[i].getDisplayColor();
                slot.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].basePrice.ToString();
                itemsDisplayed.Add(inventory.Container[i], slot);
            }
        }
    }
}
