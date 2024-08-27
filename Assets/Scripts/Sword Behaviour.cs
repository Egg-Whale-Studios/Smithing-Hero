using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    public ScriptableSwords data;
    
    [Header("Stats")]
    private int sword_damage;
    private int sword_speed;
    private bool is_elemental;
    private string element_type;
    private bool is_shiny;
    void Start()
    {
        sword_damage = data.sword_damage;
        sword_speed = data.sword_speed;
        is_elemental = data.is_elemental;
        element_type = data.element_type;
        is_shiny = data.is_shiny;
    }

    
    void Update()
    {
        
    }
}
