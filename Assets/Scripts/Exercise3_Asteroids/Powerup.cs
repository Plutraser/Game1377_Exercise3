using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpaceshipController spaceship = collision.gameObject.GetComponent<SpaceshipController>();
            if (gameObject.CompareTag("LifePowerup"))
            {
                spaceship.CollectedLifePowerup();
                Destroy(gameObject);
            }         
            if (gameObject.CompareTag("MovementPowerup"))
            {
                spaceship.CollectedMovementPowerup();
                Destroy(gameObject);
            }
            if (gameObject.CompareTag("BulletPowerup"))
            {
                spaceship.CollectedBulletPowerup();
                Destroy(gameObject);
            }
        }
    }
}
