using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent; // Karakterin hareket sistemi
    public Transform targetTower; // Hedef kule
    public float attackRange = 5f; // D��mana sald�r� mesafesi
    public LayerMask enemyLayer; // D��manlar�n layer'�
    public float detectionRadius = 10f; // D��man alg�lama yar��ap�

    private Transform currentEnemyTarget; // Alg�lanan d��man

    void Start()
    {
        if (targetTower != null)
        {
            // Kuleye do�ru hareket ba�lat
            MoveToPos(targetTower.position);
        }
    }

    void Update()
    {
        if (currentEnemyTarget != null)
        {
            // D��mana sald�r
            agent.isStopped = true;
            transform.LookAt(currentEnemyTarget); // D��mana bak
            Attack();
        }
        else
        {
            // Yak�ndaki d��manlar� kontrol et
            CheckForEnemies();

            if (targetTower != null)
            {
                // Hedef kuleye do�ru hareket etmeye devam et
                agent.isStopped = false;
                MoveToPos(targetTower.position);
            }
        }
    }


    public void MoveToPos(Vector3 pos)
    {
        // Verilen pozisyona hareket eder
        agent.SetDestination(pos);
        //agent.stoppingDistance = 1f; // Hedefe yakla��nca dur
    }

    private void CheckForEnemies()
    {
        // Alg�lama alan�ndaki d��manlar� kontrol et
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // En yak�n d��man� hedefle
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);
            SetEnemyTarget(closestEnemy);
        }
        else
        {
            // D��man yoksa hedefi temizle
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
        // Sald�r� i�lemi burada tetiklenir
        Debug.Log($"{gameObject.name} is attacking {currentEnemyTarget.name}!");
    }

    private void OnDrawGizmosSelected()
    {
        // Alg�lama alan�n� g�rselle�tirmek i�in
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
