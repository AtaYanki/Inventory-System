using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Objects", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObjects
{
    public float atkBonus;
    public float defenceBonus;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
