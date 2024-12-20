using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent; // Karakterin hareket sistemi
    public Transform targetTower; // Hedef kule
    public float attackRange = 5f; // Düþmana saldýrý mesafesi
    public LayerMask enemyLayer; // Düþmanlarýn layer'ý
    public float detectionRadius = 10f; // Düþman algýlama yarýçapý

    private Transform currentEnemyTarget; // Algýlanan düþman

    void Start()
    {
        if (targetTower != null)
        {
            // Kuleye doðru hareket baþlat
            MoveToPos(targetTower.position);
        }
    }

    void Update()
    {
        if (currentEnemyTarget != null)
        {
            // Düþmana saldýr
            agent.isStopped = true;
            transform.LookAt(currentEnemyTarget); // Düþmana bak
            Attack();
        }
        else
        {
            // Yakýndaki düþmanlarý kontrol et
            CheckForEnemies();

            if (targetTower != null)
            {
                // Hedef kuleye doðru hareket etmeye devam et
                agent.isStopped = false;
                MoveToPos(targetTower.position);
            }
        }
    }


    public void MoveToPos(Vector3 pos)
    {
        // Verilen pozisyona hareket eder
        agent.SetDestination(pos);
        //agent.stoppingDistance = 1f; // Hedefe yaklaþýnca dur
    }

    private void CheckForEnemies()
    {
        // Algýlama alanýndaki düþmanlarý kontrol et
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // En yakýn düþmaný hedefle
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);
            SetEnemyTarget(closestEnemy);
        }
        else
        {
            // Düþman yoksa hedefi temizle
            ClearEnemyTarget();
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

    public void SetEnemyTarget(Transform enemy)
    {
        currentEnemyTarget = enemy;
    }

    public void ClearEnemyTarget()
    {
        currentEnemyTarget = null;
    }

    private void Attack()
    {
        // Saldýrý iþlemi burada tetiklenir
        Debug.Log($"{gameObject.name} is attacking {currentEnemyTarget.name}!");
    }

    private void OnDrawGizmosSelected()
    {
        // Algýlama alanýný görselleþtirmek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
