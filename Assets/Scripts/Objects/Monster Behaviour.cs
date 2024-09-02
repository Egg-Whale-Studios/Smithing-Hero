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
    
    
    private GameObject text_obj;
    public FloatingNumber floating_text;
    public CombatManager manager;
    
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<CombatManager>();
        text_obj = GameObject.FindGameObjectWithTag("Reward Texts").transform.GetChild(0).gameObject;
        floating_text = text_obj.GetComponent<FloatingNumber>();
    }

    private void OnEnable()
    {
        current_health = Mathf.Infinity;
    }

    public void Set_Data(float health, int reward, Color color)
    {
        max_health = health;
        current_health = health;
        this.reward = reward;
        GetComponent<SpriteRenderer>().color = color;
        if (floating_text == null) Debug.Log("Floating text is null");
        floating_text.Change_Text(Color.yellow, reward + " Gold", 2);
    }
    
    public void take_damage(float damage)
    {
        current_health -= damage;
        if (current_health <= 0)
        {
            manager.Monster_Death(reward);
        }
    }

    private void Boss_Timeout()
    {
        
    }
}