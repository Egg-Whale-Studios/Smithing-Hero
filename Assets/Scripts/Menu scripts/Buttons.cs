using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    public Sprite cross;
    public Sprite empty;
    
    [Header("Windows")]
    public GameObject battle_window;
    public GameObject smithing_window;
    public GameObject shop_window;
    public GameObject hero_window;
    public GameObject sword_choice_window;
    
    private Vector3 smithing_window_start_pos;
    public bool is_battle_window = true;
    
    
    [Header("Crafting")]
    public Inventory inventory;
    
    [Header("Merge Upgrade Window")]
    public int merge_slot_cost;
    public int battle_slot_cost;
    private RectTransform shop_window_rect;
    private bool is_shop_window_open;
    
    [Header("Hero Window")]
    private RectTransform hero_window_rect;
    private bool is_hero_window_open;
    private int hero_ind;
    
    private List<Image> sword_choice_images = new List<Image>();
    private List<Image> sword_choice_bg_images = new List<Image>();
    private List<GameObject> sword_choice_slots = new List<GameObject>();
    private bool is_sword_choice_open;
    public Image[] hero_sword_displays;

    [Header("Battle Window")] 
    public GameObject[] battle_swords;
    public SwordBehaviour[] battle_sword_scripts;

    

    private void Start()
    {
        smithing_window_start_pos = smithing_window.transform.position;
        inventory = Inventory.instance;
        shop_window_rect = shop_window.GetComponent<RectTransform>();
        hero_window_rect = hero_window.GetComponent<RectTransform>();
        
        for(int i = 0; i < sword_choice_window.transform.childCount; i++)
        {
            
            sword_choice_images.Add(sword_choice_window.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>());
            sword_choice_bg_images.Add(sword_choice_window.transform.GetChild(i).GetComponent<Image>());
            sword_choice_slots.Add(sword_choice_window.transform.GetChild(i).gameObject);
        }
        
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

    #region Battle Window
    
    public void Battle_Tap()
    {
        for (int i = 0; i < battle_swords.Length; i++)
        {
            if (battle_swords[i].activeSelf)
            {
                battle_sword_scripts[i].moveNextPos();
                
            }
        }
    }
    
    public void Open_Hero_Window()
    {
        is_hero_window_open = !is_hero_window_open;
        if(is_hero_window_open) hero_window_rect.DOAnchorPosY(-1500, 0.5f).SetEase(Ease.OutBack);
        else hero_window_rect.DOAnchorPosY(-2540, 0.5f).SetEase(Ease.InBack);
            
    }
    
    public void Open_Close_Sword_Window(int temp_hero_ind) 
    {
        
        
        if(is_sword_choice_open) sword_choice_window.transform.parent.transform.parent.gameObject.SetActive(false);

        else
        {
            if(inventory.hero_sword[temp_hero_ind] != null)
            {
                
                update_sword_choice("Already Equipped");
            }
            else
            {
                update_sword_choice("Not Equipped");
            }
            
            sword_choice_window.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        
        is_sword_choice_open = !is_sword_choice_open;
        
        hero_ind = temp_hero_ind;
        
    }

    public void update_sword_choice(string window_type)
    {
        for (int a = 0; a < sword_choice_slots.Count; a++)
        {
            sword_choice_slots[a].gameObject.SetActive(false);
        }
        
        
        if (window_type == "Already Equipped")
        {
            sword_choice_slots[0].gameObject.SetActive(true);
            sword_choice_bg_images[0].sprite = inventory.slot_bg_sprites[0];
            sword_choice_images[0].sprite = cross;
            
            
            for (int i = 0; i < inventory.sword_inventory.Count; i++)
            {
                sword_choice_slots[i + 1].gameObject.SetActive(true);
                sword_choice_images[i + 1].sprite = inventory.sword_inventory[i].Item1.sword_sprite;

                switch (inventory.sword_inventory[i].Item1.element_type)
                {
                    case "Fire":
                        sword_choice_bg_images[i + 1].sprite = inventory.slot_bg_sprites[3];
                        break;
                    case "Earth":
                        sword_choice_bg_images[i + 1].sprite = inventory.slot_bg_sprites[2];
                        break;
                    case "Charge":
                        sword_choice_bg_images[i + 1].sprite = inventory.slot_bg_sprites[3];
                        break;
                    default:
                        sword_choice_bg_images[i + 1].sprite = inventory.slot_bg_sprites[0];
                        break;
                }
            }
        }

        else
        {
            for (int i = 0; i < inventory.sword_inventory.Count; i++)
            {
                
                sword_choice_slots[i].gameObject.SetActive(true);
                sword_choice_images[i].sprite = inventory.sword_inventory[i].Item1.sword_sprite;

                switch (inventory.sword_inventory[i].Item1.element_type)
                {
                    case "Fire":
                        sword_choice_bg_images[i].sprite = inventory.slot_bg_sprites[3];
                        break;
                    case "Earth":
                        sword_choice_bg_images[i].sprite = inventory.slot_bg_sprites[2];
                        break;
                    case "Charge":
                        sword_choice_bg_images[i].sprite = inventory.slot_bg_sprites[3];
                        break;
                    default:
                        sword_choice_bg_images[i].sprite = inventory.slot_bg_sprites[0];
                        break;
                }
            }
        }
        
        
    }
    
    public void Choose_Sword()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int sword_ind = button.transform.GetSiblingIndex();
        
        
        if (inventory.hero_sword[hero_ind] == null)
        {
            inventory.Add_Sword_To_Hero(hero_ind, sword_ind); // update sword logo
            
        }
        else
        {
            if (sword_ind == 0)
            {
                
                inventory.remove_Sword_From_Hero(hero_ind);
            }
            else
            {
                ScriptableSwords sword_to_add = inventory.hero_sword[hero_ind];
                ScriptableSwords sword_to_remove = inventory.sword_inventory[sword_ind - 1].Item1;
                
                inventory.hero_sword[hero_ind] = sword_to_remove;
                
                inventory.Add_Sword(sword_to_add);
                
                inventory.sword_inventory.RemoveAt(sword_ind);
                
                
            }
        }

        for (int i = 0; i < hero_sword_displays.Length; i++)
        {
            if (inventory.hero_sword[i] != null)
            {
                hero_sword_displays[i].sprite = inventory.hero_sword[i].sword_sprite;
            }
            else
            {
                hero_sword_displays[i].sprite = empty;
            }
        }
        
        
        
        Open_Close_Sword_Window(hero_ind);
        
    }
    

    #endregion
    
    

    #region Merging Window

    public void Upgrade_Menu()
    {
        
        is_shop_window_open = !is_shop_window_open;
        if (is_shop_window_open) shop_window_rect.DOAnchorPosY(-1140, 0.5f).SetEase(Ease.OutBack);
        else shop_window_rect.DOAnchorPosY(-2540, 0.5f).SetEase(Ease.InBack);
    }

    #region Shop Upgrades

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

    public void Elemental_Chance_Upgrade()
    {
        
    }
    public void Unlock_Automatic_Crafting()
    {
        
    }

    public void Craft_Upper_Tier()
    {
        
    }

    #endregion
    
    

    #endregion
    
    
}
