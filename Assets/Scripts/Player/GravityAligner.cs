using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aligns the player to point towards the direction of gravity
/// </summary>
public class GravityAligner : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    private GravityZone gravityZone;

    private void OnTriggerEnter(Collider other)
    {
        GravityZone zone = other.gameObject.GetComponent<GravityZone>();

        if (!zone) 
            return;

        //if (gravityZone)
        //    Debug.Log($"Swapping from gravity zone {gravityZone.gameObject.name} -> {zone.gameObject.name}");
        //else
        //    Debug.Log($"Entering gravity zone {zone.gameObject.name}");

        gravityZone = zone;
    }

    private void OnTriggerExit(Collider other)
    {
        GravityZone zone = other.gameObject.GetComponent<GravityZone>();

        if (zone)
        {
            Debug.Log($"Left a gravity zone: {zone.gameObject.name}");

            gravityZone = null;
        }
    }

    private void FixedUpdate()
    {
        if (gravityZone)
        {
            AlignToGravity();
        }
    }

    void AlignToGravity()
    {
        Vector3 difference = transform.position - gravityZone.transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, difference);

        playerRigidbody.MoveRotation(targetRotation);
    }
}
