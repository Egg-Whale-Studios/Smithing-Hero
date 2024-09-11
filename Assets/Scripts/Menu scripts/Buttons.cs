using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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
    public GameObject hero_choice_window;
    
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
    private int hero_to_replace_ind;
    
    private List<Image> sword_choice_images = new List<Image>();
    private List<Image> sword_choice_bg_images = new List<Image>();
    private List<GameObject> sword_choice_slots = new List<GameObject>();
    private bool is_sword_choice_open;
    public Image[] hero_sword_displays;
    
    private List<Image> hero_choice_images = new List<Image>();
    private List<Image> hero_choice_bg_images = new List<Image>(); // efsaneviler fln için
    private List<GameObject> hero_choice_slots = new List<GameObject>();
    private bool is_hero_choice_open;
    public Image[] hero_displays;
    
    public GameObject hero_info_window;
    public Image hero_info_image;
    public TMP_Text hero_info_stats;
    public TMP_Text hero_info_name;
    public TMP_Text hero_info_rarity;

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
            
            hero_choice_images.Add(hero_choice_window.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>());
            hero_choice_bg_images.Add(hero_choice_window.transform.GetChild(i).GetComponent<Image>());
            hero_choice_slots.Add(hero_choice_window.transform.GetChild(i).gameObject);
            
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

    public void Change_Window(string direction)
    {
        if (direction == "Left")
        {
            Camera.main.transform.position -= new Vector3(4.75f, 0, 0);
        }
        else
        {
            Camera.main.transform.position += new Vector3(4.75f, 0, 0);
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
                
                Update_Sword_Choice("Already Equipped");
            }
            else
            {
                Update_Sword_Choice("Not Equipped");
            }
            
            sword_choice_window.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        
        is_sword_choice_open = !is_sword_choice_open;
        
        hero_ind = temp_hero_ind;
        
    }

    public void Update_Sword_Choice(string window_type)
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
    
    public void Open_Close_Hero_Choice_Window(int temp_hero_ind) 
    {
        
        hero_ind = temp_hero_ind; // Burada bi bug çıkabilir
        if(is_hero_choice_open) hero_choice_window.transform.parent.transform.parent.gameObject.SetActive(false);

        else
        {
            if(inventory.hero[temp_hero_ind] != null)
            {
                
                Update_Hero_Choice("Already Equipped");
            }
            else
            {
                Update_Hero_Choice("Not Equipped");
            }
            
            hero_choice_window.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        
        is_hero_choice_open = !is_hero_choice_open;
        
        
        
    }
    
    public void Update_Hero_Choice(string window_type)
    {
        for (int a = 0; a < hero_choice_slots.Count; a++)
        {
            hero_choice_slots[a].gameObject.SetActive(false);
        }
        
        
        if (window_type == "Already Equipped")
        {
            hero_choice_slots[0].gameObject.SetActive(true);
            hero_choice_images[0].sprite = cross;
            
            
            for (int i = 0; i < inventory.hero_inventory.Count; i++)
            {
                if (inventory.hero_inventory[i] == inventory.hero[hero_ind]) continue;
                hero_choice_slots[i + 1].gameObject.SetActive(true);
                hero_choice_images[i + 1].sprite = inventory.hero_inventory[i].hero_data.hero_sprite;
            }
        }

        else
        {
            for (int i = 0; i < inventory.hero_inventory.Count; i++)
            {
                
                hero_choice_slots[i].gameObject.SetActive(true);
                hero_choice_images[i].sprite = inventory.hero_inventory[i].hero_data.hero_sprite;
                
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


        Inventory.On_Sword_Change();
        Open_Close_Sword_Window(hero_ind);
        
    }
    
    
    public void Choose_Hero() // Optimize edilebilir
    {
        
        
        
        if (inventory.hero[hero_ind] == null)
        {
            inventory.hero[hero_ind] = inventory.hero_inventory[hero_to_replace_ind];
            inventory.hero_inventory.RemoveAt(hero_to_replace_ind); // burada çıkarttık listeden hero'yu ileride buradan patlayabiliriz
        }
        else
        {
        
            inventory.hero_inventory.Add(inventory.hero[hero_ind]); // burada ekledik listeye hero'yu ileride buradan patlayabiliriz
            inventory.hero[hero_ind] = inventory.hero_inventory[hero_to_replace_ind -1];
            inventory.hero_inventory.RemoveAt(hero_to_replace_ind-1); // burada çıkarttık listeden hero'yu ileride buradan patlayabiliriz
        
        }

        for (int i = 0; i < hero_displays.Length; i++)
        {
            if (inventory.hero[i] != null)
            {
                hero_displays[i].sprite = inventory.hero[i].hero_data.hero_sprite;
            }
            else
            {
                hero_displays[i].sprite = empty;
            }
        }
        
        
        hero_info_window.SetActive(false);
        Open_Close_Hero_Choice_Window(hero_ind);
        
    }

    public void Open_Hero_Info_Window()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        hero_to_replace_ind = button.transform.GetSiblingIndex();
        Hero_Data hero_data;
        
        if (inventory.hero[hero_ind] == null)
        {
            hero_data = inventory.hero_inventory[hero_to_replace_ind];
        }
        else
        {
            if (hero_to_replace_ind != 0)
            {
                
                hero_data = inventory.hero_inventory[hero_to_replace_ind - 1];
            }
            else
            {
                inventory.hero_inventory.Add(inventory.hero[hero_ind]);
                inventory.hero[hero_ind] = null;
                
                hero_choice_window.transform.parent.parent.gameObject.SetActive(false);
                hero_displays[hero_ind].sprite = empty;
                return;
            }
        }
        
        
        
        hero_info_window.SetActive(true);
        hero_info_image.sprite = hero_data.hero_data.hero_sprite;
        hero_info_name.text = hero_data.hero_data.name;
        
        switch (hero_data.hero_data.rarity)
        {
            case 1:
                hero_info_rarity.text = "Common Hero";
                
                break;
            case 2:
                hero_info_rarity.text = "Epic Hero";
                hero_info_rarity.color = Color.magenta;
                break;
            case 3:
                hero_info_rarity.text = "Legendary Hero";
                hero_info_rarity.color = Color.yellow;
                break;
        }
    }

    public void Hero_Level_Up(int temp_ind)
    {

        if (inventory.hero[temp_ind] != null)
        {
            inventory.hero[temp_ind].combat_level++;
        }
    }
    
    #endregion
    
    

    #region Merging Window

    public void Upgrade_Menu()
    {
        
        is_shop_window_open = !is_shop_window_open;
        if (is_shop_window_open) shop_window_rect.DOAnchorPosY(-1140, 0.5f).SetEase(Ease.OutBack);
        else shop_window_rect.DOAnchorPosY(-2540, 0.5f).SetEase(Ease.InBack);
    }
    
    

    #endregion
    
    
}