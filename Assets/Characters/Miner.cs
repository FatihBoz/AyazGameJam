using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    [Header("** Mine **")]
    private bool isInMine = false; // Madende mi?
    private float goldPerSecond = 1f; // Her saniye kazanýlan altýn
    private float mineTime = 3f; // Madende bekleme süresi
    float detectionRadius = 50f; // Maden arama mesafesi
    float goldamount = 30;
    
    private Transform targetMine; // Hedef maden
    private bool hasGold = false; // Altýn toplandý mý?


    [Header("** Other **")]
    private Lord owner;
    private GameObject ownerGameObject;

    public string OwnerTagNameOfCastle;
    public float moveSpeed = 3f;

    private NavMeshAgent agent;

    private MinerSoldier minerSoldier;

    AudioSource audioSource;

    public Vector3 SpawnPoint;

    [SerializeField] private Vector3 farTeleportPointPosition;
    private static GameObject farTeleportPoint;
    private Vector3 previousPos;

    public Lord Owner { get => owner;}

    private void Awake()
    {
        minerSoldier = GetComponent<MinerSoldier>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (farTeleportPoint == null)
        {
            farTeleportPoint = new GameObject("FarTeleportPoint");
            farTeleportPoint.transform.position = farTeleportPointPosition;
        }


        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();
        ownerGameObject = GameObject.FindWithTag(OwnerTagNameOfCastle);
    }

    private void Update()
    {
        if (isInMine) return;



        if (hasGold)
        {
            MoveToPos(SpawnPoint); // Altýn toplandýysa kuleye dön
        }
        else
        {
            FindClosestMine();

            if (targetMine != null && agent.isActiveAndEnabled)
            {
                MoveToPos(targetMine.position);
            }
        }
    }

    private void FindClosestMine()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Mine"))
            {
                if (targetMine == null || Vector3.Distance(transform.position, hit.transform.position) < Vector3.Distance(transform.position, targetMine.position))
                {
                    targetMine = hit.transform;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine") && !isInMine)
        {
            print("tekrar mý giriyon buraya");
            isInMine = true;
            agent.enabled = false;

            StartCoroutine(MineGold());
        }
        if (other.CompareTag("MinerPoint") && hasGold)
        {
            print("Miner Reached the Point of retur");
            Owner.AddGold(goldamount); // Toplanan altýnlarý ekle
            UIUpdater.instance.UpdateSource();

            hasGold = false; // Altýn býrakýldý
            print("Deployed GOld");
        }
    }

    private IEnumerator MineGold()
    {
        //audioSource.Play();
        //audioSource.Play();
        previousPos = transform.position;
        transform.position = farTeleportPoint.transform.position;
        yield return new WaitForSeconds(mineTime);
        transform.position = previousPos;
        agent.enabled = true;
        hasGold = true;

        yield return new WaitForSeconds(.1f);
        isInMine = false;

    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
