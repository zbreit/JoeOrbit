using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Camera camera;
    
    public Shader miniMapShader;

    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetComponent<Camera>();
        if (miniMapShader)
            camera.SetReplacementShader(miniMapShader, "Default");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
