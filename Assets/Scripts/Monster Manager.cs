using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterManager : MonoBehaviour
{
    public GameObject monster;
    private WaveManager wave_manager;
    public GameObject battle_scene;

    private void Awake()
    {
        monster = Instantiate(monster, battle_scene.transform);
    }

    void Start()
    {
        wave_manager = GetComponent<WaveManager>();
        StartCoroutine(Generate_Monster());
    }
    
    
    
    
    public IEnumerator Generate_Monster()
    {
        yield return new WaitForSeconds(2);
        List<Color> colors = new List<Color>() { Color.cyan , Color.green, Color.red, Color.yellow };
        monster.SetActive(true);
        monster.GetComponent<MonsterBehaviour>().Set_Data(50,100,colors[Random.Range(0,colors.Count - 1)]);
    }
    
    public void Wave_Change()
    {
        wave_manager.Next_Stage();
    }
    
    public void Boss_Fail()
    {
        wave_manager.Fall_Back();
    }
    
    
}
