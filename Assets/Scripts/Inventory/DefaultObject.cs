using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Objects", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObjects
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
