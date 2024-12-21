using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public ICombat relatedBeing;
    private Slider healthSlider;
    private float maxHealth = 100f;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        SetMaxHealth(maxHealth);
    }

    public void SetMaxHealth(float maxHP)
    {
        healthSlider.maxValue = maxHP;
        healthSlider.value = maxHP;
    }


    public void TakeDamage(float currentHp)
    {
        healthSlider.value = currentHp;
        print("current hp :"+ currentHp);
    }
}
