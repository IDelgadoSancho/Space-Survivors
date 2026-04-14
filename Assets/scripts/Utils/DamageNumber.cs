using System;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    
    void Start()
    {
        Destroy(gameObject,1);
    }

    
    void Update()
    {
        
    }

    public void SetValue(int value)
    {
        damageText.text = value.ToString();
    }
}
