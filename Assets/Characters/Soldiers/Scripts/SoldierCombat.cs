using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SoldierCombat : Soldier
{
    [Header("*** Health Bar ***")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Vector3 healthBarOffset;
    [SerializeField] private float healthBarXRotationOffSet = -45f;
    private HealthBarUI healthBarInstance;

    [Header("*** VFX ***")]
    [SerializeField] private GameObject dyingEffect;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private float effectDestroyTime = 1f;
    [SerializeField] private Vector3 effectOffSet;

    [Header("*** OTHER ***")]

    public Lord owner;
    public NavMeshAgent agent;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;


    GameObject targetTower;
    public LayerMask enemyLayer;
    private Transform currentTarget;
    private Vector3 currentTargetLocation;

    public string TargetTagNameOfCastle;
    public string OwnerTagNameOfCastle;


    private float currentHp;
    private ISoldierAnimation anim;


    protected override void Awake()
    {
        base.Awake();
        InstantiateHealthBar();
        currentHp = soldierSO.MaxHp;
        anim = GetComponent<ISoldierAnimation>();
        
    }

    void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();


        targetTower = GameObject.FindWithTag(TargetTagNameOfCastle);

        if (targetTower != null)
        {
            print("target tower null de�il");
            MoveToPos(targetTower.transform.position);
        }

        
    }


    #region UI
    public void InstantiateHealthBar()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity).GetComponent<HealthBarUI>();

        healthBarInstance.relatedSoldier = this;
        GameObject soldierCanvas = GameObject.Find("SoldierCanvas");
        healthBarInstance.transform.SetParent(soldierCanvas.transform);
    }

    #endregion UI

    void Update()
    {
        if (healthBarInstance != null)
        {
            print("HP BAR INSTANCE NULL DE��L "+healthBarInstance.name);
            
        }
        CheckForEnemies();

        if (currentTarget != null)
        {
            // D��man sald�r� menzilinde ise sald�r
            if (soldierSO.Range >= Vector3.Distance(currentTarget.position, gameObject.transform.position))
            {
                anim.AttackAnimation();
                agent.isStopped = true;

            }
            else
            {
                MoveToPos(currentTarget.transform.position);
            }
        }
        else
        {   
            if (targetTower != null)
            {
                
                agent.isStopped = false;
                MoveToPos(targetTower.transform.position);
                SetEnemyTarget(targetTower.transform);
            }
            
        }

        transform.LookAt(currentTarget);
        print("current target:"+currentTarget.name);

        healthBarInstance.gameObject.transform.SetPositionAndRotation(transform.position + healthBarOffset, Quaternion.Euler(healthBarXRotationOffSet, 0, 0));
    }


    public void TakeDamage(float damage)
    {
        if (currentHp <= 0)
            return;


        currentHp -= damage;
        healthBarInstance.TakeDamage(currentHp);
        Destroy(Instantiate(bloodEffect, transform.position + effectOffSet, bloodEffect.transform.rotation), effectDestroyTime);

        if (currentHp <= 0)
        {
            StartCoroutine(DieAndDelayedRevive(effectDestroyTime));
        }
    }

    private IEnumerator DieAndDelayedRevive(float delayTime)
    {
        anim.DieAnimation();
        Destroy(healthBarInstance);
        GameObject obj = Instantiate(dyingEffect, transform.position + effectOffSet, dyingEffect.transform.rotation);

        yield return new WaitForSeconds(delayTime);
        
        Destroy(obj);
        if (canRevive && soldierToTransform != null)
        {
            Soldier soldier = Instantiate(soldierToTransform, transform.position, transform.rotation);
            soldier.SetCanRevive(false);
        }

        Destroy(gameObject);
    }


    public void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        //agent.stoppingDistance = 1f; // Hedefe yakla��nca dur
    }

    private Transform CheckForEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, soldierSO.DetectionRange, enemyLayer);

        Transform closestTarget =  null;

        if (enemiesInRange.Length > 0)
        {
            // En yak�n d��man� hedefle
            closestTarget = FindClosestEnemy(enemiesInRange);
            SetEnemyTarget(closestTarget);
            agent.SetDestination(currentTargetLocation);
        }

        return closestTarget;

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
        currentTarget = enemy;
    }

    public void ClearEnemyTarget()
    {
        currentTarget = null;
    }

    private void Attack() // with animation event
    {
        if (currentTarget.TryGetComponent<SoldierCombat>(out var enemy))
        {
            enemy.TakeDamage(soldierSO.AttackDamage);
        }
    }

    public void ChangeMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, soldierSO.Range);
    }

}
