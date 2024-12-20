using UnityEngine;
using UnityEngine.AI;

public class SoldierCombat : Soldier
{
    public NavMeshAgent agent;
    //public GameObject targetTower;
    public LayerMask enemyLayer;
    private Transform currentEnemyTarget;
    private Vector3 currentEnemyLocation;


    private float currentHp;
    private ISoldierAnimation anim;


    private void Awake()
    {
        currentHp = soldierSO.MaxHp;
        anim = GetComponent<ISoldierAnimation>();   
    }

    void Start()
    {
        /*
        if (targetTower != null)
        {
            MoveToPos(targetTower.transform.position);
        }
        */
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
            /*
            if (targetTower != null)
            {
                // Hedef kuleye do�ru hareket etmeye devam et
                agent.isStopped = false;
                MoveToPos(targetTower.transform.position);
            }
            */
        }
    }


    public void TakeDamage(float damage)
    {
        anim.HitReactAnimation();
        currentHp -= damage;
        print("current hp:"+currentHp);
        if (currentHp <= 0 && canRevive)
        {
            anim.DieAnimation();
            print("�ld�");
            //Instantiate Revive effect
            if (soldierToTransform.SoldierPrefab != null)
            {
                Soldier soldier = Instantiate(soldierToTransform.SoldierPrefab, transform.position, transform.rotation);
                soldier.SetCanRevive(false);
            }

            
            Destroy(gameObject);
        }
    }


    public void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        //agent.stoppingDistance = 1f; // Hedefe yakla��nca dur
    }

    private void CheckForEnemies()
    {
        // Alg�lama alan�ndaki d��manlar� kontrol et
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, soldierSO.DetectionRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            // En yak�n d��man� hedefle
            Transform closestEnemy = FindClosestEnemy(enemiesInRange);
            SetEnemyTarget(closestEnemy);
            agent.SetDestination(currentEnemyLocation);
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
        anim.AttackAnimation();

        Debug.Log($"{gameObject.name} is attacking {currentEnemyTarget.name}!");
    }
}
