using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public GameObject player;
    public PlayerMovement movement;
    public InventoryObject inventory;
    public GameObject inventoryPrefab;
    public List<Transform> droppedObjects;
    public List<Vector3> droppedObjectsPos;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    
    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        UpdateSlots();
        TranslateDroppedObjects();
        if(movement.itemDropped <= 0)
        {
            droppedObjects.Clear();
            droppedObjectsPos.Clear();
        }
    }

    public void UpdateSlots()
    {
        foreach(KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if(_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<Text>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for(int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate {OnEnter(obj);});
            AddEvent(obj, EventTriggerType.PointerExit, delegate {OnExit(obj);});
            AddEvent(obj, EventTriggerType.BeginDrag, delegate {OnDragStart(obj);});
            AddEvent(obj, EventTriggerType.EndDrag, delegate {OnDragEnd(obj);});
            AddEvent(obj, EventTriggerType.Drag, delegate {OnDrag(obj);});

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
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

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            mouseItem.hoverItem = itemsDisplayed[obj];
    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 100);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        if(mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            movement.itemDropped = 1f;
            Vector3 RandomCircle (Vector3 Center, float radius)
            {
                float ang = Random.value * 360;
                Vector3 pos;
                pos.x = Center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = Center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
                pos.z = Center.z;
                return pos;
            }
            Vector3 center = player.transform.position;
            for (int i = 0; i < itemsDisplayed[obj].amount; i++)
            {
                Vector3 pos = RandomCircle(center, 1.2f);
                droppedObjectsPos.Add(pos);
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
                GameObject droppedObject = Instantiate(inventory.database.GetItem[itemsDisplayed[obj].ID].prefab, player.transform.position, new Quaternion(rot.x ,rot.y, 0, 0));
                droppedObjects.Add(droppedObject.transform);
            }
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0f);
    }
    void TranslateDroppedObjects()
    {
        for(int i = droppedObjects.Count - 1; i >= 0; i--)
        {
            droppedObjects[i].transform.position = Vector3.Lerp(droppedObjects[i].transform.position, droppedObjectsPos[i], 5 * Time.deltaTime);
        }
    }
}
public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
