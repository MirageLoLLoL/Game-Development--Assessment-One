using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    void SetChildRigidbodies(bool enabled, Vector3 force)
    {
        var currentRB = GetComponent<Rigidbody>();
        currentRB.isKinematic = enabled;

        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            if(rb != currentRB)
            {
                rb.isKinematic = !enabled;
                rb.linearVelocity = force;
            }
        }
    }

    void SetChildColliders(bool enabled)
    {
        var currentCol = GetComponent<Collider>();
        currentCol.enabled = !enabled;
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (col != currentCol)
            {
                col.enabled = enabled;
            }
        }
    }

    public void OnRagdoll()
    {
        var force = GetComponent<Rigidbody>().linearVelocity;
        SetChildColliders(true);
        SetChildRigidbodies(true, force);
    }
}
