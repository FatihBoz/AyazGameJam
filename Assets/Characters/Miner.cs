using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    private bool isInMine = false; // Madende mi?
    private float goldPerSecond = 1f; // Her saniye kazan�lan alt�n
    private float mineTime = 3f; // Madende bekleme s�resi
    private float detectionRadius = 10f; // Maden arama mesafesi

    private Transform targetMine; // Hedef maden
    private bool hasGold = false; // Alt�n topland� m�?

    private Lord owner;
    private GameObject ownerGameObject;

    public string OwnerTagNameOfCastle;
    public float moveSpeed = 3f;

    private NavMeshAgent agent;
    private Renderer minerRenderer; // G�r�nmez yapmak i�in

    private void Awake()
    {
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
            MoveToPos(ownerGameObject.transform.position); // Alt�n topland�ysa kuleye d�n
        }
        else
        {
            FindClosestMine();

            if (targetMine != null)
            {
                MoveToPos(targetMine.position); // En yak�n madene git
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
            minerRenderer.enabled = false; // G�r�nmez yap

            StartCoroutine(MineGold(other.GetComponent<Mine>()));
        }
        if (other.CompareTag(OwnerTagNameOfCastle) && hasGold)
        {
            owner.AddGold(goldPerSecond * mineTime); // Toplanan alt�nlar� ekle
            hasGold = false; // Alt�n b�rak�ld�
            print("Deployed GOld");
        }
    }

    private IEnumerator MineGold(Mine mine)
    {
        yield return new WaitForSeconds(mineTime); // Madende bekle
        isInMine = false;
        hasGold = true;
        minerRenderer.enabled = true; // G�r�n�r yap
        agent.isStopped = false; // Hareketi devam ettir
    }

    private void MoveToPos(Vector3 pos)
    {
        print(pos);
        agent.SetDestination(pos);
        agent.stoppingDistance = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
