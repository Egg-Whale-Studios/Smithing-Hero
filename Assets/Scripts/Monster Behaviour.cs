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
    int reward = 100;
    
    
    
    
    private Inventory inventory;
    private GameObject floating_text;
    public MonsterManager monster_manager;
    
    
    
    
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Inventory>();
        monster_manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<MonsterManager>();
        floating_text = GameObject.FindGameObjectWithTag("Reward Texts").transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        current_health = Mathf.Infinity;
    }
    
    void Update()
    {
        if (current_health <= 0)
        {
            Death();
        }
        else if(is_boss) // Zaman tut
        {
            
        }
    }

    public void Set_Data(float health, int reward, Color color)
    {
        max_health = health;
        current_health = health;
        this.reward = reward;
        GetComponent<SpriteRenderer>().color = color;
    }
    
    public void take_damage(float damage)
    {
        current_health -= damage;
    }

    private void Death()
    {
        inventory.Change_Gold(reward);
            
        floating_text.transform.position = transform.position;
        floating_text.SetActive(true);
        floating_text.GetComponent<FloatingNumber>().Change_Text(Color.yellow, reward + " Gold", 2);
        StartCoroutine(monster_manager.Generate_Monster());
        monster_manager.Wave_Change();
        gameObject.SetActive(false);
    }

    private void Boss_Timeout()
    {
        
    }
}