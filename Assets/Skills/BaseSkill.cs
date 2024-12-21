using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] protected float radius = 5f; // Etki alan� yar��ap�
    [SerializeField] protected LayerMask targetLayer; // Hedef katman�
    [Header("Particle Settings")]
    [SerializeField] protected ParticleSystem effectParticle; // Efekt partik�l�

    // K�re i�indeki nesneleri bul
    protected Collider[] GetObjectsInArea()
    {
        return Physics.OverlapSphere(transform.position, radius, targetLayer);
    }

    // Her s�n�f kendi efektini uygular
    public abstract void ApplyEffect();

    // Particle sistemini �al��t�r
    protected void PlayParticleEffect()
    {
        Instantiate(effectParticle, new Vector3(10,0 ,-7), Quaternion.identity);

    }

    // Debug i�in etki alan�n� g�ster
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Dinamik yar��ap ayarlama
    public virtual void SetRadius(float newRadius)
    {
        radius = newRadius;
    }

    public virtual float GetRadius()
    {
        return radius;
    }
}
