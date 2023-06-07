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
    private float length;

    void Start()
    {
        currentHealth = maxHealth;
        pawn = GetComponent<Pawn>();

        childObjectHealth = transform.Find("HealthBar");
        
        length = childObjectHealth.localScale.x;
    }

    public void DecreaseHealth(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            float healthPercentage = (float)currentHealth / maxHealth;

            float newScaleX = healthPercentage * length;

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
