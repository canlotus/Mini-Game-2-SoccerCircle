using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float bounceDamping = 0.85f; 
    public float wallPushForce = 0.2f; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) 
        {
            Vector2 normal = collision.contacts[0].normal;

            rb.velocity += normal * wallPushForce;

            rb.velocity *= bounceDamping;
        }
    }
}