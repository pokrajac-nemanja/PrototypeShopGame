using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Tradable Item", menuName = "Inventory System/TradableItems/Default")]
public class DefaultTradableItem : TradableItem
{
    public void Awake()
    {
        group = TradableItemGroup.Default;
    }
}
