using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{

    [Header("Crafting")]
    public Inventory inventory;
    
    
    public GameObject battle_window;
    public GameObject smithing_window;
    private Vector3 smithing_window_start_pos;
    public bool is_battle_window = true;
    
    [Header("Shopping")]
    public GameObject shop_window;
    public int merge_slot_cost;
    public int battle_slot_cost;
    private Vector3 shop_window_start_pos;
    
    

    private void Start()
    {
        smithing_window_start_pos = smithing_window.transform.position;
        shop_window_start_pos = shop_window.transform.position;
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
        int crafting_cost = 10;
        
        if (inventory.unlocked_merge_slots > inventory.sword_inventory.Count)
        {
            if(inventory.gold >= crafting_cost)
            {
                inventory.Change_Gold(-crafting_cost);
                inventory.Add_Sword();
            }
            else
            {
                Debug.Log("Not enough gold");
            }
        }
        else
        {
            Debug.Log("Not enough space");
        }
        
    }

    public void Change_Window()
    {
        if (is_battle_window)
        {
            
            is_battle_window = false;
            Camera.main.transform.position = smithing_window.transform.position;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        }
        else
        {
            
            is_battle_window = true;
            Camera.main.transform.position = Vector3.zero;
        }
        
    }

    
    public void Upgrade_Menu()
    {
        
        if(shop_window.transform.position == shop_window_start_pos)
        {
            shop_window.transform.position = new Vector3(shop_window.transform.position.x,-5,shop_window.transform.position.z);
        }
        else
        {
            shop_window.transform.position = shop_window_start_pos;
        }
    }
    
    public void Unlock_Merge_Slot()
    {
        if (inventory.gold >= merge_slot_cost)
        {
            inventory.Change_Gold(-merge_slot_cost);
            inventory.Unlock_Slot();
        }
        else
        {
            Debug.Log("Not enough gold");
        }
    }
    
    public void Unlock_Battle_Slot()
    {
        if (inventory.gold >= battle_slot_cost)
        {
            inventory.Change_Gold(battle_slot_cost);
            inventory.Unlock_Battle_Slot();
        }
        else
        {
            Debug.Log("Not enough gold");
        }
    }
}
