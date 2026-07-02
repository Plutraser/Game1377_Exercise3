using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 10f;
    [SerializeField] private float maxThrust = 11f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] private float fireRate = 0.15f;

    private float rotationInput;
    private float thrustInput;
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleRotation();
        HandleHyperspace();
        FireBullet();
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
        transform.Rotate(Vector3.back * rotationInput * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Handles the movement of the ship, if pressing forward the ship will move forward. Does not allow movement backward
    /// </summary>
    private void HandleThrust()
    {
        if (thrustInput > 0)
        {
            rb.AddForce(moveDirection * thrustForce, ForceMode2D.Force);
        }
        if (rb.linearVelocity.magnitude > maxThrust)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxThrust;
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
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
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
        transform.position = randomLocation;
    }
}
