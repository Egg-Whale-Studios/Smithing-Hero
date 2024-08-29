using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{

    private Color color;
    private float size;
    private string text;
    
    
    public void Change_Text(Color color, string text, float size)
    {
        TMPro.TextMeshPro text_mesh = GetComponent<TMPro.TextMeshPro>();
        text_mesh.color = color;
        text_mesh.fontSize *= size;
        text_mesh.text = text;
    }

    
    void OnEnable()
    {

        StartCoroutine(countdown());
    }

    private IEnumerator countdown()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        
        transform.position += new Vector3(0, 0.002f, 0);
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime);
    
    }
}
