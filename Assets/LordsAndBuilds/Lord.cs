using System;
using UnityEngine;

public class Lord : MonoBehaviour, ICombat
{
    public static Action<Lord> OnLordDestroyed;

    [Header("VFX")]
    [SerializeField] private GameObject stoneHitEffect;

    [Header("* * Numerical Values * * ")]
    [SerializeField] private float maxHP = 500f;
    
    public int gold = 0;
    public int currentMinerCount = 0;

    [Header("** UI **")]
    [SerializeField] private HealthBarUI healthBarUI;

    private float currentHp;


    private void Start()
    {
        healthBarUI.relatedBeing = this;
        healthBarUI.SetMaxHealth(maxHP);
        currentHp = maxHP;

    }

    public void AddGold(float amount)
    {

        gold += (int)amount;
    }


    public void TakeDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        healthBarUI.TakeDamage(currentHp);
        Destroy(Instantiate(stoneHitEffect, transform.position, Quaternion.identity), .75f);

        if (currentHp <= 0)
        {
            OnLordDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
