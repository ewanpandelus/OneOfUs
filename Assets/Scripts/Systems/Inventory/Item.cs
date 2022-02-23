using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ItemObject")]
public class Item : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private ItemType itemType;
    [SerializeField] private SpriteRenderer sprite;

   
}
public enum ItemType
{
    Weapon, 
    Food, 
    Resource,
}
