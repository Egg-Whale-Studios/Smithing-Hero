using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwordBehaviour : MonoBehaviour
{
    public ScriptableSwords data;
    
    private Vector2 monster_size;
    public Rigidbody2D body;
    
    public GameObject damage_text;
    public SpriteRenderer sprite_renderer;
    private Sprite sword_sprite;
    public TrailRenderer trail_renderer;

    public Hero_Data data_from_hero;


    private bool monster_present;

    [Header("Stats")]
    [NonSerialized] public float sword_damage;
    [NonSerialized] public float damage_to_deal;
    [NonSerialized] public float sword_speed;
    private bool is_elemental;
    private string element_type;
    private bool is_shiny;
    private float attack_cooldown;
    
    private Upgrades upgrades;

    public bool is_heroic = false;

    public Color gizmoColor = Color.red;
    
    private int battle_sword_ind;

    [Header("Movement")] 
    public float HorizontalDistance;
    public float VerticalDistance;
    string pos="a";
    Vector3 posit;
    
    public GameObject monster;
    public MonsterBehaviour monster_data;
    private BoxCollider2D monster_target_gen_collider;
    
    
    void Start()
    {
        upgrades = Upgrades.instance;
        attack_cooldown = 3f;
        
        sprite_renderer = GetComponent<SpriteRenderer>();
        
        Vector2 monster_size = monster.GetComponent<BoxCollider2D>().size;
        
        HorizontalDistance = monster_size.x * 1.5f;
        VerticalDistance = monster_size.y * 1.5f;
        
    }
    
    

    void OnEnable()
    {
        sword_damage = data.sword_damage;
        sword_speed = data.sword_speed;
        is_elemental = data.is_elemental;
        element_type = data.element_type;
        is_shiny = data.is_shiny;
        sprite_renderer.sprite = data.sword_sprite;
        damage_text.GetComponent<FloatingNumber>().Change_Text(Color.white, sword_damage.ToString(), 1);

        
        trail_renderer.enabled = true;
        
        switch (element_type)
        {
           
            
            case "Fire":
                trail_renderer.startColor = Color.red;
                trail_renderer.endColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
                break;
            case "Earth":
                trail_renderer.startColor = Color.gray;
                trail_renderer.endColor = new Color(Color.grey.r, Color.grey.g, Color.grey.b, 0.3f);
                break;
            case "Charge":
                trail_renderer.startColor = Color.yellow;
                trail_renderer.endColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.3f);
                break;
            default:
                trail_renderer.enabled = false;
                break;
        }
        
        Buff_By_Element();
        Monster_Check();
        
        
    }
    

    public void Buff_By_Element()
    {
        switch (element_type)
        {
            case "Fire":
                if (monster_data.element_type == "fire")
                {
                    sword_damage *= 1.5f;
                }
                else
                {
                    sword_damage= data.sword_damage;
                }
                break;
            case "Earth":
                if (monster_data.element_type == "earth")
                {
                    sword_damage *= 1.5f;
                }
                   
                else
                {
                    sword_damage = data.sword_damage;
                }
                break;
            case "Charge":
                if (monster_data.element_type == "charge")
                {
                    sword_damage *= 1.5f;
                }
                else
                {
                    sword_damage = data.sword_damage;
                }
                break;

        }
    }
    
    public IEnumerator AttackAlways()
    {
        if (is_heroic)
        {
            Vector2 temp_pos = posit;
            CalculatePos();
            RotateTowards(posit);
            
            transform.DOMove(posit, attack_cooldown/sword_speed).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(data_from_hero.hero_data.cooldown);
            
            StartCoroutine(AttackAlways());
        }
    }
        
    
    public void Set_Script(ScriptableSwords script)
    {
        data = script;
    }
    
    private void Monster_Check() // optimize et
    {
        
        monster_target_gen_collider = monster.GetComponent<BoxCollider2D>();
        monster_size = monster_target_gen_collider.size;
    }
    
    #region Sword Movement
    public void moveNextPos()
    {
        
        CalculatePos();
        RotateTowards(posit);
        float delay_time = Random.Range(0f, 0.2f);
        transform.DOMove(posit,1f).SetEase(Ease.InOutBack).SetDelay(delay_time);

        
    }

    
    void CalculatePos()
    {
        
        int option= Random.Range(1, 4);
        
        if (pos == "a")
        {
            if (option == 1)
            {
                selectOnD();
            }
            if (option == 2)
            {
                selectOnE();
            }
            if (option == 3)
            {
                selectOnF();
            }
        }
        else if (pos == "b")
        {
            if (option == 1)
            {
                selectOnG();
            }
            if (option == 2)
            {
                selectOnE();
            }
            if (option == 3)
            {
                selectOnF();
            }
        }
        else if (pos == "c")
        {
            if (option == 1)
            {
                selectOnG();
            }
            if (option == 2)
            {
                selectOnH();
            }
            if (option == 3)
            {
                selectOnF();
            }
        }
        else if (pos == "d")
        {
            if (option == 1)
            {
                selectOnG();
            }
            if (option == 2)
            {
                selectOnH();
            }
            if (option == 3)
            {
                selectOnA();
            }
        }
        else if(pos == "e")
        {
            if (option == 1)
            {
                selectOnB();
            }
            if (option == 2)
            {
                selectOnH();
            }
            if (option == 3)
            {
                selectOnA();
            }
        }
        else if (pos == "f")
        {
            if (option == 1)
            {
                selectOnB();
            }
            if (option == 2)
            {
                selectOnC();
            }
            if (option == 3)
            {
                selectOnA();
            }
        }
        else if (pos == "g")
        {
            if (option == 1)
            {
                selectOnB();
            }
            if (option == 2)
            {
                selectOnC();
            }
            if (option == 3)
            {
                selectOnD();
            }
        }
        else if (pos == "h")
        {
            if (option == 1)
            {
                selectOnE();
            }
            if (option == 2)
            {
                selectOnC();
            }
            if (option == 3)
            {
                selectOnD();
            }
        }
        
        posit = monster.transform.position + posit;
    }
    
    void selectOnA()
    {
      
        int option = Random.Range(1, 3);
        if (option == 1)
        {
            float y = Random.Range(1 * VerticalDistance / 3, 3 * VerticalDistance / 3);
            float x = -1*HorizontalDistance;
            posit = new Vector2(x, y);
        }
        if (option == 2)
        {
            float x = Random.Range(-1*HorizontalDistance / 3, -1 * HorizontalDistance / 3);
            float y = VerticalDistance;
            posit = new Vector2(x, y);
        }
        pos = "a";
    }
    void selectOnB()
    {
     
        float x = Random.Range(-1 * HorizontalDistance / 3, HorizontalDistance / 3);
        float y = VerticalDistance;
        posit = new Vector2(x, y);
        pos = "b";
    }
    void selectOnC()
    {
     
        int option = Random.Range(1, 3);
        if (option == 1)
        {
            float y = Random.Range(1 * VerticalDistance / 3, 3 * VerticalDistance / 3);
            float x = 1 * HorizontalDistance;
            posit = new Vector2(x, y);
        }
        if (option == 2)
        {
            float x = Random.Range(1 * HorizontalDistance / 3, 3 * HorizontalDistance / 3);
            float y = VerticalDistance;
            posit = new Vector2(x, y);
        }
        pos = "c";
    }
    void selectOnD()
    {
        
        float y = Random.Range(-1 * VerticalDistance / 3, VerticalDistance / 3);
        float x = HorizontalDistance;
        posit = new Vector2(x, y);
        pos = "d";
    }
    void selectOnE()
    {
      
        int option = Random.Range(1, 3);
        if (option == 1)
        {
            float y = Random.Range(-3 * VerticalDistance / 3, -1*VerticalDistance / 3);
            float x = HorizontalDistance;
            posit = new Vector2(x, y);
        }
        if (option == 2)
        {
            float x = Random.Range(HorizontalDistance / 3, 3 * HorizontalDistance / 3);
            float y = -1*VerticalDistance;
            posit = new Vector2(x, y);
        }
        pos = "e";
    }
    void selectOnF()
    {
      
        float x = Random.Range(-1 * HorizontalDistance / 3, HorizontalDistance / 3);
        float y = -1*VerticalDistance;
        posit = new Vector2(x, y);
        pos = "f";
    }
    void selectOnG()
    {
        
        int option = Random.Range(1, 3);
        if (option == 1)
        {
            float y = Random.Range(-3 * VerticalDistance / 3, -1 * VerticalDistance / 3);
            float x = -1 * HorizontalDistance;
            posit = new Vector2(x, y);
        }
        if (option == 2)
        {
            float x = Random.Range(-3 * HorizontalDistance / 3, -1 * HorizontalDistance / 3);
            float y = -1*VerticalDistance;
            posit = new Vector2(x, y);
        }
        pos = "g";
    }
    void selectOnH()
    {
        
        float y = Random.Range(-1 * VerticalDistance / 3, VerticalDistance / 3);
        float x = -1*HorizontalDistance;
        posit = new Vector2(x, y);
        pos = "h";
    }
    
    private void RotateTowards(Vector2 targetPosition)
    {
        Vector3 direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.DORotate(new Vector3(0, 0, angle - 90), 0.5f).SetEase(Ease.InOutSine);
    }
    
    
    #endregion
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            damage_to_deal = 0;
            
            damage_to_deal = sword_damage + upgrades.Get_Attack_Bonus(sword_damage);
            
            if(data_from_hero != null) data_from_hero.hero_data.Buff(damage_to_deal, sword_speed,data_from_hero.combat_level,data_from_hero.evolution_level, this);
            damage_text.GetComponent<FloatingNumber>().Change_Text(Color.white, damage_to_deal.ToString(), 1);
            damage_text.GetComponent<FloatingNumber>().AnimateFloatingNumber(transform);
            other.gameObject.GetComponent<MonsterBehaviour>().take_damage(damage_to_deal);
        }
    }

    void OnDrawGizmos()
    {
        // Gizmos'un rengini ayarlayın
        Gizmos.color = gizmoColor;

        // 2D noktayı işaretlemek için bir daire çiziyoruz
        Gizmos.DrawSphere(posit, 0.5f);
    }
    


}