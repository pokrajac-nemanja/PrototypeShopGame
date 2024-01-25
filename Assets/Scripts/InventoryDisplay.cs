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
    public GameObject buttonPrefab;

    public int X_START;
    public int Y_START;
    public int NUMBER_OF_COLUMNS;
    public int HORIZONTAL_SPACING;
    public int VERTICAL_SPACING;
    public Color SELECTED_SLOT_COLOR;

    private Color DEFAULT_SLOT_COLOR;
    private GameObject selectedDisplaySlot;
    private GameObject deleteButton;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        DEFAULT_SLOT_COLOR = inventoryDisplaySlot.transform.GetComponent<Image>().color;
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        // Create Delete button
        var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, transform);
        button.GetComponent<RectTransform>().localPosition = new Vector2(70, 180);
        button.GetComponent<Button>().interactable = false;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Delete";
        //AddEvent(button, EventTriggerType.PointerClick, delegate { DeleteSelected(); });
        button.GetComponent<Button>().onClick.AddListener(DeleteSelected);
        deleteButton = button;

        // Create inventory item grid
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            var slot = Instantiate(inventoryDisplaySlot, Vector3.zero, Quaternion.identity, transform);
            slot.GetComponent<RectTransform>().localPosition = GetPosition(i);
            if (inventory.Items[i].ID > -1)
            {
                slot.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.Items[i].item.getDisplayIcon();
                slot.transform.GetChild(0).GetComponentInChildren<Image>().color = inventory.Items[i].item.getDisplayColor();
                slot.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Items[i].item.basePrice.ToString() + "g";
            }
            itemsDisplayed.Add(slot, inventory.Items[i]);

            AddEvent(slot, EventTriggerType.PointerEnter, delegate { OnEnter(slot); });
            AddEvent(slot, EventTriggerType.PointerExit, delegate { OnExit(slot); });
            AddEvent(slot, EventTriggerType.BeginDrag, delegate { OnDragStart(slot); });
            AddEvent(slot, EventTriggerType.EndDrag, delegate { OnDragEnd(slot); });
            AddEvent(slot, EventTriggerType.Drag, delegate { OnDrag(slot); });
            AddEvent(slot, EventTriggerType.PointerClick, delegate { OnClick(slot); });
        }
    }

    public void UpdateDisplay()
    {
        // Update Delete button
        if (selectedDisplaySlot != null)
        {
            deleteButton.GetComponent<Button>().interactable = true;
        } else
        {
            deleteButton.GetComponent<Button>().interactable = false;
        }

        // Update inventory item grid
        Dictionary<GameObject, InventorySlot> shownItems = new Dictionary<GameObject, InventorySlot>(itemsDisplayed);
        foreach (KeyValuePair<GameObject, InventorySlot> slot in shownItems)
        {
            if (slot.Value.ID > -1)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.item.getDisplayIcon();
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = slot.Value.item.getDisplayColor();
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.item.basePrice.ToString() + "g";
            }
            else
            {
                var baseSlot = inventoryDisplaySlot.transform.GetChild(0).GetComponent<Image>();
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = baseSlot.sprite;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = baseSlot.color;
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            if (slot.Key == selectedDisplaySlot)
            {
                slot.Key.transform.GetComponent<Image>().color = SELECTED_SLOT_COLOR;
            } else
            {
                slot.Key.transform.GetComponent<Image>().color = DEFAULT_SLOT_COLOR;
            }
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
        InventorySlot slot;
        if (itemsDisplayed.TryGetValue(obj, out slot))
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = slot.item.prefab.GetComponent<SpriteRenderer>().sprite;
            img.color = slot.item.prefab.GetComponent<SpriteRenderer>().color;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = slot;
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

    public void OnClick(GameObject obj)
    {
        if (itemsDisplayed[obj].ID > -1)
        {
            SelectSlot(obj);
        }
    }

    public void DeleteSelected()
    {
        InventorySlot item = itemsDisplayed[selectedDisplaySlot];
        inventory.DeleteItem(item);
        selectedDisplaySlot = null;
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    private void SelectSlot(GameObject slot)
    {
        if (selectedDisplaySlot == slot)
        {
            selectedDisplaySlot = null;
        } else
        {
            selectedDisplaySlot = slot;
        }
    }
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public GameObject hoverObject;
    public InventorySlot hoverItem;
}
