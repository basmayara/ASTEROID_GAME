using UnityEngine;

public enum AsteroidSize
{
    Small = 1,
    Medium = 2,
    Large = 3
}


public class Asteroid : SpaceObject
{
    // "static event": one event shared by ALL asteroids. Anyone can subscribe with
    //     Asteroid.Destroyed += MyFunction;
    public static event System.Action<Asteroid> Destroyed;

    public AsteroidSize Size { get; private set; } // readable by everybody, changeable only here

    // Score depending on the size (a switch expression = a short "if / else if").
    public int Points => Size switch
    {
        AsteroidSize.Large => 20,
        AsteroidSize.Medium => 50,
        _ => 100               // "_" = everything else (Small): small ones are harder to hit
    };

    bool hit; // protects against being hit twice

    // Called by AsteroidBuilder right after the asteroid is created.
    public void Init(AsteroidSize size, Vector2 initialVelocity)
    {
        Size = size;
        velocity = initialVelocity;

        float scale = size switch
        {
            AsteroidSize.Large => 1.6f,
            AsteroidSize.Medium => 0.9f,
            _ => 0.5f
        };
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    // Called by a Bullet when it touches this asteroid.
    public void Hit()
    {
        if (hit) return;
        hit = true;
        Destroyed?.Invoke(this); // announce to everybody who listens
        Destroy(gameObject);
    }
}
