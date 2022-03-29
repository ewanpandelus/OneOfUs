using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    public delegate void CollectedWheatDelegate();
    public event CollectedWheatDelegate collectedWheatEvent;


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
        CheckInventoryForKeyItems();
    }
    public bool CheckInventoryContainsAmountOfItem(string _item, float _threshold)
    {
        return inventory[_item] >= _threshold;
    }
    private void CheckInventoryForKeyItems()
    {
        if(CheckInventoryContainsAmountOfItem("Wheat", 10))
        {
            collectedWheatEvent?.Invoke();
        }
    }
}
