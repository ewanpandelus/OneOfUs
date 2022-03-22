using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddItem(string _item)
    {
        if (inventory.ContainsKey(_item))
        {
            inventory[_item]++;
        }
        else
        {
            inventory.Add(_item, 1);
        }
    }
}
