using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SoldierCombat : Soldier , ICombat
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
    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        InstantiateHealthBar();
        currentHp = soldierSO.MaxHp;
        anim = GetComponent<ISoldierAnimation>();

        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();


        targetTower = GameObject.FindWithTag(TargetTagNameOfCastle);

        if (targetTower != null)
        {
            MoveToPos(targetTower.transform.position);
        }


        //audioSource = GetComponent<AudioSource>();

        //audioSource.clip = soldierSO.attackAudioClip;

    }



    #region UI
    public void InstantiateHealthBar()
    {
        healthBarInstance = Instantiate(healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity).GetComponent<HealthBarUI>();

        healthBarInstance.relatedBeing = this;
        GameObject soldierCanvas = GameObject.Find("SoldierCanvas");
        healthBarInstance.transform.SetParent(soldierCanvas.transform);
    }

    #endregion UI

    void Update()
    {
        UpdateHealthBarPosition();

        if (currentTarget == null)
        {
            CheckForEnemies();
        }


        if (currentTarget != null)
        {
            RotateToTarget();
            // Düþman saldýrý menzilinde ise saldýr
            if (soldierSO.Range >= Vector3.Distance(currentTarget.position, gameObject.transform.position))
            {
                anim.AttackAnimation();
                if (agent.isActiveAndEnabled)
                {
                    agent.isStopped = true;
                }


            }
            else
            {
                if (agent.isActiveAndEnabled)
                {

                    print("targeta giriyor :"+currentTarget.name);
                    MoveToPos(currentTarget.transform.position);
                }

            }
        }
        else
        {   
            if (targetTower != null)
            {
                if (agent.isActiveAndEnabled)
                {
                    print("yürüme yönü");
                    agent.isStopped = false;
                    RotateToMoveDirection();
                    MoveToPos(targetTower.transform.position);
                }
            }
            
        }

        CheckIfTargetIsInRange();
    }


    void UpdateHealthBarPosition()
    {
        healthBarInstance.gameObject.transform.SetPositionAndRotation(transform.position + healthBarOffset, Quaternion.Euler(healthBarXRotationOffSet, 0, 0));
    }

    void RotateToMoveDirection()
    {
        Vector3 moveDir = agent.velocity.normalized;


        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
                Time.deltaTime * 5f
            );
        }
    }

    void RotateToTarget()
    {
        transform.LookAt(currentTarget);

        transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
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
        //agent.stoppingDistance = 1f; // Hedefe yaklaþýnca dur
    }

    private Transform CheckForEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, soldierSO.DetectionRange, enemyLayer);

        Transform closestTarget =  null;

        if (enemiesInRange.Length > 0)
        {
            closestTarget = FindClosestEnemy(enemiesInRange);
            SetEnemyTarget(closestTarget);
            if (agent.isActiveAndEnabled)
            {
                agent.SetDestination(currentTargetLocation);
            }
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

    void CheckIfTargetIsInRange()
    {
        if (currentTarget == null)
            return;

        if (Vector3.Distance(currentTarget.position, transform.position) > soldierSO.Range)
        {
            currentTarget = null;
        }
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
        if (currentTarget == null)
            return;

        if (soldierSO.AttackDamage == 8)
        {
            if (gameObject.TryGetComponent<RangedSoldier>(out var RangedSoldier))
            {
                RangedSoldier.Shoot(currentTarget);
            }
        }

        else if (currentTarget.TryGetComponent<ICombat>(out var enemy))
        {
            enemy.TakeDamage(soldierSO.AttackDamage);
        }




        
        if(audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void SetNextSoldierToTransform(Soldier soldier)
    {
        soldierToTransform = soldier;
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

    private void OnDestroy()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

    }

}
