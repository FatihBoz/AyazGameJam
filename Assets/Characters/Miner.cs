using UnityEngine;

public class Miner : MonoBehaviour
{
    private bool isInMine = false;
    private float goldPerSecond = 1f; // Her saniye kazanýlacak altýn miktarý
    private float timeSpentInMine = 0f;
    private float detectionRadius = 10f; // Madencinin arayacaðý mesafe
    private Transform targetMine; // En yakýn "Mine" nesnesi

    Lord owner;

    public string OwnerTagNameOfCastle;

    public float moveSpeed = 3f; // Madencinin hareket hýzý

    private void Start()
    {
        owner = GameObject.FindWithTag(OwnerTagNameOfCastle).GetComponent<Lord>();

    }

    private void Update()
    {
        if (isInMine)
        {
            // Madencinin içinde bulunduðu madene baðlý olarak altýn kazan
            timeSpentInMine += Time.deltaTime;
            if (timeSpentInMine >= 1f) // 1 saniye geçtiðinde
            {
                timeSpentInMine = 0f;
                if (owner != null)
                {
                    owner.AddGold(goldPerSecond); // Sahibi altýn kazandýr
                }
            }
        }
        else
        {
            // Madenci en yakýn "Mine" nesnesini bulmaya çalýþýyor
            FindClosestMine();

            if (targetMine != null)
            {
                // En yakýn "Mine" nesnesine doðru hareket et
                MoveTowardsMine();
            }
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

    private void MoveTowardsMine()
    {
        if (targetMine != null)
        {
            GetComponent<MineMovement>().MoveToMine(targetMine.position);
            print("Aloo bu maden nerde?");
            //Vector3 direction = (targetMine.position - transform.position).normalized; // "Mine" nesnesine doðru yön
            //transform.position = Vector3.MoveTowards(transform.position, targetMine.position, moveSpeed * Time.deltaTime); // Madenci hareket et
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
