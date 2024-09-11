using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [Header("Monster")]
    public GameObject monster;
    public GameObject battle_scene;
    private MonsterBehaviour monster_data;
    private float health_value;
    public Sprite[] monster_sprites;
    List<Color> colors = new List<Color>();
    List<string> elements = new List<string>();
    
    [Header("Wave")]
    [NonSerialized] public int wave;
    public GameObject wave_indicator;
    private Image wave_sprite;
    public Sprite[] wave_images;
    public float boss_timer;

    [NonSerialized] public bool wave_skip;
    private bool wave_skip_part;
    private int wave_skip_start;
    private float wave_skip_timer;

    public GameObject boss_timer_indicator;
    
    
    [Header("Reward")]
    private Inventory inventory;
    private Upgrades upgrades;
    private int reward_value;
    
    [Header("Other")]
    public SwordBehaviour[] sword_scripts;
    public FloatingNumber floating_text;
    

    void Start()
    {
        StartCoroutine(Generate_Monster());
        monster_data = monster.GetComponent<MonsterBehaviour>();
        inventory = Inventory.instance;
        upgrades = Upgrades.instance;
        wave_sprite = wave_indicator.GetComponent<Image>();
        wave = 1;
        boss_timer = 20;
        colors = new List<Color>() {Color.white, Color.cyan , Color.green, Color.red, Color.yellow };
        elements = new List<string>() { "charge", "earth", "fire" };
        
    }


    #region Monster Battle
    
    public IEnumerator Generate_Monster()
    {
        
        yield return new WaitForSeconds(2);
        
        monster.SetActive(true);

        if (wave % 5 == 0) boss_timer_indicator.SetActive(true);
        
        
        health_value = Mathf.Pow(2, wave);
        reward_value= (int)Mathf.Pow(2.2f, wave);
        monster_data.Set_Data(health_value, monster_sprites[(wave-1)% monster_sprites.Length], colors[(int)((wave - 1)/ monster_sprites.Length)], elements[Random.Range(0,3)]);
        foreach(var sword in sword_scripts)
        {
            sword.Buff_By_Element();
        }
    }
    
    public void Monster_Death()
    {
        
        reward_value += upgrades.Get_Gold_Bonus(reward_value);
        
        floating_text.Change_Text(Color.yellow, reward_value + " Gold", 1);
        inventory.Change_Gold(reward_value);
        
        floating_text.AnimateFloatingNumber(transform);
        Next_Stage();
        
        monster.SetActive(false);
    }

    public void Monster_Escape()
    {
        Fall_Back();
        monster.SetActive(false);
    }

    public void Check_Wave_Skip()
    {
        wave_skip_part = !wave_skip_part;
        
        if(wave_skip_part)
        {
            wave_skip_start = wave;
            wave_skip_timer = Time.time + 20; // Kaç saniyye verceksek
        }
        else
        {
            if (Time.time < wave_skip_timer)
            {
                wave += 6;
                Wave_Update();
            }
            Check_Wave_Skip();
        }
        
    }
    
    #endregion


    #region Wave Management

    public void Next_Stage()
    {
        wave++;
        Wave_Update();
    }
    
    public void Fall_Back()
    {
        wave = (wave % 5 == 0 ? wave -= 5 : wave -= wave % 5);
        
        Wave_Update();
    }
    
    private void Wave_Update()
    {
        wave_sprite.sprite = wave_images[wave % 5];
        StartCoroutine(Generate_Monster());

        if (wave % 5 == 1)
        {
            Check_Wave_Skip();
        }
    }

    #endregion
    
    
    
    
    
}
