using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Objects", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObjects
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Food;
    }
}
