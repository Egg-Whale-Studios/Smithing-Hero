using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwordBehaviour : MonoBehaviour
{
    public ScriptableSwords data;
    private GameObject battle_scene;
    private GameObject monster;
    private BoxCollider2D monster_target_gen_collider;
    private Vector2 monster_size;
    private Rigidbody2D body;
    private float x_force;
    private float y_force;
    public GameObject damage_text;

    private bool monster_present;

    [Header("Stats")]
    private float sword_damage;
    private float sword_speed;
    private bool is_elemental;
    private string element_type;
    private bool is_shiny;

    
    private int battle_sword_ind;

    private Vector3 attack_direction;
    void Awake()
    {
        sword_damage = data.sword_damage;
        sword_speed = data.sword_speed;
        is_elemental = data.is_elemental;
        element_type = data.element_type;
        is_shiny = data.is_shiny;
        
        body = GetComponent<Rigidbody2D>();
        battle_scene = GameObject.FindGameObjectWithTag("Battle Window");
        monster = battle_scene.transform.GetChild(battle_scene.transform.childCount - 1).gameObject;
        
        Monster_Check(); // Monster değiştiğinde tekrar kontrol etmeyi yazmayı unutma
        Turn_Around();
        StartCoroutine(Hitagain());
        
        damage_text.GetComponent<FloatingNumber>().Change_Text(Color.white, sword_damage.ToString(), 1);
        
    }
    
    

    void Update()
    {
        
        if ((monster.transform.position - transform.position).magnitude > 1000)
        {
            Turn_Around();
        }

        body.AddForce((attack_direction - transform.position).normalized * sword_speed);


        body.velocity = sword_speed * body.velocity.normalized;

        transform.rotation =
            Quaternion.Euler(0, 0, Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg - 90);
    
        
    }

    private void Monster_Check()
    {
        
        monster = GameObject.FindGameObjectWithTag("Monster");
        monster_target_gen_collider = monster.GetComponent<BoxCollider2D>();
        monster_size = monster_target_gen_collider.size;
    }


    private void Turn_Around()
    {

        float temp_x = Random.Range(monster.transform.position.x - monster_size.x / 2, monster.transform.position.x + monster_size.x / 2);
        float temp_y = Random.Range(monster.transform.position.y - monster_size.y / 2, monster.transform.position.y + monster_size.y / 2);
        

        attack_direction = new Vector3(temp_x, temp_y,0);
        

    }

    public IEnumerator Hitagain()
    {
        
        yield return new WaitForSeconds(0.5f);
        Turn_Around();
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attack_direction, 0.5f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            
            damage_text.transform.position = transform.position;
            damage_text.transform.localScale = Vector2.one;
            damage_text.SetActive(true);
            other.gameObject.GetComponent<MonsterBehaviour>().take_damage(sword_damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            StartCoroutine(Hitagain());
        }
    }
}