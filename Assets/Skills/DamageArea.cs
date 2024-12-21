using UnityEngine;

public class FireAreaEffect : BaseSkill
{
    [Header("Fire Settings")]
    public float burnDamage = 10f; // Yanma hasar�

    private void Start()
    {
        SetRadius(7f); // �zel yar��ap de�eri
    }

    public override void ApplyEffect()
    {
        Collider[] targets = GetObjectsInArea();

        foreach (Collider target in targets)
        {
            //Give Damage
        }
        PlayParticleEffect();
    }
}
