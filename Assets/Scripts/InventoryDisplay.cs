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

    Dictionary<GameObject, TradableItem> itemsDisplayed = new Dictionary<GameObject, TradableItem>();

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
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            var slot = Instantiate(inventoryDisplaySlot, transform);
            slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
            if (inventory.Items[i])
            {
                slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Items[i].getDisplayIcon();
                slot.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Items[i].getDisplayColor();
                slot.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].basePrice.ToString() + "g";
            }
            itemsDisplayed.Add(slot, inventory.Items[i]);
        }
    }

    public void UpdateDisplay()
    {
        int i = 0;
        foreach (KeyValuePair<GameObject, TradableItem> slot in itemsDisplayed)
        {
            if (slot.Value != inventory.Items[i])
            {
                if (inventory.Items[i])
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Items[i].getDisplayIcon();
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Items[i].getDisplayColor();
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].basePrice.ToString() + "g";
                }
                else
                {
                    var baseSlot = inventoryDisplaySlot.transform.GetChild(0).GetComponent<Image>();
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = baseSlot.sprite;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = baseSlot.color;
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
            i++;
        }
    }

    public Vector2 GetPosition(int i)
    {
        return new Vector2(X_START + (HORIZONTAL_SPACING * (i % NUMBER_OF_COLUMNS)), Y_START + (-VERTICAL_SPACING * (i / NUMBER_OF_COLUMNS)));
    }
}
