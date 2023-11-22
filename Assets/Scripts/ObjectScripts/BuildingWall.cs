using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public BoxCollider boxCollider;

    public GameObject startPole;
    public GameObject endPole;

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
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance + 0.1f);
    }
}
