using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // These variables determine the spawn area for the asteroids.
    // They are calculated at Start based off of the camera size. 
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private int initialAsteroids;
    [SerializeField] private GameObject[] powerupPrefabs;
    private float spawnXMax = 0f;
    private float spawnXMin = 0f;
    private float spawnYMax = 0f;
    private float spawnYMin = 0f;
    private float spawnXMaxPowerups = 0f;
    private float spawnXMinPowerups = 0f;
    private float spawnYMaxPowerups = 0f;
    private float spawnYMinPowerups = 0f;

    private float playerSafeDistance = 3;
    
    void Start()
    {
        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = Camera.main.aspect * screenHalfHeight;
        spawnXMax = screenHalfWidth + playerSafeDistance;
        spawnXMin = -screenHalfWidth - playerSafeDistance;
        spawnYMax = screenHalfHeight + playerSafeDistance;
        spawnYMin = -screenHalfHeight - playerSafeDistance;

        spawnXMaxPowerups = (screenHalfWidth);
        spawnXMinPowerups = (-screenHalfWidth);
        spawnYMaxPowerups = (screenHalfHeight);
        spawnYMinPowerups = (-screenHalfHeight);
        SpawnInitialAsteroids();
        SpawnPowerups();
    }
    /// <summary>
    /// Spawn the five initial asteroids at the beginning of the game. They spawn a safe distance from the player.
    /// </summary>
    private void SpawnInitialAsteroids()
    {
        for(int i = initialAsteroids; i > 0; i--)
        {
            SpawnAsteroid(new Vector3 (Random.Range(spawnXMin, spawnXMax), Random.Range(spawnYMin, spawnYMax)), Asteroid.AsteroidSize.Large);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SpawnPowerups()
    {
        for (int i = 0; i < powerupPrefabs.Length; i++)
        {
            SpawnAllPowerups((new Vector3 (Random.Range(spawnXMinPowerups, spawnXMaxPowerups), Random.Range(spawnYMinPowerups, spawnYMaxPowerups))), i);
        }
    }

    /// <summary>
    /// Spawns asteroid taking in the parameters of position and size. Also allows the spawned object to be able to use this script.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    public void SpawnAsteroid(Vector3 position, Asteroid.AsteroidSize size)
    {
        // Spawn an asteroid at the location specified by position parameter with the size specified by the size parameter.
        GameObject spawnedAsteroid = Instantiate(asteroidPrefabs[(int)size], position, asteroidPrefabs[(int)size].transform.rotation);
        //GetComponent<SpaceshipController>().asteroids.Add(spawnedAsteroid);
        spawnedAsteroid.GetComponent<Asteroid>().AsteroidSpawnerScript = this;
        
    }

    private void SpawnAllPowerups(Vector3 position, int powerup)
    {
        GameObject spawnedPowerup = Instantiate(powerupPrefabs[powerup], position, powerupPrefabs[powerup].transform.rotation);
    }
}