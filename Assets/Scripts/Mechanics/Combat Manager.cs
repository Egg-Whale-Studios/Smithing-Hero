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
    
    [Header("Wave")]
    [NonSerialized] public int wave;
    public GameObject wave_indicator;
    private Image wave_sprite;
    public Sprite[] wave_images;
    
    [Header("Reward")]
    private Inventory inventory;
    
    

    void Start()
    {
        StartCoroutine(Generate_Monster());
        monster_data = monster.GetComponent<MonsterBehaviour>();
        inventory = Inventory.instance;
        wave_sprite = wave_indicator.GetComponent<Image>();
        wave = 1;
    }


    #region Monster Battle
    
    public IEnumerator Generate_Monster()
    {
        
        yield return new WaitForSeconds(2);
        List<Color> colors = new List<Color>() { Color.cyan , Color.green, Color.red, Color.yellow };
        monster.SetActive(true);
        monster_data.Set_Data(50,100,colors[Random.Range(0,colors.Count - 1)]);
    }
    
    public void Monster_Death(int reward)
    {
        inventory.Change_Gold(reward);
        
        monster_data.floating_text.AnimateFloatingNumber(transform);
        Next_Stage();
        
        StartCoroutine(Generate_Monster());
        monster.SetActive(false);
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
        wave -= wave % 5;
        Wave_Update();
    }
    
    private void Wave_Update()
    {
        wave_sprite.sprite = wave_images[wave % 5];
    }

    #endregion
    
    
    
    
    
}
