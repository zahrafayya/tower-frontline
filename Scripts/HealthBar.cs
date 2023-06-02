using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health value
    private Transform childObjectHealth;
    private float actualDamage;

    private int currentHealth; // Current health value

    void Start()
    {
        currentHealth = maxHealth;
        childObjectHealth = transform.Find("HealthBar");
    }

    public void DecreaseHealth(int damage)
    {
        currentHealth -= damage;

        float healthPercentage = (float)currentHealth / maxHealth;

        float newScaleX = healthPercentage;

        Vector3 newScale = childObjectHealth.localScale;
        newScale.x = newScaleX;

        childObjectHealth.localScale = newScale;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
