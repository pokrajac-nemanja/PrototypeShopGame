using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public InventorySlot[] Items = new InventorySlot[20];
    public Vest equipedOutfit;
    public Hat equipedHat;

    private int nextID = 0;

    public void OnEnable()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] is not null && Items[i].item is not null)
            {
                Items[i].ID = nextID;
                nextID++;
            }
        }
    }

    public void AddItem(TradableItem _item)
    {
        bool itemAdded = false;

        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].item is null)
            {
                Items[i].item = _item;
                Items[i].ID = nextID;
                nextID++;
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

    public void RemoveItem()
    {

    }

    public void MoveItem(TradableItem item1, TradableItem item2)
    {

    }

    public void Clear()
    {
        Items = new InventorySlot[20];
        nextID = 0;
    }

    public void DeleteItem(InventorySlot item)
    {
        item.ID = -1;
        item.item = null;
    }

    public void EquipItem(InventorySlot item)
    {
        InventorySlot temp = new InventorySlot();
        Garment garment = (Garment) item.item;
        if (garment.type == GarmentType.Vest)
        {
            temp.item = equipedOutfit;
            equipedOutfit = (Vest) item.item;
            item.item = temp.item;
        } else if (garment.type == GarmentType.Hat)
        {
            temp.item = equipedHat;
            equipedHat = (Hat)item.item;
            item.item = temp.item;
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public TradableItem item;
}