using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Merminin verece�i hasar miktar�
    float speed = 30f;  // Merminin h�z�

    Rigidbody rb;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // E�er mermi allyChar tag'ine sahip bir karaktere �arparsa
        if (collision.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            //
        }
        Destroy(gameObject);


    }
}
