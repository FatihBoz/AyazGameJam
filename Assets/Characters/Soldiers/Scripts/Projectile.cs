using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Merminin verece�i hasar miktar�
    public float speed = 30;  // Merminin h�z�

    Rigidbody rb;

    public string enemyCastleLayer;
    public string enemyLayer;
    
    void Start()
    {
        Destroy(gameObject, 5f);
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * speed; // Y�n� merminin "forward" y�n�yle hizala
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        // E�er mermi allyChar tag'ine sahip bir karaktere �arparsa
        if (collision.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {

            if (collision.gameObject.transform.TryGetComponent<ICombat>(out var enemy))
            {
                enemy.TakeDamage(8);
            }
            

        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("trigger");
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
