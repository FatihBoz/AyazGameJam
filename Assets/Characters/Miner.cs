using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    private bool isInMine = false;
    private float goldPerSecond = 1f; // Her saniye kazanýlacak altýn miktarý
    private float timeSpentInMine = 0f;
    private float detectionRadius = 10f; // Madencinin arayacaðý mesafe
    private Transform targetMine; // En yakýn "Mine" nesnesi

    Lord owner;

    GameObject ownerGameObject;

    bool hasSource;

    public string OwnerTagNameOfCastle;

    public float moveSpeed = 3f; // Madencinin hareket hýzý

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 1f; // Hedefe yaklaþýnca dur
    }

    private void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();

        ownerGameObject = GameObject.FindWithTag(OwnerTagNameOfCastle);

        owner.AddGold(goldPerSecond);
    }

    private void Update()
    {

        if (hasSource)
        {
            MoveToPos(ownerGameObject.transform.position);
        }
        else
        {
            FindClosestMine();
        }

        if (targetMine != null)
        {
            MoveToPos(targetMine.position);
            //print("Going To Mine");
        }
    }

    private void FindClosestMine()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius); // Çevredeki tüm nesneleri kontrol et
        targetMine = null; // En baþta null

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Mine"))
            {
                if (targetMine == null || Vector3.Distance(transform.position, hit.transform.position) < Vector3.Distance(transform.position, targetMine.position))
                {
                    targetMine = hit.transform; // En yakýn "Mine" nesnesini bul
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            isInMine = true;
            timeSpentInMine = 0f; // Yeniden baþlat
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            isInMine = false;
        }
    }

    // Gizmos ile mesafeyi çiz
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Dairenin rengini kýrmýzý yap
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Madencinin etrafýndaki mesafeyi daire olarak çiz
    }
}
