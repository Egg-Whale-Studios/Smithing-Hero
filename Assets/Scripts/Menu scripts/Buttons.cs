using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

    [Header("Crafting")]
    public Inventory inventory;

    

    private void Start()
    {
        
        inventory = Inventory.instance;
    }

    public void Adventurer_Button()
    {
        
    }
    
    public void Smith_Button()
    {
        
    }
    
    public void Battle_Button()
    {
        
    }
    
    public void Gem_Shop_Button()
    {
        
    }
    
    public void Craft_Button()
    {
        int crafting_cost = 0;
        if(inventory.sword_inventory == null) Debug.Log("null");
        if (inventory.unlocked_merge_slots > inventory.sword_inventory.Count)
        {
            inventory.gold -= crafting_cost;
            inventory.Add_Sword();
        }
        else
        {
            Debug.Log("Not enough space");
        }
        
    }
    
    

    
    void Update()
    {
        
    }
}
