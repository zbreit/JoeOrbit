using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateGun : ToolGun
{
    private void OnEnable()
    {
        //Debug.Log("Manipulator Gun Enabled!");
    }

    private void OnDisable()
    {
        //Debug.Log("Manipulator Gun Disabled!");
    }
    public override void Shoot()
    {
        Debug.Log("Manipulator Gun shoot!");
    }

    public override void Suck()
    {
        Debug.Log("Manipulator Gun suck!");
    }
}
