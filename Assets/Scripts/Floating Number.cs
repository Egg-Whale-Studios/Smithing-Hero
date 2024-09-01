using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    private Color color;
    private float size;
    private string text;
    public TMPro.TextMeshPro text_mesh;
    
    

    public void Change_Text(Color color, string text, float size)
    {
        
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
    
    public void AnimateFloatingNumber(Transform targetPosition)
    {
        transform.position = targetPosition.position;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        transform.DOMoveY(transform.position.y + 1, 1).SetEase(Ease.OutQuad);
        transform.DOScale(Vector3.zero, 1).SetEase(Ease.OutQuad);
    }
}
