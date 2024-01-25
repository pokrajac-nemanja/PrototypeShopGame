using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventoryDisplay : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();

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
            var slot = Instantiate(inventoryDisplaySlot, Vector3.zero, Quaternion.identity, transform);
            slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
            if (inventory.Items[i])
            {
                slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Items[i].getDisplayIcon();
                slot.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Items[i].getDisplayColor();
                slot.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].basePrice.ToString() + "g";
            }

            AddEvent(slot, EventTriggerType.PointerEnter, delegate { OnEnter(slot); });
            AddEvent(slot, EventTriggerType.PointerExit, delegate { OnExit(slot); });
            AddEvent(slot, EventTriggerType.BeginDrag, delegate { OnDragStart(slot); });
            AddEvent(slot, EventTriggerType.EndDrag, delegate { OnDragEnd(slot); });
            AddEvent(slot, EventTriggerType.Drag, delegate { OnDrag(slot); });

            itemsDisplayed.Add(slot, inventory.Items[i]);
        }
    }

    public void UpdateDisplay()
    {
        int i = 0;
        Dictionary<GameObject, TradableItem> shownItems = new Dictionary<GameObject, TradableItem>(itemsDisplayed);
        foreach (KeyValuePair<GameObject, TradableItem> slot in shownItems)
        {
            if (slot.Value != inventory.Items[i])
            {
                itemsDisplayed.Remove(slot.Key);
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
                itemsDisplayed.Add(slot.Key, inventory.Items[i]);
            }
            i++;
        }
    }

    public Vector2 GetPosition(int i)
    {
        return new Vector2(X_START + (HORIZONTAL_SPACING * (i % NUMBER_OF_COLUMNS)), Y_START + (-VERTICAL_SPACING * (i / NUMBER_OF_COLUMNS)));
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObject = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObject = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rTransform = mouseObject.AddComponent<RectTransform>();
        rTransform.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        mouseObject.transform.localScale = Vector3.one;
        TradableItem item;
        if (itemsDisplayed.TryGetValue(obj, out item))
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = item.prefab.GetComponent<SpriteRenderer>().sprite;
            img.color = item.prefab.GetComponent<SpriteRenderer>().color;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = item;
    }

    public void OnDragEnd(GameObject obj)
    {

    }

    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            //TODO: figure out why position is offset by half a diagonal of the screen.
            mouseItem.obj.GetComponent<RectTransform>().localPosition = Input.mousePosition - new Vector3(550, 295, 0);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}

public class MouseItem
{
    public GameObject obj;
    public TradableItem item;
    public GameObject hoverObject;
    public TradableItem hoverItem;
}
