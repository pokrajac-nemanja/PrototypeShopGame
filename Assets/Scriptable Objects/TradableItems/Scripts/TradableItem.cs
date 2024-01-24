using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TradableItemGroup
{
    Garment,
    Default
}
public abstract class TradableItem : ScriptableObject
{
    public GameObject prefab;
    public TradableItemGroup group;
    public int basePrice;
    [TextArea(5, 10)]
    public string description;

    public Sprite getDisplayIcon()
    {
        return prefab.GetComponent<SpriteRenderer>().sprite;
    }

    public Color getDisplayColor()
    {
        return prefab.GetComponent<SpriteRenderer>().color;
    }
}
