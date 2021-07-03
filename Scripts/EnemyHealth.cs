using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100.0f;

    public GameObject healthBarUI;
    public Slider slider;

    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();

    }

    void Update()
    {
        slider.value = CalculateHealth();
        
        if ((health > 0) && (health <= maxHealth))
        {
            healthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            healthBarUI.SetActive(false);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

    }

    internal void Damage(float dmg)
    {
        health -= dmg;
        Debug.Log("new script health = " + health);
    }


    float CalculateHealth()
    {
        return health / maxHealth;
    }


}
