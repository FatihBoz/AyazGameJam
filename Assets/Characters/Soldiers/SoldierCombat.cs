using UnityEngine;
using UnityEngine.AI;


public class SoldierCombat : Soldier
{
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
        currentHp = soldierSO.MaxHp;
        anim = GetComponent<ISoldierAnimation>();
    }

    void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();


        targetTower = GameObject.FindWithTag(TargetTagNameOfCastle);

        if (targetTower != null)
        {
            print("target tower null deðil");
            MoveToPos(targetTower.transform.position);
        }
        
    }

    void Update()
    {
        CheckForEnemies();

        if (currentTarget != null)
        {
            // Düþman saldýrý menzilinde ise saldýr
            if (soldierSO.Range >= Vector3.Distance(currentTarget.position, gameObject.transform.position))
            {
                print("attack anim");
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
            
            if (CheckForEnemies() != null)
            {
                return;
            }

            ClearEnemyTarget();
            
            if (targetTower != null)
            {
                
                agent.isStopped = false;
                MoveToPos(targetTower.transform.position);
                SetEnemyTarget(targetTower.transform);
            }
            
        }

        transform.LookAt(currentTarget);
        print("current target:"+currentTarget.name);
    }


    public void TakeDamage(float damage)
    {
        anim.HitReactAnimation();
        currentHp -= damage;
        print("current hp:"+currentHp);
        if (currentHp <= 0 && canRevive)
        {
            anim.DieAnimation();
            print("öldü");
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
        //agent.stoppingDistance = 1f; // Hedefe yaklaþýnca dur
    }

    private Transform CheckForEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, soldierSO.DetectionRange, enemyLayer);

        Transform closestTarget =  null;

        if (enemiesInRange.Length > 0)
        {
            // En yakýn düþmaný hedefle
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

    private void Attack()
    {

        print("yapýþtýrdý lavuða:"+currentTarget.name);
       
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
