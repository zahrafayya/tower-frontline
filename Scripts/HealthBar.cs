using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health value
    
    private Transform childObjectHealth;
    private Pawn pawn;
    private float actualDamage;
    private int currentHealth; // Current health value

    void Start()
    {
        currentHealth = maxHealth;
        pawn = GetComponent<Pawn>();
        
        childObjectHealth = transform.Find("HealthBar");
    }

    public void DecreaseHealth(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            float healthPercentage = (float)currentHealth / maxHealth;

            float newScaleX = healthPercentage * 0.7f;

            Vector3 newScale = childObjectHealth.localScale;
            newScale.x = newScaleX;

            childObjectHealth.localScale = newScale;

            if (currentHealth <= 0)
            {
                pawn.OnDeadEvent();
            }
        }
    }
}
