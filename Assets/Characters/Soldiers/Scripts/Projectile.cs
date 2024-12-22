using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Merminin vereceði hasar miktarý
    float speed = 20;  // Merminin hýzý

    Rigidbody rb;

    public string enemyCastleLayer;
    public string enemyLayer;
    
    void Start()
    {
        Destroy(gameObject, 5f);
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * speed; // Yönü merminin "forward" yönüyle hizala
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hop mermi deðdi");
        // Eðer mermi allyChar tag'ine sahip bir karaktere çarparsa
        if (collision.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {

            if (collision.gameObject.transform.TryGetComponent<ICombat>(out var enemy))
            {
                enemy.TakeDamage(8);
            }
            Destroy(gameObject);

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(enemyCastleLayer))
        {

            if (other.gameObject.transform.TryGetComponent<ICombat>(out var enemy))
            {
                enemy.TakeDamage(8);
            }
            Destroy(gameObject);

        }
    }
}
