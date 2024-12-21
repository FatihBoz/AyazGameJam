using UnityEngine;

public class Miner : MonoBehaviour
{
    private bool isInMine = false;
    private float goldPerSecond = 1f; // Her saniye kazan�lacak alt�n miktar�
    private float timeSpentInMine = 0f;
    private float detectionRadius = 10f; // Madencinin arayaca�� mesafe
    private Transform targetMine; // En yak�n "Mine" nesnesi

    Lord owner;

    public string OwnerTagNameOfCastle;

    public float moveSpeed = 3f; // Madencinin hareket h�z�

    private void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();

    }

    private void Update()
    {
        if (isInMine)
        {
            // Madencinin i�inde bulundu�u madene ba�l� olarak alt�n kazan
            timeSpentInMine += Time.deltaTime;
            if (timeSpentInMine >= 1f) // 1 saniye ge�ti�inde
            {
                timeSpentInMine = 0f;
                if (owner != null)
                {
                    owner.AddGold(goldPerSecond); // Sahibi alt�n kazand�r
                }
            }
        }
        else
        {
            // Madenci en yak�n "Mine" nesnesini bulmaya �al���yor
            FindClosestMine();

            if (targetMine != null)
            {
                // En yak�n "Mine" nesnesine do�ru hareket et
                MoveTowardsMine();
            }
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

    private void MoveTowardsMine()
    {
        if (targetMine != null)
        {
            GetComponent<MineMovement>().MoveToMine(targetMine.position);
            print("Aloo bu maden nerde?");
            //Vector3 direction = (targetMine.position - transform.position).normalized; // "Mine" nesnesine do�ru y�n
            //transform.position = Vector3.MoveTowards(transform.position, targetMine.position, moveSpeed * Time.deltaTime); // Madenci hareket et
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
