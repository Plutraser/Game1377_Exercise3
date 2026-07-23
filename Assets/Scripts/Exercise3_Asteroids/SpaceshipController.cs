using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SpaceshipController : MonoBehaviour
{
    /// <summary>
    /// Organize all of these later with headers and shit
    /// </summary>
    public Animator Animation;
    private AudioSource bulletAudioSource;
    private AudioSource thrustAudioSource;
    [SerializeField] private AudioClip bulletsFiredSoundClip;
    [SerializeField] private AudioClip spaceshipExplodedSoundClip;
    [SerializeField] private AudioClip hyperspaceSoundClip;
    [SerializeField] private AudioClip spaceshipThrustSoundClip;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxThrust = 11f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] private float fireRate = .35f;
    public bool IsDead = false;
    public int Lives = 3;
    private float spaceshipDeathAnimationDuration = .5f;
    private int safeDistance = 3;
    //
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
        bulletAudioSource = GetComponent<AudioSource>();
        thrustAudioSource = GetComponent<AudioSource>();

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
                float fasterPitch = 1.5f;
                if (!thrustAudioSource.isPlaying)
                {
                    thrustAudioSource.clip = spaceshipThrustSoundClip;
                    bulletAudioSource.pitch = fasterPitch;
                    thrustAudioSource.PlayOneShot(spaceshipThrustSoundClip);
                }
                rb.AddForce(moveDirection * thrustForce * thrustMultiplier, ForceMode2D.Force);
                Animation.SetBool("isThrusting", true);
            }
        }
        else
        {
            if (thrustInput > 0)
            {
                float regularPitch = 1f;
                if (!thrustAudioSource.isPlaying)
                {
                    thrustAudioSource.clip = spaceshipThrustSoundClip;
                    bulletAudioSource.pitch = regularPitch;
                    thrustAudioSource.PlayOneShot(spaceshipThrustSoundClip);
                }
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
            float regularPitch = 1f;
            bulletAudioSource.clip = bulletsFiredSoundClip;
            nextFireTime = Time.time + fireRate;
            Animation.Play("FireBullet");
            bulletAudioSource.pitch = regularPitch;
            bulletAudioSource.PlayOneShot(bulletsFiredSoundClip);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (fatterBullets)
            {
                float increasedSize = 1f;
                float lowerPitch = .5f;
                bulletAudioSource.pitch = lowerPitch;
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
            Animation.SetBool("usedHyperspace", true);
            TeleportToRandomLocation();
        }
    }

    /// <summary>
    /// Teleports to the ship in a random location within the screenbounds
    /// </summary>
    private void TeleportToRandomLocation()
    {
        Vector3 randomLocation = new Vector3(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop), 0);
        for (int i = 0; i < GetComponent<ObjectSpawner>().asteroids.Count; i++)
        {
            Vector3 fromRandomLocationToAsteroid = GetComponent<ObjectSpawner>().asteroids[i].transform.position - randomLocation;
            while (fromRandomLocationToAsteroid.magnitude > safeDistance)
            {
                randomLocation = new Vector3(Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight), Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop), 0);
            }
        }
        float animationDuration = .2f;
        StartCoroutine(TeleportDelay(animationDuration, randomLocation));
    }

    private IEnumerator TeleportDelay(float duration, Vector3 location)
    {
        float volume = 1f;
        float pitch = 1f;
        SoundFXManager.instance.PlaySoundFXClip(hyperspaceSoundClip, transform, volume, pitch);
        Animation.Play("Hyperspace");

        yield return new WaitForSeconds(duration);

        transform.position = location;
    }

    public void Respawn()
    {
        float origin = 0;
        float timer = 3f;
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
        float volume = 1f;
        float pitch = 1f;
        SoundFXManager.instance.PlaySoundFXClip(spaceshipExplodedSoundClip, transform, volume, pitch);
        Lives -= 1;
        IsDead = true;
        if (Lives > 0)
        {
            IsDead = false;
            Respawn();
        }
        else
        {
            StartCoroutine(Death(spaceshipDeathAnimationDuration));
        }
    }
    
    private IEnumerator Death(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
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
