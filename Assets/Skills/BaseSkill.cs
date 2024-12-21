using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] protected float radius = 5f; // Etki alaný yarýçapý
    [SerializeField] protected LayerMask targetLayer; // Hedef katmaný
    [Header("Particle Settings")]
    [SerializeField] protected ParticleSystem effectParticle; // Efekt partikülü

    // Küre içindeki nesneleri bul
    protected Collider[] GetObjectsInArea()
    {
        return Physics.OverlapSphere(transform.position, radius, targetLayer);
    }

    // Her sýnýf kendi efektini uygular
    public abstract void ApplyEffect();

    // Particle sistemini çalýþtýr
    protected void PlayParticleEffect()
    {
        Instantiate(effectParticle, new Vector3(10,0 ,-7), Quaternion.identity);

    }

    // Debug için etki alanýný göster
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Dinamik yarýçap ayarlama
    public virtual void SetRadius(float newRadius)
    {
        radius = newRadius;
    }

    public virtual float GetRadius()
    {
        return radius;
    }
}
