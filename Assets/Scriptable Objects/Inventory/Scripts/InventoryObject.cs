using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<TradableItem> Container = new List<TradableItem>();
    public Vest equipedOutfit;
    public Hat equipedHat;

    public void AddItem(TradableItem _item)
    {
        Container.Add(_item);
    }
}
