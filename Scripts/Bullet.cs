using UnityEngine;


public class Bullet : SpaceObject
{
    [SerializeField] float speed = 12f;     // units per second
    [SerializeField] float lifetime = 1.2f; // seconds before it disappears alone

    bool used; // protects against hitting two asteroids in the same frame

    // Called by BulletFactory right after the bullet is created.
    public void Launch(Vector2 direction)
    {
        velocity = direction.normalized * speed; // normalized = length 1, so speed is always the same
        Destroy(gameObject, lifetime);           // Destroy(object, delay) = delete it after "lifetime" seconds
    }

    // Unity calls this once when this trigger starts touching another collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid == null) return; // we touched something that is not an asteroid

        used = true;
        asteroid.Hit();
        Destroy(gameObject);
    }
}
