using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Merminin vereceði hasar miktarý
    float speed = 25;  // Merminin hýzý

    Rigidbody rb;

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
            print("Enemyyyy");
            //
        }
        Destroy(gameObject);


    }
}
