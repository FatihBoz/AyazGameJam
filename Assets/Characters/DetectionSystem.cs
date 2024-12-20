using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public float detectionRadius = 10f; // D��man alg�lama yar��ap�
    public LayerMask enemyLayer; // D��manlar�n bulundu�u layer

    private SoldierCombat unitController;

    void Start()
    {
        unitController = GetComponent<SoldierCombat>();
    }

    void Update()
    {
        // Alg�lama alan�ndaki d��manlar� kontrol et
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // En yak�n d��man� hedefle
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);
            unitController.SetEnemyTarget(closestEnemy);
        }
        else
        {
            // D��man yoksa hedefi temizle
            unitController.ClearEnemyTarget();
        }
    }

    private Transform FindClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
