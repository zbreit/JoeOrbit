using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGun : ToolGun
{
    private readonly StackInventory inventory = new();

    public override void Shoot()
    {
        if (inventory.IsEmpty)
            return;

        // TODO: cache Camera.main!
        if (!RaycastCursorPosition(Camera.main, out var raycastHit))
            return;

        ItemScriptableObject item = inventory.RemoveItem();

        Instantiate(item.prefab, raycastHit.point, Random.rotation);
        Debug.Log(inventory);
    }

    public override void Suck()
    {
        // Raycast to find current object
        Item item = GetItemAtCursor();

        if (!item)
        {
            return;
        }

        if (inventory.Add(item.scriptableObject))
        {
            //Debug.Log("Successfully added item!");
        } 
        else
        {
            //Debug.Log("Inventory was out of space for this item!");
        }

        StartCoroutine(DelayedDestroy(item.gameObject));
        Debug.Log(inventory);
    }

    private bool RaycastCursorPosition(Camera camera, out RaycastHit raycastHit)
    {
        Vector3 centerOfScreen = new Vector3(0.5f, 0.5f, 0);
        Ray ray = camera.ViewportPointToRay(centerOfScreen);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3);

        return Physics.Raycast(ray, out raycastHit);
    }

    private Item GetItemAtCursor()
    {
        // TODO: cache Camera.main
        if (!RaycastCursorPosition(Camera.main, out var raycastHit))
        {
            return null;
        }

        return raycastHit.collider.gameObject.GetComponent<Item>();
    }

    // To ensure that any collisions are detected, we teleport the object away for a single physics frame
    // before destroying it. THIS IS QUITE HACKY.
    // TODO: ask Ryan about this!
    private IEnumerator DelayedDestroy(GameObject obj)
    {
        //Vector3 farLocation = new Vector3(1000000, 1000000, 1000000);
        //obj.transform.position = farLocation;

        //yield return new WaitForFixedUpdate();

        Destroy(obj);
        yield break;
    }
}
