using UnityEngine;

public class KeyboardInput : MonoBehaviour, IInputProvider
{
    // "Horizontal"  Left/Right   -1, 0 or +1.
    public float Rotation => Input.GetAxisRaw("Horizontal");

    // Up arrow, W (QWERTY) or Z (AZERTY). GetKey = true WHILE the key is held.
    public bool Thrust => Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z);
    public bool Fire => Input.GetKey(KeyCode.Space);

    // GetKeyDown = true only on the single frame when the key goes down.
    public bool SwitchWeaponPressed => Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
    public bool PausePressed => Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);
    public bool ConfirmPressed => Input.GetKeyDown(KeyCode.Return);
}
