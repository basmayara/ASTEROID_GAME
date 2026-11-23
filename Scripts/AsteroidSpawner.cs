using System.Collections.Generic;
using UnityEngine;


public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;   // drag the Asteroid prefab here
    [SerializeField] int firstWaveCount = 3;    // number of big asteroids in wave 1
    [SerializeField] float baseSpeed = 1.5f;

    readonly List<Asteroid> alive = new List<Asteroid>(); // all asteroids currently in the scene
    int currentWave = 1;

    // Announced when the last asteroid is destroyed. GameManager listens to it.
    public event System.Action WaveCleared;

    // OnEnable / OnDisable: subscribe and unsubscribe to the event.
    // Always unsubscribe, otherwise the event keeps calling a dead object.
    void OnEnable() { Asteroid.Destroyed += OnAsteroidDestroyed; }
    void OnDisable() { Asteroid.Destroyed -= OnAsteroidDestroyed; }

    // Wave 1 = 3 large asteroids, wave 2 = 4, wave 3 = 5...
    public void SpawnWave(int wave)
    {
        currentWave = wave;
        int count = firstWaveCount + wave - 1;
        for (int i = 0; i < count; i++)
            Spawn(RandomEdgePoint(), AsteroidSize.Large);
    }

    // Removes every asteroid (used when a new game starts or when going back to the menu).
    public void ClearAll()
    {
        foreach (Asteroid a in alive)
            if (a != null) Destroy(a.gameObject);
        alive.Clear();
    }

    void Spawn(Vector2 position, AsteroidSize size)
    {
        // Small asteroids are faster: factor 1 (large), 1.5 (medium), 2 (small).
        float sizeFactor = 1f + (3 - (int)size) * 0.5f;
        // Speed grows with the wave, plus a little randomness (80% to 120%).
        float speed = (baseSpeed + currentWave * 0.2f) * sizeFactor * Random.Range(0.8f, 1.2f);

        // BUILDER: we read the steps like a sentence.
        Asteroid asteroid = new AsteroidBuilder(asteroidPrefab)
            .At(position)
            .OfSize(size)
            .MovingTowards(Random.insideUnitCircle) // a random direction
            .WithSpeed(speed)
            .Build();

        alive.Add(asteroid);
    }

    // Called by the event every time ANY asteroid is destroyed.
    void OnAsteroidDestroyed(Asteroid asteroid)
    {
        alive.Remove(asteroid);

        // Large -> 2 medium, Medium -> 2 small, Small -> nothing.
        if (asteroid.Size > AsteroidSize.Small)
        {
            AsteroidSize smaller = (AsteroidSize)((int)asteroid.Size - 1);
            Spawn(asteroid.transform.position, smaller);
            Spawn(asteroid.transform.position, smaller);
        }

        // No asteroid left (the new children were already counted above): wave finished.
        if (alive.Count == 0)
            WaveCleared?.Invoke();
    }

    // A random point on the border of the screen, so asteroids enter from outside.
    static Vector2 RandomEdgePoint()
    {
        float hw = ScreenBounds.HalfWidth;
        float hh = ScreenBounds.HalfHeight;

        if (Random.value < 0.5f) // left or right edge
            return new Vector2(Random.value < 0.5f ? -hw : hw, Random.Range(-hh, hh));

        // top or bottom edge
        return new Vector2(Random.Range(-hw, hw), Random.value < 0.5f ? -hh : hh);
    }
}
