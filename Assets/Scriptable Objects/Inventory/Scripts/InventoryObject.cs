using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public TradableItem[] Items = new TradableItem[20];
    public Vest equipedOutfit;
    public Hat equipedHat;

    public void AddItem(TradableItem _item)
    {
        bool itemAdded = false;

        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] is null)
            {
                Items[i] = _item;
                itemAdded = true;
                return;
            }
        }
        if (!itemAdded)
        {
            //do something if the item isn't added
            Debug.Log("Item not added!");
        }
    }

    public void MoveItem(TradableItem item1, TradableItem item2)
    {

    }
}
