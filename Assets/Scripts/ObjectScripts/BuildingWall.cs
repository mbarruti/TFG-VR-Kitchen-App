using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public BoxCollider boxCollider;

    public Renderer renderer;

    public Pole startPole;
    public Pole endPole;

    public bool axisX;
    public bool axisZ;

    // TO-DO: guardar los Poles conectados a esta pared

    /// <summary>
    /// Adjust the width of the wall scaling the z axis
    /// </summary>
    public void AdjustWall()
    {
        //endPole.transform.position = _hitPos + GetOffset(hit.normal);

        //startPole.transform.LookAt(endPole.transform.position);
        //endPole.transform.LookAt(startPole.transform.position);

        float distance = Vector3.Distance(startPole.transform.position, endPole.transform.position);
        transform.position = startPole.transform.position + distance / 2 * startPole.transform.forward;
        transform.rotation = startPole.transform.rotation;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance - 0.1f);
    }

    public void SetEndPolePosition(Vector3 sum, GameObject plane)
    {
        // Start pole position in local coordinates of the plane hit
        Vector3 localStartPolePosition = plane.transform.InverseTransformPoint(startPole.transform.position);

        Vector3 localSum = plane.transform.InverseTransformPoint(sum);

        Vector3 localEndPolePosition = new Vector3(localStartPolePosition.x, localStartPolePosition.y, Mathf.Clamp(localSum.z, localStartPolePosition.z + startPole.transform.localScale.z, Mathf.Infinity));
        //Vector3 localEndPolePosition = new Vector3(localStartPolePosition.x, localStartPolePosition.y, Mathf.Clamp(localSum.z, localStartPolePosition.z + plane.transform.localPosition.z, Mathf.Infinity));

        endPole.transform.position = plane.transform.TransformPoint(localEndPolePosition);
    }
}
