using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableHeroes : ScriptableObject
{
    public new string name;
    public Sprite hero_sprite;
    public int rarity;
    public Buff_Type buff_type;
    public float cooldown = 1.5f;

    public float speedBoost;
    public float damageBoost;


    public void Buff(float dmg, float spd,int hero_combat_level, int hero_evolve_level, SwordBehaviour script)
    {
        
        
        switch (rarity)
        {
            case 1: // common
                damageBoost = (10 + 2.5f * hero_combat_level) * hero_evolve_level;
                break;
            case 2: // epic - Elemental Boosts
                damageBoost = (50 + 5 * hero_combat_level) * hero_evolve_level;
                break;
            case 3: // legendary
                damageBoost = (100 + 10 * hero_combat_level) * hero_evolve_level;
                break;
        }

        
        script.damage_to_deal *= damageBoost/100;
        spd *= speedBoost;
    }

}


public enum Buff_Type
{
    Damage,
    Speed,
    Charge,
    Earth,
    Fire,
    Money,
    Special
}

public class Hero_Data
{
    public ScriptableHeroes hero_data;
    public int evolution_level;
    public int combat_level;
    
    public Hero_Data(ScriptableHeroes hero, int evolution, int level)
    {
        hero_data = hero;
        evolution_level = level;
        combat_level = level;
    }
    
}