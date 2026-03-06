using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public virtual Vector3 GetGravity(Vector3 position) //Virtual allows subtypes to override the functionality of parent types
    {
        return Physics.gravity; //Default gravity setting
    }
    /// <summary>
    /// Adds the gravity field to the list if the object is enabled
    /// </summary>
    void OnEnable()
    {
        CustomGravity.Register(this);
    }
    /// <summary>
    /// Removes gravity fields from the list if the object disabled
    /// </summary>
    void OnDisable()
    {
        CustomGravity.Unregister(this);
    }
}
