using UnityEngine;


// PATTERN USED: BUILDER


public class AsteroidBuilder
{
    readonly Asteroid prefab;                  // the model to copy

    // Default values (used if a step is skipped).
    Vector2 position = Vector2.zero;
    AsteroidSize size = AsteroidSize.Large;
    Vector2 direction = Vector2.right;
    float speed = 1f;

    public AsteroidBuilder(Asteroid prefab)
    {
        this.prefab = prefab;
    }

    public AsteroidBuilder At(Vector2 position)
    {
        this.position = position;
        return this; // returning "this" is what allows the chain .At().OfSize()...
    }

    public AsteroidBuilder OfSize(AsteroidSize size)
    {
        this.size = size;
        return this;
    }

    public AsteroidBuilder MovingTowards(Vector2 direction)
    {
        // A zero direction would make a motionless asteroid, so we use a safe default.
        this.direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
        return this;
    }

    public AsteroidBuilder WithSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    // Last step: really create the object with everything we collected.
    public Asteroid Build()
    {
        Asteroid asteroid = Object.Instantiate(prefab, position, Quaternion.identity);
        asteroid.Init(size, direction * speed);
        return asteroid;
    }
}
