using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTimer : MonoBehaviour
{
    public CombatManager manager;
    
    private float max_timer;
    private float current_timer;

    public RectTransform fill;

    void OnEnable()
    {
        max_timer = manager.boss_timer;
        current_timer = max_timer;
    }

    
    void FixedUpdate()
    {
        
        current_timer -= Time.deltaTime;
        fill.localScale = new Vector3(current_timer / max_timer, 1, 1);
        if (current_timer <= 0)
        {
            manager.Monster_Escape();
            gameObject.SetActive(false);
        }
    }
}
