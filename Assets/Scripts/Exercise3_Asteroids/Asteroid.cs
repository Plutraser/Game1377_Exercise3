using UnityEngine;
using UnityEngine.EventSystems;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    public ObjectSpawner AsteroidSpawnerScript;
    public Animator AsteroidExplosion;
    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private float rotateSpeed;
    private Rigidbody2D rb;
    private ObjectSpawner spawner;
    private int babyAsteroidCount = 2;
    public bool IsDestroyed = false;

    void Start()
    {
        AsteroidExplosion = GetComponent<Animator>();
        rotateSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0).normalized * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (IsDestroyed)
        {
            AsteroidExplosion.SetBool("IsDestroyed", true);
        }
        transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// The asteroid that is destroyed goes down a size to spawn its children.
    /// </summary>
    public void BreakAsteroid()
    {
        if (size > 0)
        {
            SpawnChildren(size - 1);
        }
    }

    /// <summary>
    /// Spawns 2 children from the parent asteroid
    /// </summary>
    /// <param name="childSize"></param>
    private void SpawnChildren(AsteroidSize childSize)
    {
        for (int i = 0; i < babyAsteroidCount; i++)
        {
            AsteroidSpawnerScript.SpawnAsteroid(transform.position, childSize);
        }
    }
    /// <summary>
    /// Asteroid destroys Player ship when it collides
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpaceshipController spaceship = collision.gameObject.GetComponent<SpaceshipController>();
            spaceship.AttackedByAsteroid();
        }
    }
}
