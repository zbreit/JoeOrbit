using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public float sphereCheckRadius = 0.1f;
    public LayerMask groundMask;

    [field: SerializeField]
    public bool IsGrounded { get; private set; }


    private void FixedUpdate()
    {
        // NOTE: we compute this once every fixed update so that, if multiple scripts access this value,
        // we don't perform the raycast calculation multiple times.
        //
        // TODO: Investigate if a stale IsGrounded value can cause bugs.
        IsGrounded = Physics.CheckSphere(transform.position, sphereCheckRadius, groundMask, QueryTriggerInteraction.Ignore);


        Debug.DrawLine(transform.position, transform.position - transform.up.normalized * sphereCheckRadius, Color.red);
    }
}
