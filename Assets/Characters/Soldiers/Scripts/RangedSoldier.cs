using UnityEngine;

public class RangedSoldier : MonoBehaviour
{
    public GameObject projectilePrefab; // F�rlat�lacak mermi prefab'�
    public Transform shootPoint; // Merminin f�rlat�laca�� nokta
    public float shootRate = 1f; // At�� h�z� (saniye ba��na at�� say�s�)
    private float nextShootTime = 0f; // Bir sonraki at�� zaman�

    public void Shoot(Transform target)
    {
        // Hedefin y�n�n� hesapla
        Vector3 direction = target.position - shootPoint.position;

        // F�rlat�lacak mermiyi olu�tur ve mermiyi hedefe do�ru y�nlendir
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

        // Bir sonraki at�� zaman�n� ayarla
        nextShootTime = Time.time + shootRate;
    }

}
