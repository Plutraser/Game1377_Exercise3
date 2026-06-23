/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

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

    private float rotationInput;
    private float thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    void FixedUpdate()
    {
        moveDirection = (thrustInput * Vector3.up + rotationInput * Vector3.right).normalized;
        HandleThrust();
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.W)) //Change this to button later?
        {
            rb.AddForce(moveDirection * thrustForce, ForceMode2D.Force);
            if (rb.linearVelocity.magnitude > maxThrust)  // Figuring out the total speed of the object is greater than the max speed
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxThrust; //Capping the speed 
            }
        }
    }

    private void HandleFire()
    {

    }

    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            TeleportToRandomLocation();
        }
    }

    private void TeleportToRandomLocation()
    {
        Vector3 randomLocation = new Vector3(Random.Range(-6, 8), Random.Range(-3, 4), 0); //I found the coordinates of the width and height of the screen. Now im randomly picking a number between them.
        transform.position = randomLocation; //Places object in random coordinates
        Debug.Log("Left shift pressed.");
    }
}
