using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, SelectionBase]
public class Sun : MonoBehaviour
{
    public float dist = 100;
    public float tilt = 0;
    public float pan = 0;
    private float rotation = 0;
    public float yearLength = 60;
    public GameObject sunModel;
    public GameObject sunLight;
    
    void Start()
    {
        PositionModel();
    }
    
    void OnValidate()
    {
        PositionModel();
    }

    void Update()
    {
        // Recalculate rotation
        rotation += (360 * Time.deltaTime / yearLength) % 360;
        SetRotation();
    }

    private void SetRotation() {
        // Quaternion desiredRotation = Quaternion.Euler(pan, 0, tilt);
        // desiredRotation = desiredRotation * Quaternion.AngleAxis(rotation, desiredRotation*(new Vector3(0,1,0)));
        // transform.rotation = desiredRotation;
        transform.rotation = Quaternion.Euler(0, pan, 0)*Quaternion.Euler(tilt, 0, 0)*Quaternion.Euler(0, rotation, 0);
    }

    private void PositionModel() {
        if (sunModel)
            sunModel.transform.localPosition = new Vector3(0,0,dist);
        if (sunLight)
            sunLight.transform.localPosition = new Vector3(0,0,dist);
    }
}
