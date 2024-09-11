using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [Header("Common Rune Upgrades")]
    private int gold_multiplier_level;
    private int attack_boost_level;
    private int boss_timer_level;
    private int rarity_upgrade_level;
    
    public TMP_Text gold_multiplier_level_text;
    public TMP_Text attack_boost_level_text;
    public TMP_Text boss_timer_level_text;
    public TMP_Text rarity_upgrade_level_text;

    
    private string last_chosen_common_rune;
    public TMP_Text upgrade_name;
    public TMP_Text upgrade_description;
    public TMP_Text upgrade_cost_text;

    private int upgrade_cost;


    [Header("Epic Rune Upgrades")]
    public GameObject[] ghost_swords;
    public SwordBehaviour[] ghost_sword_behaviours;
    
    private bool hold_to_attack;
    private bool wave_skip;
    private bool ghost_swords_active;
    
    [Header("Temporary Upgrades")]
    public int battle_slot_cost;
    public int merge_slot_cost;

    private int battle_slot_level;
    private int merge_slot_level;
    private int base_sword_level;
    private int double_merge_level;



    [Header("Necessities")] 
    public CombatManager combat_manager;
    private Inventory inventory;
    public static Upgrades instance;
    private void Awake()
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
        
        gold_multiplier_level = 0;
        attack_boost_level = 0;
        boss_timer_level = 0;
        rarity_upgrade_level = 0;
        
        hold_to_attack = false;
        wave_skip = false;
        ghost_swords_active = false;
        
        battle_slot_level = 1;
        merge_slot_level = 1;
        base_sword_level = 0;
        double_merge_level = 0;
    }

    private void Start()
    {
        inventory = Inventory.instance;
        
    }

    public void Choose_Common_Rune(string upgrade)
    {
        switch (upgrade)
        {
            case "Gold Multiplier":
                upgrade_cost = 100 * gold_multiplier_level;
                upgrade_name.text = "Gold Multiplier";
                upgrade_description.text = "The gold you earn is increased by " + gold_multiplier_level * 5f + " <color=\"green\">(+ 5)</color> %";
                upgrade_cost_text.text = "Cost: " + upgrade_cost;
                last_chosen_common_rune = "Gold Multiplier";
                break;
            case "Attack Boost":
                upgrade_cost = 100 * attack_boost_level;
                upgrade_name.text = "Attack Boost";
                upgrade_description.text = "Your attacks deal " + attack_boost_level * 2.5f + " <color=\"green\">(+ 2.5)</color> % more damage";
                upgrade_cost_text.text = "Cost: " + upgrade_cost;
                last_chosen_common_rune = "Attack Boost";
                break;
            case "Boss Timer":
                upgrade_cost = 100 * boss_timer_level;
                upgrade_name.text = "Boss Timer";
                upgrade_description.text = "Bosses wait " + boss_timer_level * 0.5f + " <color=\"green\">(+ 0.5)</color> % more seconds before disappearing";
                upgrade_cost_text.text = "Cost: " + upgrade_cost;
                last_chosen_common_rune = "Boss Timer";
                break;
            case "Rarity Upgrade":
                upgrade_cost = 100 * rarity_upgrade_level;
                upgrade_name.text = "Rarity Upgrade";
                upgrade_description.text = "Your chances of getting a rare sword is increased by " + rarity_upgrade_level * 2.5f + " <color=\"green\">(+ 2.5)</color> %";
                upgrade_cost_text.text = "Cost: " + upgrade_cost;
                last_chosen_common_rune = "Rarity Upgrade";
                break;
        }
        
    }

    public void Level_Up_Common_Rune()
    {
        switch (last_chosen_common_rune)
        {
            case "Gold Multiplier":
                gold_multiplier_level++;
                gold_multiplier_level_text.text = "Level: " + gold_multiplier_level;
                upgrade_description.text = "The gold you earn is increased by " + gold_multiplier_level * 5f + " <color=\"green\">(+ 5)</color> %";
                break;
            case "Attack Boost":
                attack_boost_level++;
                attack_boost_level_text.text = "Level: " + attack_boost_level;
                upgrade_description.text = "Your attacks deal " + attack_boost_level * 2.5f + " <color=\"green\">(+ 2.5)</color> % more damage";
                break;
            case "Boss Timer":
                boss_timer_level++;
                boss_timer_level_text.text = "Level: " + boss_timer_level;
                upgrade_description.text = "Bosses wait " + boss_timer_level * 0.5f + " <color=\"green\">(+ 0.5)</color> % more seconds before disappearing";
                break;
            case "Rarity Upgrade":
                rarity_upgrade_level++;
                rarity_upgrade_level_text.text = "Level: " + rarity_upgrade_level;
                Get_Rarity_Bonus();
                upgrade_description.text = "Upgrade the rarity of your swords!";
                break;
        }
    }

    public void Choose_Epic_Rune(string upgrade)
    {
        switch (upgrade)
        {
            case "Ghost Swords":
                upgrade_name.text = "Ghost Swords";
                upgrade_description.text = "Your swords will attack on their own";
                last_chosen_common_rune = "Ghost Swords";
                break;
            case "Hold To Attack":
                upgrade_name.text = "Hold To Attack";
                upgrade_description.text = "Hold to charge your attack";
                last_chosen_common_rune = "Hold To Attack";
                break;
            case "Wave Skip":
                upgrade_name.text = "Wave Skip";
                upgrade_description.text = "Skip the next 5 waves";
                last_chosen_common_rune = "Wave Skip";
                break;
        }
    }
    
    /*
    public void Level_Up_Epic_Rune()
    {
        switch (last_chosen_common_rune)
        {
            case "Ghost Swords":
                ghost_swords_active = true;
                break;
            case "Hold To Attack":
                hold_to_attack = true;
                break;
            case "Wave Skip":
                wave_skip = true;
                break;
        }
        
    }
    */
    
    #region Common Runes
    
    public int Get_Gold_Bonus(int gold_earned)
    {
        return (int)(gold_earned * gold_multiplier_level * 0.05f);
    }
    
    public float Get_Attack_Bonus(float attack_damage)
    {
        return attack_damage * attack_boost_level * 0.025f;
    }

    public void Get_Rarity_Bonus()
    {
        inventory.sword_rarity += 1;
    }
    
    public void Get_Boss_Timer_Bonus()
    {
        combat_manager.boss_timer += boss_timer_level * 0.5f;
    }
    
    #endregion

    #region Epic Runes // Bunları detaylı düşünmen lazım

    public void Ghost_Swords()
    {
        
    }

    public void Hold_To_Attack()
    {
        
    }

    public void Wave_Skip()
    {
        
    }
    

    #endregion
    
    
    #region Temporary Upgrades
    
    public void Unlock_Battle_Slot()
    {
        battle_slot_level++;
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
    
    public void Unlock_Merge_Slot()
    {
        merge_slot_level++;
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

    public void Base_Sword_Upgrade()
    {
        base_sword_level++;
        inventory.crafting_tier++;
    }

    public void Double_Merge_Upgrade()
    {
        double_merge_level++;
        inventory.double_merge_chance = double_merge_level * 0.25f;
    }
    
    #endregion
    
    
}
