using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarComponent : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    private float _originalScale;
   

    void Start()
    {
        _originalScale = gameObject.transform.localScale.x;
        
    }

    void Update()
    {
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * _originalScale;
        gameObject.transform.localScale = tmpScale;
    }

    public void Damage(float amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;
        }
    }
}


