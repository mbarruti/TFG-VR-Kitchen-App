using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public BoxCollider boxCollider;

    public Renderer wallRenderer;

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
        transform.localScale = new Vector3(transform.localScale.x, startPole.transform.localScale.y, distance/* - 0.1f*/);
    }

    public void SetHeight(float padValue)
    {
        // Limit the minimum height of the wall to 2.5 meters and maximum to 6 meters
        float heightOffset = Mathf.Clamp(startPole.transform.localScale.y + (padValue * 0.05f), 2.5f, 6f);
        Debug.Log(heightOffset);

        startPole.transform.localScale = new Vector3(startPole.transform.localScale.x, heightOffset, startPole.transform.localScale.z);
        endPole.transform.localScale = new Vector3(endPole.transform.localScale.x, heightOffset, endPole.transform.localScale.z);

        // TO-DO: probar sin esta ultima linea
        transform.localScale = new Vector3(transform.localScale.x, startPole.transform.localScale.y, transform.localScale.z);
    }

    public void SetRotation(float rotationValue)
    {
        //startPole.transform.parent.eulerAngles = new Vector3(startPole.transform.parent.eulerAngles.x, startPole.transform.parent.eulerAngles.y + rotationValue, startPole.transform.parent.eulerAngles.z);
        startPole.transform.eulerAngles = new Vector3(startPole.transform.eulerAngles.x, startPole.transform.eulerAngles.y + rotationValue, startPole.transform.eulerAngles.z);
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
