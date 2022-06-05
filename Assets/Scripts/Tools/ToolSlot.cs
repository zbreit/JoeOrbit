using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolSlot : MonoBehaviour
{
    public ToolGun[] availableTools;

    public ToolGun activeTool => (activeToolIndex >= 0 && availableTools.Length > 0) 
        ? availableTools[activeToolIndex]
        : null;

    private int activeToolIndex = 0;

    // Unfortunately, there's no way to get continuous callbacks using the input system,
    // so we have to track this ourselves.
    private bool sucking = false;

    private void Start()
    {
        foreach(ToolGun tool in availableTools)
        {
            tool.gameObject.SetActive(false);
        }


        if (activeTool)
            activeTool.gameObject.SetActive(true);
        else
            Debug.LogWarning("No available tools for this tool slot!");
    }

    private void Update()
    {
        if (sucking && activeTool)
            activeTool.Suck();
    }

    // Unity Input Callbacks
    public void OnSwapTools(InputAction.CallbackContext context)
    {
        if (!context.performed || !activeTool) 
            return;

        float direction = context.ReadValue<float>();

        //Debug.Log($"Swapped tools w/ direction {direction}");

        if (direction > 0)
            NextTool();
        else
            PreviousTool();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && activeTool)
            activeTool.Shoot();
    }

    public void OnSuck(InputAction.CallbackContext context)
    {
        if (context.performed)
            sucking = true;
        else if (context.canceled)
            sucking = false;
    }

    public void NextTool()
    {
        SetActiveTool((activeToolIndex + 1) % availableTools.Length);
    }

    public void PreviousTool()
    {
        SetActiveTool((activeToolIndex - 1) % availableTools.Length);
    }

    public void SetActiveTool(int toolIndex)
    {
        activeTool.gameObject.SetActive(false);
        activeToolIndex = toolIndex;
        activeTool.gameObject.SetActive(true);
    }
}
