using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    public float gravity = 9.81f;

    [SerializeField]
    private ISet<Rigidbody> rigidbodies = new HashSet<Rigidbody>();

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            rigidbodies.Add(other.attachedRigidbody);

            // TODO: do we need to cache the old value of useGravity?
            other.attachedRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
            rigidbodies.Remove(other.attachedRigidbody);
    }

    void ApplyGravity()
    {
        foreach (var rb in rigidbodies.ToList())
        {
            try
            {
                Vector3 direction = (transform.position - rb.position).normalized;
                rb.AddForce(direction * gravity, ForceMode.Acceleration);
            } catch(MissingReferenceException) {
                //Debug.Log("This rigidbody component was destroyed!");
                rigidbodies.Remove(rb);
            }
        }
    }
}
