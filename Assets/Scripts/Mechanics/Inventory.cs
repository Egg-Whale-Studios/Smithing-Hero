using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class Inventory : MonoBehaviour
{
    [Header("Valuables")]
    public List<Tuple<ScriptableSwords,int>> sword_inventory = new List<Tuple<ScriptableSwords, int>>();
    
    public int gold;
    public int gem;

    public GameObject gold_display;
    private TMP_Text gold_display_value;
    
    public static Inventory instance;

    [Header("Smithing Panel")] 
    public int unlocked_merge_slots;
    public int unlocked_battle_slots;
    
    public GameObject smithing_panel;
    public List<GameObject> slots = new List<GameObject>();
    public Sprite empty;
    
    public Sprite[] slot_bg_sprites;
    private Image[] slot_backgrounds = new Image[50];
    
    [Header("Merge mechanics")]
    [NonSerialized] public bool moving_sword;
    [NonSerialized] public GameObject moving_sword_source;
    [NonSerialized] public int moving_sword_ind;
    [NonSerialized] public int moving_sword_slot_ind;
    [NonSerialized] private GameObject moving_sword_sprite;

    public float double_merge_chance;

    private bool is_elemental;
    private string element_name;
    private bool is_shiny;
    
    [Header("Battle Mechanics")]
    public List<ScriptableSwords> battle_swords;
    public List<GameObject> damage_texts;
    public GameObject[] active_sword_group;
    public GameObject[] active_hero_sword_group;
    private List<SwordBehaviour> active_sword_scripts = new List<SwordBehaviour>();
    public SwordBehaviour[] active_hero_sword_scripts = new SwordBehaviour[5];

    public ScriptableSwords[] hero_sword = new ScriptableSwords[5]; // Bu ikisini birlestir
    public Hero_Data[] hero = new Hero_Data[5];
    public List<Hero_Data> hero_inventory = new List<Hero_Data>(); // Hero data, evolution stage, combat level

    [Header("Upgrades")] 
    public int crafting_tier;
    [SerializeField] private ScriptableSwords[] sword_tiers;
    public int sword_rarity;
    
    [Header("Windows")]
    


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
        hero_sword = new ScriptableSwords[5];
        
        On_Sword_Change += Update_Slot_Sword;
        On_Sword_Change += Update_Slot_Background;
        On_Sword_Change += Check_Battle_Swords;
        On_Sword_Change += Update_Damage_Texts;
        
        unlocked_merge_slots = 4;
        unlocked_battle_slots = 1;
        crafting_tier = 0;

        double_merge_chance = 0;
        
        
        gold_display_value = gold_display.GetComponentInChildren<TMP_Text>();
        
            
        for (int i = 0; i < smithing_panel.transform.childCount; i++)
        {
            
            slots.Add(smithing_panel.transform.GetChild(i).gameObject);
        }
        
        
        Change_Gold(999);
        
        

        foreach (var sword in active_sword_group)
        {
            active_sword_scripts.Add(sword.GetComponent<SwordBehaviour>());
        }
        
        for (int i = 0; i < slot_backgrounds.Length; i++)
        {
            slot_backgrounds[i] = slots[i].GetComponent<Image>();
        }
        
        On_Sword_Change?.Invoke();
        
    }

    private void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0) && !moving_sword) Pick_Up_Sword();
        if (Input.GetMouseButtonUp(0) && moving_sword) Drop_Sword();
        

        if (moving_sword)
        {
            moving_sword_sprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving_sword_sprite.transform.position = new Vector3(moving_sword_sprite.transform.position.x,moving_sword_sprite.transform.position.y,2);
            moving_sword_sprite.transform.rotation = Quaternion.Euler(0,0,-45);
        }
        
    }


    #region Inventory Methods

    public void Add_Sword() // Adds a sword to the first empty slot
    {
        int extra_index = BuffWeapon(sword_tiers[crafting_tier], sword_tiers[crafting_tier]);
        
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
        
        sword_inventory.Add(Tuple.Create(sword_tiers[crafting_tier + extra_index], temp_ind));
        On_Sword_Change?.Invoke();
    }
    
    public void Add_Sword(ScriptableSwords specific_sword) // Adds a sword to the first empty slot
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
        
        sword_inventory.Add(Tuple.Create(specific_sword, temp_ind));
        On_Sword_Change?.Invoke();
    }
    
    public void Change_Gold(int gold_amount)
    {
        gold += gold_amount;
        gold_display_value.text = gold.ToString();
    }
    
    
    public void Add_Gem(int gem_amount)
    {
        gem += gem_amount;
    }


    public void Add_Hero(ScriptableHeroes new_hero)
    {
        bool is_new = true;
        int old_ind = 0;
        
        for (int i = 0; i < hero_inventory.Count; i++)
        {
            
            if (hero_inventory[i].hero_data == new_hero)
            {
                is_new = false;
                old_ind = i;
                break;
            }
        }
        
        if (is_new)
        {
            hero_inventory.Add(new Hero_Data(new_hero,1,1));
        }
        else
        {
            hero_inventory[old_ind].evolution_level += 1;
        }
        
        
    }
    

    #endregion

    #region Smithing Window

    
    public void Update_Slot_Background() //Updates the background sprite of slots, showing if they are unlocked and indicating battle slots
    // Optimize et
    {
        
        Tuple<ScriptableSwords,int> temp_sword = null;
        
        
        for (int i = 0; i < unlocked_merge_slots; i++)
        {
            bool sword_found = false;
            foreach (var sword in sword_inventory)
            {
                if (sword.Item2 == i)
                {
                    temp_sword = sword;
                    sword_found = true;
                    break;
                }
            }
            
            if (i < unlocked_battle_slots)
            {
                
                if(sword_found && temp_sword.Item1.is_elemental)
                {
                
                    switch (temp_sword.Item1.element_type)
                    {
                        case "Fire":
                            slot_backgrounds[i].sprite = slot_bg_sprites[7];
                            break;
                        case "Earth":
                            slot_backgrounds[i].sprite = slot_bg_sprites[6];
                            break;
                        case "Charge":
                            slot_backgrounds[i].sprite = slot_bg_sprites[5];
                            break;
                    }
                
                }
                else
                {
                    slot_backgrounds[i].sprite = slot_bg_sprites[4]; 
                    
                }
            }
            
            
            else
            {
                if(sword_found && temp_sword.Item1.is_elemental)
                {
                
                    switch (temp_sword.Item1.element_type)
                    {
                        case "Fire":
                            slot_backgrounds[i].sprite = slot_bg_sprites[3];
                            break;
                        case "Earth":
                           slot_backgrounds[i].sprite = slot_bg_sprites[2];
                            break;
                        case "Charge":
                           slot_backgrounds[i].sprite = slot_bg_sprites[1];
                            break;
                    }
                
                }
                else
                {
                   slot_backgrounds[i].sprite = slot_bg_sprites[0];
                }
            }
            
            
            
        }
        
        for (int i = unlocked_merge_slots; i < slots.Count; i++)
        {
            slots[i].transform.GetComponent<Image>().sprite = empty;
            // Buraya lockedlar gelcek
        }
        
        
        
    }

    public void Update_Slot_Sword() // Updates the sprite of swords in slots
    {
        
        foreach (var slot in slots)
        {
            slot.transform.GetChild(0).GetComponent<Image>().sprite = empty;
        }
        
        for (int i = 0;i<sword_inventory.Count;i++)
        {
            slots[sword_inventory[i].Item2].transform.GetChild(0).GetComponent<Image>().sprite = sword_inventory[i].Item1.sword_sprite;
            slots[sword_inventory[i].Item2].transform.GetChild(0).transform.rotation = Quaternion.Euler(0,0,-45);
        }
        
        
        sword_inventory.Sort((x, y) => x.Item2.CompareTo(y.Item2)); // Rearranging the sword inventory after any change
    }

    

    public void Merge_Swords(int source_ind, int target_ind, int tier) // Merges two swords in the inventory
    {
        
        int create_ind = sword_inventory[target_ind].Item2;
        int extra_index = BuffWeapon(sword_inventory[source_ind].Item1, sword_inventory[target_ind].Item1);

        if (double_merge_chance > UnityEngine.Random.Range(0f, 100f)) extra_index += 8;
        
        if(source_ind > target_ind)
        {
            sword_inventory.RemoveAt(source_ind);
            sword_inventory.RemoveAt(target_ind);
            sword_inventory.Add(Tuple.Create(sword_tiers[tier + 8 + extra_index], create_ind));
            
        }
        else if(source_ind < target_ind)
        {
            sword_inventory.RemoveAt(target_ind);
            sword_inventory.RemoveAt(source_ind);
            sword_inventory.Add(Tuple.Create(sword_tiers[tier + 8 + extra_index], create_ind));
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
            sword_inventory.RemoveAt(first_ind);
            sword_inventory.RemoveAt(second_ind);
        }
        else
        {
            sword_inventory.RemoveAt(second_ind);
            sword_inventory.RemoveAt(first_ind); 
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
                        
                        moving_sword_sprite = Instantiate(moving_sword_source, moving_sword_source.transform.position, Quaternion.identity,slot.transform.parent.transform);

                        moving_sword_source.GetComponent<Image>().sprite = empty;
                        
                        moving_sword_sprite.transform.SetAsLastSibling();
                        
                        
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
        
        
    }
    
    public void Unlock_Slot()
    {
        unlocked_merge_slots += 1;
        Update_Slot_Background();
    }

    public void Unlock_Battle_Slot()
    {
        unlocked_battle_slots += 1;
        Update_Slot_Background();
        On_Sword_Change?.Invoke();
    }
    

    #endregion

    #region Battle Window

    public void Check_Battle_Swords()
    {
        
        battle_swords.Clear();
        
        for (int i = 0; i < unlocked_battle_slots; i++)
        {
            foreach (var sword in sword_inventory)
            {
                if (sword.Item2 == i)
                {
                    battle_swords.Add(sword.Item1);
                }
            }
            
        }
        
        Change_Active_Swords();
    }

    private void Change_Active_Swords()
    {
        
        foreach (var old_sword in active_sword_group)
        {
            old_sword.SetActive(false);
        }

        for (int i = 0; i < hero_sword.Length; i++)
        {
            active_hero_sword_scripts[i].is_heroic = false;
            active_hero_sword_group[i].SetActive(false);
        }
        
        for (int i = 0; i < battle_swords.Count; i++)
        {
            ScriptableSwords base_sword = battle_swords[i];
            GameObject temp_sword = active_sword_group[i];
            active_sword_scripts[i].Set_Script(base_sword);
            temp_sword.SetActive(true);
            
            active_sword_scripts[i].damage_text = damage_texts[i];
        }
        
        for (int i = 0; i < hero_sword.Length; i++)
        {
            if (hero_sword[i] != null)
            {
                ScriptableSwords base_sword = hero_sword[i];
                active_hero_sword_scripts[i].Set_Script(base_sword);
                active_hero_sword_scripts[i].is_heroic = true; // Bu aşağı tarafa da eklenebilir. 
                active_hero_sword_scripts[i].data_from_hero = hero[i];
                active_hero_sword_group[i].SetActive(true);
                active_hero_sword_scripts[i].damage_text = damage_texts[i + 8];
                StartCoroutine(active_hero_sword_scripts[i].AttackAlways());
                
            }

        }
        
    }

    private void Update_Damage_Texts()
    {
        for (int i = 0; i < unlocked_battle_slots; i++)
        {
            foreach (var sword in sword_inventory)
            {
                if (sword.Item2 == i)
                {
                    damage_texts[i].SetActive(true);
                    damage_texts[i].GetComponent<FloatingNumber>().Change_Text(Color.white, sword.Item1.sword_damage.ToString(), 1);
                }
            }
            
        }
    }
    
    int BuffWeapon(ScriptableSwords mother_sword, ScriptableSwords father_sword)
    {
        int extra_index = 0;
        
        float shiny_percentage = 10f;
        

        // Increase percentages by mother and father swords
        if (mother_sword.is_shiny)
        {
            shiny_percentage += 10f;
        }
        if (father_sword.is_shiny)
        {
            shiny_percentage += 10f;
        }

        if (shiny_percentage == 30)
        {
            shiny_percentage = 50;
        }

        float elemental_percentage = 10f;
        if (mother_sword.is_elemental)
        {
            elemental_percentage += 10;
        }
        if (father_sword.is_elemental)
        {
            elemental_percentage += 10;
        }

        float s_rand_percentage=UnityEngine.Random.Range(0f, 100f);
        if (s_rand_percentage < shiny_percentage)
        {
            extra_index += 3;
        }
        float e_rand_percentage=UnityEngine.Random.Range(0f, 100f);
        if (e_rand_percentage < elemental_percentage)
        {
            
            int element= UnityEngine.Random.Range(1, 4);
            switch (element)
            {
                case 1: // Charge
                    extra_index += 1;
                    break;
                case 2: // Earth
                    extra_index += 2;
                    break;
                case 3: // Fire
                    extra_index += 3;
                    break;
                

            }
        }
        return extra_index;
    }


    public void Add_Sword_To_Hero(int hero_ind, int sword_ind)
    {
        hero_sword[hero_ind] = sword_inventory[sword_ind].Item1;
        sword_inventory.RemoveAt(sword_ind);
    }
    
    public void remove_Sword_From_Hero(int hero_ind)
    {
        if(sword_inventory.Count < unlocked_merge_slots)
        {
            Add_Sword(hero_sword[hero_ind]);
            hero_sword[hero_ind] = null;
        }
        else
        {
            Debug.Log("No space in inventory");
        }
    }
    

    #endregion
}