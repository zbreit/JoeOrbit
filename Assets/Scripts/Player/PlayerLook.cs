using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform head;
    public Transform eye;
    public float turnSensitivity = 0.01f;
    public AngleBounds tiltBounds = new() { minDegrees = -75, maxDegrees = 75 };

    private float tilt = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var lookDirection = context.ReadValue<Vector2>();
        Look(lookDirection);
    }

    private void Look(Vector2 deltaLook)
    {
        TiltEye(deltaLook.y * turnSensitivity);
        TurnHead(deltaLook.x * turnSensitivity);
    }

    private void TiltEye(float deltaTilt)
    {
        tilt = Mathf.Clamp(tilt + deltaTilt, tiltBounds.minDegrees, tiltBounds.maxDegrees);
        eye.localEulerAngles = Vector3.right * tilt;
    }

    private void TurnHead(float deltaPan)
    {
        head.rotation *= Quaternion.Euler(0, deltaPan, 0);
    }
}
