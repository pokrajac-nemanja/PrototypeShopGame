using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat Tradable Item", menuName = "Inventory System/TradableItems/Garments/Hat")]
public class Hat : Garment
{
    public override void Awake()
    {
        base.Awake();
        type = GarmentType.Hat;
    }
}