using UnityEngine;


public interface IWeapon
{
    string Name { get; }       // for debugging
    float Cooldown { get; }    // seconds between two shots
    Color Tint { get; }        // color of the ship while this weapon is equipped
    void Fire(BulletFactory factory, Transform muzzle); // "muzzle" = the ship (position + direction)
}

// Weapon 1: one bullet straight ahead (the original behavior).
public class SingleShot : IWeapon
{
    public string Name => "Single shot";
    public float Cooldown => 0.25f;
    public Color Tint => Color.white;

    public void Fire(BulletFactory factory, Transform muzzle)
    {
       
        Vector3 spawn = muzzle.position + muzzle.up * 0.5f;
        factory.Create(spawn, muzzle.rotation, muzzle.up);
    }
}

public class TripleShot : IWeapon
{
    const float SpreadAngle = 12f; // degrees between two bullets

    public string Name => "Triple shot";
    public float Cooldown => 0.4f;                    // a bit slower, to stay balanced
    public Color Tint => new Color(0.4f, 0.9f, 1f);   // light blue ship

    public void Fire(BulletFactory factory, Transform muzzle)
    {
        Vector3 spawn = muzzle.position + muzzle.up * 0.5f;

        for (int i = -1; i <= 1; i++) // i = -1, 0, +1
        {
            // Turn the ship's rotation by -12, 0 or +12 degrees around the Z axis.
            Quaternion rotation = muzzle.rotation * Quaternion.Euler(0f, 0f, i * SpreadAngle);
            // "rotation * Vector3.up" = the "up" direction after that rotation.
            factory.Create(spawn, rotation, rotation * Vector3.up);
        }
    }
}
