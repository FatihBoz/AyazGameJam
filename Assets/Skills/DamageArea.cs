using UnityEngine;

public class FireAreaEffect : BaseSkill
{
    [Header("Fire Settings")]
    public float burnDamage = 10f; // Yanma hasarý

    private void Start()
    {
        SetRadius(7f); // Özel yarýçap deðeri
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
