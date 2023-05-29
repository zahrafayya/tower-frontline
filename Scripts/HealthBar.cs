using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health value
    private Transform childObjectHealth;

    private int currentHealth; // Current health value

    void Start()
    {
        currentHealth = maxHealth;
        childObjectHealth = transform.Find("HealthBar");
    }

    public void DecreaseHealth()
    {
        currentHealth -= 5;

        float healthPercentage = (float)currentHealth / maxHealth;
        print("health percentage");
        print(healthPercentage);

        float newScaleX = childObjectHealth.localScale.x * healthPercentage;
        print("newScaleX");
        print(newScaleX);

        Vector3 newScale = childObjectHealth.localScale;
        newScale.x = newScaleX;

        childObjectHealth.localScale = newScale;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
