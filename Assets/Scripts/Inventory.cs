using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Valuables")]
    public List<Tuple<ScriptableSwords,int>> sword_inventory = new List<Tuple<ScriptableSwords, int>>();
    
    public int gold;
    public int gem;
    
    public static Inventory instance;

    [Header("Smithing Panel")] 
    public int unlocked_merge_slots;
    public int unlocked_battle_slots;
    
    public GameObject smithing_panel;
    public List<GameObject> slots = new List<GameObject>();
    public Sprite empty;
    public Sprite merge_slot_sprite;
    public Sprite battle_slot_sprite;
    public Sprite locked_slot_sprite;
    
    [Header("Merge mechanics")]
    [NonSerialized] public bool moving_sword;
    [NonSerialized] public GameObject moving_sword_source;
    [NonSerialized] public int moving_sword_ind;
    [NonSerialized] public int moving_sword_slot_ind;
    [NonSerialized] private GameObject moving_sword_sprite;

    [Header("Upgrades")] 
    public ScriptableSwords crafting_tier;
    [SerializeField] private ScriptableSwords[] sword_tiers;


    public static Action On_Sword_Change; // Added as action to be able keep track of merging info to display on achievements

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        
    }

    private void Start()
    {
        On_Sword_Change += Update_Slot_Sword;
        unlocked_merge_slots = 8;
        unlocked_battle_slots = 1;
        crafting_tier = sword_tiers[0];
        
            
        for (int i = 0; i < smithing_panel.transform.childCount; i++)
        {
            
            slots.Add(smithing_panel.transform.GetChild(i).gameObject);
        }
        
        On_Sword_Change?.Invoke();
        Update_Slot_Background();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !moving_sword) Pick_Up_Sword();
        if (Input.GetMouseButtonUp(0) && moving_sword) Drop_Sword();
        

        if (moving_sword)
        {
            
            moving_sword_sprite.transform.position = Input.mousePosition;
            
        }
        
    }


    #region Inventory Methods

    public void Add_Sword() // Adds a sword to the first empty slot
    {
        int temp_ind = 0;
        for (int i = 0; i < unlocked_merge_slots; i++)
        {
            bool slot_occupied = false;
            foreach (var tup in sword_inventory)
            {
                if (tup.Item2 == i)
                {
                    slot_occupied = true;
                    break;
                }
            }
            if (!slot_occupied)
            {
                temp_ind = i;
                break;
            }
        }
        
        sword_inventory.Add(Tuple.Create(crafting_tier, temp_ind));
        On_Sword_Change?.Invoke();
    }
    
    public void Add_Gold(int gold_amount)
    {
        gold += gold_amount;
    }
    
    public void Add_Gem(int gem_amount)
    {
        gem += gem_amount;
    }
    
    

    #endregion

    #region Smithing Window

    
    public void Update_Slot_Background() //Updates the background sprite of slots, showing if they are unlocked and indicating battle slots
    {
        for (int i = 0; i < unlocked_merge_slots; i++)
        {
            
            if (i < unlocked_battle_slots)
            {
                slots[i].GetComponent<Image>().sprite = battle_slot_sprite;
            }
            else
            {
                slots[i].GetComponent<Image>().sprite = merge_slot_sprite;
            }
            
            
            slots[i].transform.GetComponent<Image>().sprite = sword_inventory[i].Item1.sword_sprite;
            
        }
        
        for (int i = unlocked_merge_slots; i < slots.Count; i++)
        {
            slots[i].transform.GetComponent<Image>().sprite = empty;
        }
    }

    public void Update_Slot_Sword() // Updates the sprite of swords in slots
    {
        string temp = "";
        foreach (var sword in sword_inventory)
        {
            temp += sword.Item1.sword_name + " : " + sword.Item2 + "\n";
        }
        //Debug.Log(temp);
        
        foreach (var slot in slots)
        {
            slot.transform.GetChild(0).GetComponent<Image>().sprite = empty;
        }

        foreach (var sword in sword_inventory)
        {
            slots[sword.Item2].transform.GetChild(0).GetComponent<Image>().sprite = sword.Item1.sword_sprite;
        }
        
        
        sword_inventory.Sort((x, y) => x.Item2.CompareTo(y.Item2)); // Rearranging the sword inventory after any change
    }

    

    public void Merge_Swords(int source_ind, int target_ind, int tier) // Merges two swords in the inventory
    {
        Debug.LogWarning("Merging");
        Debug.Log("Source ind = " + source_ind + ", target ind = " + target_ind);
        int create_ind = sword_inventory[target_ind].Item2;
        if(source_ind > target_ind)
        {
            sword_inventory.RemoveAt(source_ind);
            sword_inventory.RemoveAt(target_ind);
            sword_inventory.Add(Tuple.Create(sword_tiers[tier + 1], create_ind));
            
        }
        else if(source_ind < target_ind)
        {
            sword_inventory.RemoveAt(target_ind);
            sword_inventory.RemoveAt(source_ind);
            sword_inventory.Add(Tuple.Create(sword_tiers[tier + 1], create_ind));
        }
        
        On_Sword_Change?.Invoke();
    }

    public void Change_Swords(int first_ind, int second_ind)
    {
        int temp_ind1 = sword_inventory[first_ind].Item2;
        int temp_ind2 = sword_inventory[second_ind].Item2;
        ScriptableSwords temp_sword1 = sword_inventory[first_ind].Item1;
        ScriptableSwords temp_sword2 = sword_inventory[second_ind].Item1;
        
        if(temp_ind1 > temp_ind2)
        {
            sword_inventory.RemoveAt(temp_ind1);
            sword_inventory.RemoveAt(temp_ind2);
        }
        else
        {
            sword_inventory.RemoveAt(temp_ind2);
            sword_inventory.RemoveAt(temp_ind1); 
        }
        
        
        sword_inventory.Add(Tuple.Create(temp_sword1, temp_ind2));
        sword_inventory.Add(Tuple.Create(temp_sword2, temp_ind1));
        
        On_Sword_Change?.Invoke();
    }

    

    


    private void Pick_Up_Sword()
    {
        PointerEventData pointer_event = new PointerEventData(EventSystem.current);
        pointer_event.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer_event, results);

        foreach (var obj in results)
        {
            if (obj.gameObject.CompareTag("Sword_Slot")) // Found a slot, doesn't know if it's unlocked or it has a sword
            {

                GameObject slot = obj.gameObject;
                int index = slot.transform.GetSiblingIndex();
                
                foreach (var sword in sword_inventory)
                {
                    if (sword.Item2 == index) // Found a sword in that slot
                    {
                        
                        moving_sword_slot_ind = index;

                        foreach (var tup in sword_inventory)
                        {
                            if (tup.Item2 == index)
                            {
                                moving_sword_ind = sword_inventory.IndexOf(tup);
                            }
                        }
                        
                        moving_sword_source = slot.transform.GetChild(0).gameObject;
                        
                        moving_sword_sprite = Instantiate(moving_sword_source, moving_sword_source.transform.position, Quaternion.identity,slot.transform.parent);

                        moving_sword_source.GetComponent<Image>().sprite = empty;
                        
                        moving_sword_sprite.transform.SetAsLastSibling();
                        
                        Debug.Log("source ind is : " + moving_sword_ind);
                        moving_sword = true;
                        return;
                    }
                }
            }
        }
        
        
    }

    private void Drop_Sword()
    {
        
        PointerEventData pointer_event = new PointerEventData(EventSystem.current);
        pointer_event.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer_event, results);
        
        foreach (var obj in results)
        {
            if (obj.gameObject.CompareTag("Sword_Slot")) // Found a slot, doesn't know if it's unlocked or it has a sword
            {
                
                GameObject slot = obj.gameObject;
                int slot_index = slot.transform.GetSiblingIndex();
                int sword_index = -1;
                
                bool slot_occupied = false;

                foreach (var sword in sword_inventory)
                {
                    if (sword.Item2 == slot_index)
                    {
                        
                        sword_index = sword_inventory.IndexOf(sword);
                        slot_occupied = true;
                        break;
                    }
                }
                
                 

                if (slot_occupied) // Found a sword in the slot
                {
                    
                    if (sword_inventory[sword_index].Item1.tier == sword_inventory[moving_sword_ind].Item1.tier) //Found a sword of same tier, merge
                    {
                        Destroy(moving_sword_sprite);
                        Merge_Swords(moving_sword_ind,sword_index,sword_inventory[moving_sword_ind].Item1.tier);
                    }
                    else // Found a sword of another tier, change
                    {
                        Destroy(moving_sword_sprite);
                        Change_Swords(sword_index,moving_sword_ind);
                    }
                }

                else // Found an empty slot
                {
                    
                    if (slot_index < unlocked_merge_slots) // The slot is unlocked
                    {
                        ScriptableSwords temp_sword = sword_inventory[moving_sword_ind].Item1;
                        int temp_ind = slot_index;
                        
                        sword_inventory.Add(Tuple.Create(temp_sword,temp_ind));
                        Destroy(moving_sword_sprite); // Hareket ettirerek optimize edilebilir
                        sword_inventory.RemoveAt(moving_sword_ind);
                        On_Sword_Change?.Invoke();
                    }
                    else // The slot is not unlocked
                    {
                        Destroy(moving_sword_sprite);
                        On_Sword_Change?.Invoke();
                    }
                }
                
            }
            else  // Dropping a sword to somewhere irrelevent
            {
                Destroy(moving_sword_sprite);
                On_Sword_Change?.Invoke();
            }
        }
        if(results.Count == 0) // Dropping a sword to somewhere irrelevent
        {
            Destroy(moving_sword_sprite);
            On_Sword_Change?.Invoke();
        }
        
        moving_sword = false;

        /*
        foreach (var sword in sword_inventory)
        {
            Debug.Log("The index of " +sword.Item1.sword_name +" is " +sword.Item2);
        }*/
        
    }
    
    public void Unlock_Slot()
    {
        
    }

    public void Unlock_Battle_Slot()
    {
        
    }
    

    #endregion
    
}
