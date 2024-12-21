using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class SoldierCombat : Soldier
{
    public Lord owner;
    public NavMeshAgent agent;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    GameObject targetTower;
    public LayerMask enemyLayer;
    private Transform currentEnemyTarget;
    private Vector3 currentEnemyLocation;

    public string TargetTagNameOfCastle;
    public string OwnerTagNameOfCastle;


    private float currentHp;
    private ISoldierAnimation anim;


    protected override void Awake()
    {
        base.Awake();
        currentHp = soldierSO.MaxHp;
        anim = GetComponent<ISoldierAnimation>();
    }

    void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();


        targetTower = GameObject.FindWithTag(TargetTagNameOfCastle);

        if (targetTower != null)
        {
            MoveToPos(targetTower.transform.position);
        }
        
    }

    void Update()
    {
        if (currentEnemyTarget != null)
        {
            // D��man sald�r� menzilinde ise sald�r
            if (soldierSO.AttackRange < Vector3.Distance(currentEnemyTarget.position, gameObject.transform.position))
            {
                Attack();
                agent.isStopped = true;

            }
            else
            {
                MoveToPos(currentEnemyTarget.transform.position);
                transform.LookAt(currentEnemyTarget); // D��mana bak
            }

        }
        else
        {
            // Yak�ndaki d��manlar� kontrol et
            CheckForEnemies();
            
            if (targetTower != null)
            {
                // Hedef kuleye do�ru hareket etmeye devam et
                agent.isStopped = false;
                MoveToPos(targetTower.transform.position);
            }
            
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

    public void ChangeMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
