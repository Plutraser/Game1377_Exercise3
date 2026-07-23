using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 5f;
    private Rigidbody2D rb;

    private float asteroidExplodeDuration = .22f;

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
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.IsDestroyed = true;
            asteroid.BreakAsteroid();
            asteroid.Explode();
            Destroy(collision.gameObject, asteroidExplodeDuration);
            Destroy(gameObject);
        }
    }
}
