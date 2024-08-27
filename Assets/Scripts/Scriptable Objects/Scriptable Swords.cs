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
    
    
    public int crafting_cost;
    
    

}
