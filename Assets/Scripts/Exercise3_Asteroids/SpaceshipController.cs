using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

public class SpaceshipController : MonoBehaviour
{
    /// <summary>
    /// Organize all of these later with headers and shit
    /// </summary>
    public Animator Animation;
    private AudioSource audioSource;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxThrust = 11f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] private float fireRate = .35f;
    public bool IsDead = false;
    private bool isThrusting = false;
    private bool firedBullet = false;
    public int Lives = 3;
    private float spaceshipDeathAnimationDuration = .4f;
    private int safeDistance = 3;
    public List<GameObject> asteroids = new List<GameObject>();
    private float rotationInput;
    private float thrustInput;
    private float nextFireTime;
    private bool fasterThrust = false;
    private bool fasterRotation = false;
    private bool fatterBullets = false;
    private float rotationMultiplier = 2f;
    private float thrustMultiplier = 2f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        Animation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsDead)
        {
            Animation.SetBool("IsDead", true);
        }
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleHyperspace();
        FireBullet();
    }

    void LateUpdate()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        moveDirection = (thrustInput * transform.up);
        HandleThrust();
    }

    /// <summary>
    /// Handles the rotation of ship using A and D
    /// </summary>
    private void HandleRotation()
    {
        if (fasterRotation)
        {
            transform.Rotate(Vector3.back * rotationInput * rotationSpeed * rotationMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.deltaTime);
        }
        
    }

    /// <summary>
    /// Handles the movement of the ship, if pressing forward the ship will move forward. Does not allow movement backward
    /// </summary>
    private void HandleThrust()
    {
        if (fasterThrust) 
        {
            if (thrustInput > 0)
            {
                audioSource.Play();
                rb.AddForce(moveDirection * thrustForce * thrustMultiplier, ForceMode2D.Force);
                Animation.SetBool("isThrusting", true);
            }
        }
        else
        {
            if (thrustInput > 0)
            {
                audioSource.Play();
                rb.AddForce(moveDirection * thrustForce, ForceMode2D.Force);
                Animation.SetBool("isThrusting", true);
            }
        }

        if (rb.linearVelocity.magnitude > maxThrust)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxThrust;
        }
        if (thrustInput <= 0)
        {
            audioSource.Stop();
            Animation.SetBool("isThrusting", false);
        }
    }

    /// <summary>
    /// Fires the bullet when space is pressed, assigns a fire rate in which the player cannot exceed.
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Animation.Play("FireBullet");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (fatterBullets)
            {
                float increasedSize = 1f;
                bullet.transform.localScale = new Vector3(increasedSize, increasedSize, 0);
            }
        }
        
    }

    /// <summary>
    /// Checks for teleport button press, button is "Left Shift"
    /// </summary>
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Teleport"))
        {
            TeleportToRandomLocation();
        }
    }

    /// <summary>
    /// Teleports to the ship in a random location within the screenbounds
    /// </summary>
    private void TeleportToRandomLocation()
    {
        Vector3 randomLocation = new Vector3(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop), 0);
        
        for (int i = 0; i < asteroids.Count; i++)
        {
            Vector3 fromRandomLocationToAsteroid = asteroids[i].transform.position - randomLocation;
            while (fromRandomLocationToAsteroid.magnitude > safeDistance)
            {
                randomLocation = new Vector3(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop), 0);
            }
        }
        transform.position = randomLocation;
    }

    public void Respawn()
    {
        float origin = 0;
        float timer = 2f;
        transform.position = new Vector3(origin, origin, origin);
        StartCoroutine(InvincibilityFrames(timer));
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        Animation.Play("Invincibility");
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(duration);

        GetComponent<Collider2D>().enabled = true;
    }

    public void AttackedByAsteroid()
    {
        Lives -= 1;
        IsDead = true;
        if (Lives > 0)
        {
            IsDead = false;
            Respawn();
        }
        else
        {
            Destroy(gameObject, spaceshipDeathAnimationDuration);
        }
    }

    public void CollectedLifePowerup()
    {
        Lives += 1;
    }
    public void CollectedMovementPowerup()
    {
        float timer = 5f;
        StartCoroutine(IncreasedMovement(timer));
    }
    private IEnumerator IncreasedMovement(float duration)
    {
        fasterRotation = true;
        fasterThrust = true;

        yield return new WaitForSeconds(duration);

        fasterRotation = false;
        fasterThrust = false;
    }
    public void CollectedBulletPowerup()
    {
        float timer = 5f;
        StartCoroutine(IncreasedBulletSize(timer));
    }
    private IEnumerator IncreasedBulletSize(float duration)
    {
        fatterBullets = true;

        yield return new WaitForSeconds(duration);

        fatterBullets = false;
    }
}
