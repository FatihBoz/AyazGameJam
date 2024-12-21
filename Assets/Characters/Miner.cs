using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    private bool isInMine = false; // Madende mi?
    private float goldPerSecond = 1f; // Her saniye kazanýlan altýn
    private float mineTime = 3f; // Madende bekleme süresi
    float detectionRadius = 50f; // Maden arama mesafesi

    private Transform targetMine; // Hedef maden
    private bool hasGold = false; // Altýn toplandý mý?

    private Lord owner;
    private GameObject ownerGameObject;

    public string OwnerTagNameOfCastle;
    public float moveSpeed = 3f;

    private NavMeshAgent agent;
    private Renderer minerRenderer; // Görünmez yapmak için

    private MinerSoldier minerSoldier;


    AudioSource audioSource;

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

        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();
        ownerGameObject = GameObject.FindWithTag(OwnerTagNameOfCastle);
    }

    private void Update()
    {
        if (isInMine) return;



        if (hasGold)
        {
            MoveToPos(ownerGameObject.transform.position); // Altýn toplandýysa kuleye dön
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
            minerRenderer.enabled = false; // Görünmez yap

            StartCoroutine(MineGold());
        }
        if (other.CompareTag(OwnerTagNameOfCastle) && hasGold)
        {
            Owner.AddGold(goldPerSecond * mineTime); // Toplanan altýnlarý ekle
            hasGold = false; // Altýn býrakýldý
            print("Deployed GOld");
        }
    }

    private IEnumerator MineGold()
    {
        //audioSource.Play();
        //audioSource.Play();
        minerSoldier.MakeHealthBarActive(false);
        print("e girdi buraya");
        yield return new WaitForSeconds(mineTime); // Madende bekle
        minerSoldier.MakeHealthBarActive(true);
        isInMine = false;
        hasGold = true;
        minerRenderer.enabled = true; // Görünür yap
        agent.isStopped = false; // Hareketi devam ettir
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

    public void MakeHealthBarInactive()
    {
        throw new System.NotImplementedException();
    }

    public void MakeHealthBarActive()
    {
        throw new System.NotImplementedException();
    }
}
