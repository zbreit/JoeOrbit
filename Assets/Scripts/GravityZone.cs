using System.Collections;
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
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (rb)
        {
            Debug.Log($"Gravitating {other.gameObject.name}");
            rigidbodies.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (rb)
        {
            Debug.Log($"Degravitating {other.gameObject.name}");
            rigidbodies.Remove(rb);
        }
    }

    void ApplyGravity()
    {
        foreach (var rb in rigidbodies)
        {
            Vector3 direction = (transform.position - rb.position).normalized;

            rb.AddForce(direction * gravity, ForceMode.Acceleration);

            Debug.DrawRay(rb.position, direction);
        }
    }
}
