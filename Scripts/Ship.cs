using UnityEngine;


public class Ship : SpaceObject
{
    [Header("Movement")]                          // [SerializeField] = visible in the Inspector
    [SerializeField] float rotationSpeed = 200f;  // degrees per second
    [SerializeField] float thrustPower = 8f;      // acceleration when pushing forward
    [SerializeField] float maxSpeed = 7f;         // speed limit
    [SerializeField] float damping = 0.4f;        // natural slow down (friction in space)

    [Header("Shooting")]
    [SerializeField] Bullet bulletPrefab;         

    [Header("Respawn")]
    [SerializeField] float invincibleTime = 2f;   // seconds of protection after respawn

    [Header("Input")]
    [SerializeField] MonoBehaviour inputSource;   // drag the "Input" object here
   

    IInputProvider input;       
    SpriteRenderer sprite;    
    BulletFactory bulletFactory;
    IWeapon[] weapons;          
    int weaponIndex;           
    IWeapon weapon;             
    float nextFireTime;         
    float invincibleUntil;      

  
    public event System.Action Destroyed;

    public bool IsInvincible => Time.time < invincibleUntil;

    protected override void Awake()
    {
        base.Awake(); // keep the Awake of SpaceObject (kinematic body, trigger collider)

        input = (IInputProvider)inputSource;
        sprite = GetComponent<SpriteRenderer>();

        bulletFactory = new BulletFactory(bulletPrefab);
        weapons = new IWeapon[] { new SingleShot(), new TripleShot() };
        EquipWeapon(0);
    }

    // This is the "variable step" of the TEMPLATE METHOD defined in SpaceObject.
    // SpaceObject.Update() calls it every frame, then moves and wraps the ship.
    protected override void OnTick()
    {
        if (Time.timeScale == 0f) return; // the game is paused: do nothing

        // 1 Rotate. Minus sign because a positive input must turn clockwise.
        transform.Rotate(0f, 0f, -input.Rotation * rotationSpeed * Time.deltaTime);

        // 2 Thrust: add speed in the direction the tip is pointing (transform.up).
        if (input.Thrust)
            velocity += (Vector2)transform.up * thrustPower * Time.deltaTime;

        // 3 Friction: lose a little speed every frame, then limit the top speed.
        velocity *= Mathf.Max(0f, 1f - damping * Time.deltaTime);
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // 4 Change weapon (STRATEGY): go to the next one, and loop back to the first.
        if (input.SwitchWeaponPressed)
            EquipWeapon((weaponIndex + 1) % weapons.Length);

        // 5) Shoot, but only if the cooldown of the current weapon is over.
        if (input.Fire && Time.time >= nextFireTime)
            Shoot();

        // 6) Blink while invincible (visible 5 times per second).
        sprite.enabled = !IsInvincible || Mathf.FloorToInt(Time.time * 10f) % 2 == 0;
    }

    
    void EquipWeapon(int index)
    {
        weaponIndex = index;
        weapon = weapons[index];
        sprite.color = weapon.Tint;
    }

    void Shoot()
    {
        nextFireTime = Time.time + weapon.Cooldown;
        weapon.Fire(bulletFactory, transform); // the weapon decides how many bullets and where
    }

    // Called by GameManager to put the ship back in the middle of the screen.
    public void Respawn()
    {
        gameObject.SetActive(true); // first: makes sure Awake has run
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        velocity = Vector2.zero;
        invincibleUntil = Time.time + invincibleTime;
        sprite.enabled = true;
        EquipWeapon(0); // always restart with the single shot
    }

    // Unity calls this every physics step while another trigger collider overlaps ours.
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsInvincible || !gameObject.activeSelf) return;
        if (other.GetComponent<Asteroid>() == null) return; // we only die from asteroids
        Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
        Destroyed?.Invoke(); // "?." = only call it if someone is listening
    }
}
