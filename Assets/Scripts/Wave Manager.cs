using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [NonSerialized] public int wave;
    public GameObject wave_indicator;
    public Sprite[] wave_images;
    
    void Start()
    {
        wave = 1;
    }

    public void Next_Stage()
    {
        wave++;
    }
    
    public void Fall_Back()
    {
        wave -= wave % 5;
    }
    
    private void Wave_Update()
    {
        wave_indicator.GetComponent<Image>().sprite = wave_images[wave % 5];
    }
    
}
