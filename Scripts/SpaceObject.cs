using UnityEngine;


// BASE CLASS of everything that flies: Ship, Bullet, Asteroid.
//
// PATTERN USED: TEMPLATE METHOD

// SOLID: O L
    

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))] // Unity adds them automatically
public abstract class SpaceObject : MonoBehaviour
{
    // Speed and direction of the object, in world units per second.
    protected Vector2 velocity;

    // Awake runs once when the object is created, before Start.
    protected virtual void Awake()
    {
       
        var body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.useFullKinematicContacts = true;
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    // THE TEMPLATE METHOD. Unity calls Update() once per frame.
    // It is private on purpose: children must NOT replace it, they only fill OnTick().
    void Update()
    {
        OnTick();                                                       
        transform.position += (Vector3)(velocity * Time.deltaTime);     
        ScreenBounds.Wrap(transform);                                    
    }

   
    protected virtual void OnTick() { }
}
