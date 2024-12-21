using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public SoldierCombat relatedSoldier;
    private Slider healthSlider;
    private float maxHealth = 100f;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    private void FixedUpdate()
    {
        if (relatedSoldier == null)
        {
            Destroy(gameObject);
        }
    }


    public void TakeDamage(float currentHp)
    {
        healthSlider.value = currentHp;
    }
}
