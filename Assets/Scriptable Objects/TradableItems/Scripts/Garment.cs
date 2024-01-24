using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GarmentType
{
    Vest,
    Hat
}
public abstract class Garment : TradableItem
{
    public GarmentType type;

    public virtual void Awake()
    {
        group = TradableItemGroup.Garment;
    }
}
