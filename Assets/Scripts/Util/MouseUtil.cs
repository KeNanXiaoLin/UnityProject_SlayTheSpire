using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUtil : MonoBehaviour
{
    private static Camera main_Camera = Camera.main;

    public static Vector3 GetMousePositionInWorldSpace(float zValue)
    {
        Plane dragPlane = new Plane(main_Camera.transform.forward, new Vector3(0, 0, zValue));
        Ray ray = main_Camera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
