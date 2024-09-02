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
    public ScriptableHeroes data;
    public int evolution_level;
    public int combat_level;
    
    public Hero_Data(ScriptableHeroes hero, int evolution, int level)
    {
        data = hero;
        evolution_level = level;
        combat_level = level;
    }
    
}
