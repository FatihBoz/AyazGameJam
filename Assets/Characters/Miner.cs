using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour
{
    private bool isInMine = false;
    private float goldPerSecond = 1f; // Her saniye kazan�lacak alt�n miktar�
    private float timeSpentInMine = 0f;
    private float detectionRadius = 10f; // Madencinin arayaca�� mesafe
    private Transform targetMine; // En yak�n "Mine" nesnesi

    Lord owner;

    GameObject ownerGameObject;

    bool hasSource;

    public string OwnerTagNameOfCastle;

    public float moveSpeed = 3f; // Madencinin hareket h�z�

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.stoppingDistance = 1f; // Hedefe yakla��nca dur
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
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius); // �evredeki t�m nesneleri kontrol et
        targetMine = null; // En ba�ta null

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Mine"))
            {
                if (targetMine == null || Vector3.Distance(transform.position, hit.transform.position) < Vector3.Distance(transform.position, targetMine.position))
                {
                    targetMine = hit.transform; // En yak�n "Mine" nesnesini bul
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            isInMine = true;
            timeSpentInMine = 0f; // Yeniden ba�lat
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            isInMine = false;
        }
    }

    // Gizmos ile mesafeyi �iz
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Dairenin rengini k�rm�z� yap
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Madencinin etraf�ndaki mesafeyi daire olarak �iz
    }
}
