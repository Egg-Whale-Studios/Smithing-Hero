using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterBehaviour : MonoBehaviour
{
    [Header("Stats")]
    public float max_health = 100;
    private float current_health;
    [NonSerialized] public bool is_boss;
    [NonSerialized] public string element_type;
    
    
    public CombatManager manager;
    private SpriteRenderer sprite_renderer;
    
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<CombatManager>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        current_health = Mathf.Infinity;
    }

    public void Set_Data(float health, Sprite appearance,Color color, string element)
    {
        max_health = health;
        current_health = health;
        sprite_renderer.color = color;
        sprite_renderer.sprite = appearance;
        
    }
    
    public void take_damage(float damage)
    {
        current_health -= damage;
        if (current_health <= 0)
        {
            manager.Monster_Death();
        }
    }
}