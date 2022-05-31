using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows the player to jump when they're grounded.
/// 
/// TODO: add variable jump height (based on duration of tap)
/// TODO: improve "jump curve" so the jump feels snappy and responsive instead of floaty
/// </summary>
public class PlayerJump : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    public GroundDetector groundDetector;

    public float jumpVelocityChange = 10;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && groundDetector.IsGrounded)
        {
            playerRigidbody.AddForce(jumpVelocityChange * playerRigidbody.transform.up, ForceMode.VelocityChange);
        } 
        
        //else if (context.canceled)
        //{
        //    jumping = false;
        //}
    }


    //[Header("Jump Force Params")]
    //public float upwardsTime = 0.8f;
    //public float timeConstant = 3.7f;
    //public float scaleFactor = 0;

    //private bool jumping = false;
    //private float startTime = 0;


    // maxJumpHeight = 1/2 * g * jumpDuration^2. We solve for jumpDuration below:
    //private float JumpDuration => Mathf.Sqrt(2 * maxJumpHeight / Physics.gravity.magnitude);
    //private bool ReachedMaxJumpHeight =>
    //    Time.time - startTime >= JumpDuration;

    //private IEnumerator JumpCoroutine()
    //{
    //    if (jumping)
    //        yield break;

    //    jumping = true;
    //    startTime = Time.time;

    //    Debug.Log("Start Jumping!");

    //    while (jumping && !ReachedMaxJumpHeight)
    //    {
    //        playerRigidbody.AddForce(jumpAcceleration * Vector3.up, ForceMode.Acceleration);
    //        yield return new WaitForFixedUpdate();
    //    }

    //    Debug.Log("Stop Jumping!");

    //    jumping = false;
    //}

    // Calculates an exponential jump force at a given point in time
    //private Vector3 ExponentialForce(float time)
    //{
    //    float tau = timeConstant / upwardsTime;
    //    float magnitude = Mathf.Exp(-time / tau) + Physics.gravity.magnitude;

    //    return magnitude * Vector3.up;
    //}
}
