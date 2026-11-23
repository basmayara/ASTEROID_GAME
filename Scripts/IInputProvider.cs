
public interface IInputProvider
{
    float Rotation { get; }            // -1 = left +1 = right  0 = ntg
    bool Thrust { get; }               // Z true 
    bool Fire { get; }                 // space true 
    bool SwitchWeaponPressed { get; }  //shift true 
    bool PausePressed { get; }         //p  true 
    bool ConfirmPressed { get; }       // Enter key start / restart
}
//solid : D  DEPENDENCY INVERSION PRINCIPLE ship and game manager depend on this interface 