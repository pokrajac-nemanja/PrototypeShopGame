using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vest Tradable Item", menuName = "Inventory System/TradableItems/Garments/Vest")]
public class Vest : Garment
{
    public override void Awake()
    {
        base.Awake();
        type = GarmentType.Vest;
    }
}
