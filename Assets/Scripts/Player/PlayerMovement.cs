using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// First-Person player movement that allows the player to move slightly in the air
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public GroundDetector groundDetector;
    public Transform playerHead;

    public float floorMoveSpeed = 6;
    public float airMoveSpeed = 6;
    public float breakSpeed = 1;

    // Used to calculate kinetic friction
    public Collider feetCollider;

    // Internal State Values
    private Vector3 moveDirection;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        MoveThePlayer();
    }

    // Input System Event Handler
    public void OnMove(InputAction.CallbackContext context)
    {
        SetMovementDirection(context.ReadValue<Vector2>());

        if (context.canceled)
        {
            SlowDownPlayer(breakSpeed);
        }
    }

    private void SetMovementDirection(Vector2 rawMoveDirection)
    {
        moveDirection = new Vector3(rawMoveDirection.x, 0, rawMoveDirection.y);
    }

    // Apply a velocity change to the player, without ever pushing the player backwards
    private void SlowDownPlayer(float speed)
    {
        Vector3 velocityChange = speed < playerRigidbody.velocity.magnitude
            ? -playerRigidbody.velocity.normalized * speed
            : -playerRigidbody.velocity;

        playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void MoveThePlayer()
    {
        Vector3 alignedMoveDirection = playerHead.rotation * moveDirection;

        // The player reaches max speed when all forces are at equilibrium, i.e., when
        // sum(forces) = moveForce - groundFriction - airDrag = 0
        Vector3 accelerationChange = alignedMoveDirection * (MaxAirDragAcceleration + GroundDragAcceleration);

        playerRigidbody.AddForce(accelerationChange, ForceMode.Acceleration);
    }


    // Private computed properties
    private float DynamicFrictionCoeff =>
        (feetCollider && feetCollider.material) ? feetCollider.material.dynamicFriction : 0;
    private float TargetMoveSpeed => groundDetector.IsGrounded ? floorMoveSpeed : airMoveSpeed;


    private float MaxAirDragAcceleration => playerRigidbody.drag * Mathf.Pow(TargetMoveSpeed, 1);

    // TODO: take angle of slopes into account
    // TODO: take friction combine into account?
    private float GroundDragAcceleration => groundDetector.IsGrounded
        ? Physics.gravity.magnitude * DynamicFrictionCoeff
        : 0;
}
