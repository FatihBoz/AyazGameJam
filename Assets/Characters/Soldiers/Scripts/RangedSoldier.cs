using UnityEngine;

public class RangedSoldier : MonoBehaviour
{
    public GameObject projectilePrefab; // Fýrlatýlacak mermi prefab'ý
    public Transform shootPoint; // Merminin fýrlatýlacaðý nokta
    public float shootRate = 1f; // Atýþ hýzý (saniye baþýna atýþ sayýsý)
    private float nextShootTime = 0f; // Bir sonraki atýþ zamaný

    public void Shoot(Transform target)
    {
        // Hedefin yönünü hesapla
        Vector3 direction = target.position - shootPoint.position;

        // Fýrlatýlacak mermiyi oluþtur ve mermiyi hedefe doðru yönlendir
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

        // Bir sonraki atýþ zamanýný ayarla
        nextShootTime = Time.time + shootRate;
    }

}
