using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float Damage = 10f; // Yanma hasarý
    public float radius;
    public LayerMask targetLayer;

    public void ApplyEffect()
    {
        Collider[] targets = GetObjectsInArea();

        foreach (Collider target in targets)
        {
            //Give Damage
            if(target.gameObject.TryGetComponent<ICombat>(out var enemy))
            {
                enemy.TakeDamage(Damage);
                Debug.Log("Giving Damage to : " + target.gameObject);
            }
        }
    }
    Collider[] GetObjectsInArea()
    {
        return Physics.OverlapSphere(transform.position, radius, targetLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Turuncu yarý saydam
        Gizmos.DrawSphere(transform.position, radius);

        Gizmos.color = Color.red; // Kenar çizgisi için kýrmýzý
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
