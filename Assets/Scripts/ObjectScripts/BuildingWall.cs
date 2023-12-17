using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public BoxCollider boxCollider;

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

    public void SetActiveAxis(Vector3 normal)
    {
        if (normal == Vector3.right || normal == Vector3.left)
        {
            axisX = true;
            axisZ = false;
        }
        else if (normal == Vector3.forward || normal == Vector3.back)
        {
            axisX = false;
            axisZ = true;
        }

        endPole.SetActiveAxisSign(this, normal);
    }

    public void SetEndPolePosition(Vector3 sum)
    {
        if (axisX == true)
        {
            //var sum = _hitPos + GetOffset(hit.normal, endPole.GetComponent<BoxCollider>());

            if (endPole.activeAxis > 0)
                endPole.transform.position = new Vector3(Mathf.Clamp(sum.x, startPole.transform.position.x + startPole.transform.localScale.x, Mathf.Infinity), startPole.transform.position.y, startPole.transform.position.z);
            else if (endPole.activeAxis < 0)
                endPole.transform.position = new Vector3(Mathf.Clamp(sum.x, Mathf.NegativeInfinity, startPole.transform.position.x - startPole.transform.localScale.x), startPole.transform.position.y, startPole.transform.position.z);
            else
                endPole.transform.position = new Vector3(sum.x, startPole.transform.position.y, startPole.transform.position.z);
        }
        else
        {
            //var sum = _hitPos + GetOffset(hit.normal, endPole.GetComponent<BoxCollider>());
            if (endPole.activeAxis > 0)
                endPole.transform.position = new Vector3(startPole.transform.position.x, startPole.transform.position.y, Mathf.Clamp(sum.z, startPole.transform.position.z + startPole.transform.localScale.z, Mathf.Infinity));
            else if (endPole.activeAxis < 0)
                endPole.transform.position = new Vector3(startPole.transform.position.x, startPole.transform.position.y, Mathf.Clamp(sum.z, Mathf.NegativeInfinity, startPole.transform.position.z - startPole.transform.localScale.z));
            else
                endPole.transform.position = new Vector3(startPole.transform.position.x, startPole.transform.position.y, sum.z);
        }
    }
}
