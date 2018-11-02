using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    public int itemId = 0;
    public ItemTypes itemType;
    public int amount;

	public void OnCollection()
    {
        if(itemType == ItemTypes.Money)
        {
            Inventory.money += amount;
        }
        else if (itemType == ItemTypes.Craftable || itemType == ItemTypes.Consumables)
        {
            //changes to 1 if item is found in inv
            int found = 0;
            //index where item is found
            int addIndex = 0;
            for(int i = 0; i < Inventory.inv.Count; i++)
            {
                if(itemId == Inventory.inv[i].Id)
                {
                    found = 1;
                    addIndex = i;
                    break;
                }
            }

            if (found == 1)
            {
                Inventory.inv[addIndex].Amount += amount;
            }
            else
            {
                Inventory.inv.Add(ItemData.CreateItem(itemId));
                if(amount >= 1)
                {
                    for(int i = 0; i < Inventory.inv.Count; i++)
                    {
                        if(itemId == Inventory.inv[i].Id)
                        {
                            Inventory.inv[i].Amount = amount;
                        }
                    }
                }
            }
        }
        else //weapons, armour, misc etc
        {
            //pick up up item
            Inventory.inv.Add(ItemData.CreateItem(itemId));
        }
        Destroy(gameObject);//remove item from world
    }
}
