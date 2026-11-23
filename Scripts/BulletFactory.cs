using UnityEngine;


// PATTERN USED: FACTORY METHOD 


public class BulletFactory
{
    readonly Bullet prefab; // the Bullet prefab: the "model" that Unity copies

    public BulletFactory(Bullet prefab)
    {
        this.prefab = prefab;
    }

    // Creates ONE bullet and returns it (that is what a Factory Method does).
    public Bullet Create(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        // Instantiate = make a copy of the prefab in the scene.
        // (Unity's Instantiate is itself the PROTOTYPE pattern: clone a model.)
        Bullet bullet = Object.Instantiate(prefab, position, rotation);
        bullet.Launch(direction);
        return bullet;
    }
}
