using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f; // Merminin verece�i hasar miktar�
    float speed = 25;  // Merminin h�z�

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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hop mermi de�di");
        // E�er mermi allyChar tag'ine sahip bir karaktere �arparsa
        if (collision.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            print("Enemyyyy");
            //
        }
        Destroy(gameObject);


    }
}
