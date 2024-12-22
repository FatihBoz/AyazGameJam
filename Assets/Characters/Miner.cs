using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    [Header("** Mine **")]
    private bool isInMine = false; // Madende mi?
    private float goldPerSecond = 1f; // Her saniye kazan�lan alt�n
    private float mineTime = 3f; // Madende bekleme s�resi
    float detectionRadius = 50f; // Maden arama mesafesi
    float goldamount = 30;
    
    private Transform targetMine; // Hedef maden
    private bool hasGold = false; // Alt�n topland� m�?


    [Header("** Other **")]
    private Lord owner;
    private GameObject ownerGameObject;

    public string OwnerTagNameOfCastle;
    public float moveSpeed = 3f;

    private NavMeshAgent agent;
    private Renderer minerRenderer; // G�r�nmez yapmak i�in

    private MinerSoldier minerSoldier;

    AudioSource audioSource;

    [SerializeField] private Vector3 farTeleportPointPosition;
    private static GameObject farTeleportPoint;
    private Vector3 previousPos;

    public Lord Owner { get => owner;}

    private void Awake()
    {
        minerSoldier = GetComponent<MinerSoldier>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        minerRenderer = GetComponentInChildren<Renderer>();
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
            MoveToPos(ownerGameObject.transform.position); // Alt�n topland�ysa kuleye d�n
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
            isInMine = true;
            agent.isStopped = true; // Hareketi durdur
            agent.enabled = false;

            StartCoroutine(MineGold());
        }
        if (other.CompareTag(OwnerTagNameOfCastle) && hasGold)
        {
            Owner.AddGold(goldamount); // Toplanan alt�nlar� ekle
            UIUpdater.instance.UpdateSource();

            hasGold = false; // Alt�n b�rak�ld�
            print("Deployed GOld");
        }
    }

    private IEnumerator MineGold()
    {
        //audioSource.Play();
        //audioSource.Play();
        //minerSoldier.MakeHealthBarActive(false);
        previousPos = transform.position;
        transform.position = farTeleportPoint.transform.position;
        yield return new WaitForSeconds(mineTime); // Madende bekle
        ExitFromMine();


    }

    void ExitFromMine()
    {
        transform.position = previousPos;
        //minerSoldier.MakeHealthBarActive(true);
        agent.enabled = true;
        isInMine = false;
        hasGold = true;
        
        agent.isStopped = false;
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
