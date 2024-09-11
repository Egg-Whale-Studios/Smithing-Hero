using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster")]
public class ScriptableMonsters : ScriptableObject
{
    public string monster_name;
    public Sprite monster_sprite;
    public int health;
    public bool is_boss;
    public bool is_elemental;
    public string element;
}

public class Monster_Data
{
    public ScriptableMonsters data;
    public int wave_level;
    
    
    public Monster_Data(ScriptableMonsters monster, int wave_count)
    {
        data = monster;
        wave_level = wave_count;
        
    }
    
}
