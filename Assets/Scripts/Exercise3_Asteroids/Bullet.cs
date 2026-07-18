using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = transform.up * bulletSpeed;
        Destroy(gameObject, bulletLifetime);
    }

    /// <summary>
    /// Bullet calls break asteroid method, destroys asteroid and destroys itself when it collides with an asteroid.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            collision.gameObject.GetComponent<Asteroid>().BreakAsteroid();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
