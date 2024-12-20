using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public float detectionRadius = 10f; // Düþman algýlama yarýçapý
    public LayerMask enemyLayer; // Düþmanlarýn bulunduðu layer

    private SoldierCombat unitController;

    void Start()
    {
        unitController = GetComponent<SoldierCombat>();
    }

    void Update()
    {
        // Algýlama alanýndaki düþmanlarý kontrol et
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // En yakýn düþmaný hedefle
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);
            unitController.SetEnemyTarget(closestEnemy);
        }
        else
        {
            // Düþman yoksa hedefi temizle
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
