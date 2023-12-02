using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public BoxCollider boxCollider;

    public GameObject startPole;
    public GameObject endPole;

    public bool axisX;
    public bool axisZ;

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

    //public void SetStartPole()
    //{
    //    if (poleList.Count == 0)
    //    {
    //        startPole.transform.position = _hitPos + GetOffset(hit.normal, startPole.GetComponent<BoxCollider>());

    //        finish = true;

    //        // Instantiate the wall in the world
    //        GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
    //        wall = auxWall.GetComponent<BuildingWall>();
    //        wall.startPole = startPole;
    //        wall.endPole = endPole;
    //    }
    //    else if (hit.collider.tag == "Wall")
    //    {
    //        wallHit = hit;

    //        startPole.transform.position = hit.collider.bounds.center;
    //        //startPole.transform.rotation = hit.collider.gameObject.transform.rotation;

    //        finish = true;

    //        // Instantiate the wall in the world
    //        GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
    //        wall = auxWall.GetComponent<BuildingWall>();
    //        wall.startPole = startPole;
    //        wall.endPole = endPole;
    //    }
    //}

    //public void SetEndPole()
    //{
    //    finish = false;

    //    var aux = Instantiate(startPole, startPole.transform.position, startPole.transform.rotation);
    //    var aux2 = Instantiate(endPole, endPole.transform.position, endPole.transform.rotation);

    //    aux.layer = LayerMask.NameToLayer("Default");
    //    aux2.layer = LayerMask.NameToLayer("Default");

    //    poleList.Add(aux);
    //    poleList.Add(aux2);

    //    startPole.transform.position = new Vector3(0, -20, 0);
    //    endPole.transform.position = new Vector3(0, -20, 0);

    //    startPole.transform.eulerAngles = new Vector3(0, 0, 0);
    //    endPole.transform.eulerAngles = new Vector3(0, 0, 0);
    //}
}
