using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sword", menuName = "Sword")]
public class ScriptableSwords : ScriptableObject
{
    [Header("Sword Info")]
    public string sword_name;
    public Sprite sword_sprite;
    
    [Header("Sword Stats")]
    public int tier;
    public int sword_damage;
    public int sword_speed;
    public bool is_elemental;
    public string element_type;
    public bool is_shiny;
    public ElementType type;
}


public enum ElementType
{
    type0, 
    type1,
    type2,
    type3,
}

public class Sword_Data
{
    public ScriptableSwords data;
    public int level;
    public int rarity;
    public int tier;
    
    public Sword_Data(ScriptableSwords sword, int sword_rarity, int sword_tier, int sword_level)
    {
        data = sword;
        rarity = sword_rarity;
        tier = sword_tier;
        level = sword_level;
        
        
        
    }
}