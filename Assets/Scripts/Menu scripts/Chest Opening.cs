using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class ChestOpening : MonoBehaviour
{
    public GameObject animation_window;
    public Image black_screen;
    public RectTransform chest_rect;
    private Vector3 chest_start_pos;
    public Image chest;
    public Image hero;
    public RectTransform hero_rect;
    private Vector3 hero_start_pos;
    public GameObject close_button;

    public Sprite[] possible_chests;
    public ScriptableHeroes[] possible_heroes;

    public Sprite empty;

    private Inventory inventory;
    
    private void Start()
    {
        inventory = Inventory.instance;
        chest_start_pos = chest_rect.transform.position;
        hero_start_pos = hero_rect.transform.position;
    }

    public void Set_Chest_Data()
    {
        
    }

    public ScriptableHeroes Hero_chest_Loot()
    {
        return possible_heroes[UnityEngine.Random.Range(0, possible_heroes.Length)];
    }


    public void Open_Chest(int chest_ind)
    {
        close_button.SetActive(false);
        ScriptableHeroes temp_hero = Hero_chest_Loot();
        inventory.Add_Hero(temp_hero);
        
        animation_window.SetActive(true);
        chest_rect.position = new Vector3(chest_rect.position.x, chest_start_pos.y, chest_rect.position.z);
        hero_rect.position = new Vector3(hero_rect.position.x, hero_start_pos.y, hero_rect.position.z);
        hero.sprite = empty;
        
        chest.sprite = possible_chests[chest_ind];

        black_screen.DOFade(0, 1).From().OnComplete(() =>
        {
            chest_rect.DOAnchorPosY(0, 2f).SetEase(Ease.OutBack);
            chest_rect.DOScale(new Vector3(0.2f,0.2f,0.2f), 2).SetEase(Ease.Linear).From().OnComplete(() =>
            {
                chest_rect.DOShakeRotation(3, 30f, 20).OnComplete(() =>
                {
                    chest.sprite = possible_chests[chest_ind + 1];
                    hero.sprite = temp_hero.hero_sprite;
                    hero.color = Color.black;
                    hero_rect.DOAnchorPosY(500, 2).SetEase(Ease.OutBack);
                    hero_rect.DOScale(new Vector3(0.2f, 0.2f,0.2f),2).SetEase(Ease.Linear).From();
                    hero.DOColor(Color.white, 0.1f).SetDelay(3).OnComplete(() =>
                    {
                        close_button.SetActive(true);
                        
                    });

                });
            });
            
        });
        
        
        
        
    }
}
