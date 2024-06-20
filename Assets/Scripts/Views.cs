using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Views : MonoBehaviour
{
    [SerializeField] WorldMenuManager menu;

    [SerializeField] GameObject axisXLine;
    [SerializeField] GameObject axisYLine;
    [SerializeField] GameObject axisZLine;
    [SerializeField] GameObject axisZLine2;

    [SerializeField] TMP_Text textX;
    [SerializeField] TMP_Text textY;
    [SerializeField] TMP_Text textZ;
    [SerializeField] TMP_Text textZ2;

    [SerializeField] GameObject floor;

    List<BuildingWall> filteredWalls = new ();

    // ----------------------------------

    public Vector3 wallsCenterPoint = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TopView()
    {
        if (wallsCenterPoint == Vector3.zero) wallsCenterPoint = menu.GetWallsCenterPoint();

        transform.position = new Vector3(wallsCenterPoint.x, 30f, wallsCenterPoint.z);
        transform.eulerAngles = new Vector3(90f, 0, 0);

        axisXLine.SetActive(true);
        axisYLine.SetActive(false);
        axisZLine.SetActive(false);
        axisZLine2.SetActive(true);

        textX.gameObject.SetActive(true);
        textY.gameObject.SetActive(false);
        textZ.gameObject.SetActive(false);
        textZ2.gameObject.SetActive(true);

        axisXLine.transform.localScale = new Vector3(axisXLine.transform.localScale.x, floor.transform.localScale.x, axisXLine.transform.localScale.z);
        axisXLine.transform.position = new Vector3(axisXLine.transform.position.x, axisXLine.transform.position.y, floor.transform.position.z - floor.transform.localScale.z / 2f - 0.8f);

        axisZLine2.transform.localScale = new Vector3(axisZLine2.transform.localScale.x, floor.transform.localScale.z, axisZLine2.transform.localScale.z);
        axisZLine2.transform.position = new Vector3(floor.transform.position.x - floor.transform.localScale.x / 2f - 0.8f, axisZLine2.transform.position.y, axisZLine2.transform.position.z);

        textX.SetText("" + axisXLine.transform.localScale.x);
        textZ2.SetText("" + axisZLine2.transform.localScale.y);
    }

    public void RightView()
    {
        if (wallsCenterPoint == Vector3.zero) wallsCenterPoint = menu.GetWallsCenterPoint();

        transform.position = new Vector3(30f, wallsCenterPoint.y, wallsCenterPoint.z);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, -90f, 0);

        axisXLine.SetActive(false);
        axisYLine.SetActive(true);
        axisZLine.SetActive(true);
        axisZLine2.SetActive(false);

        textX.gameObject.SetActive(false);
        textY.gameObject.SetActive(true);
        textZ.gameObject.SetActive(true);
        textZ2.gameObject.SetActive(false);

        axisYLine.transform.localScale = new Vector3(axisYLine.transform.localScale.x, menu.wallList[0].transform.localScale.y, axisYLine.transform.localScale.z);
        axisYLine.transform.position = new Vector3(axisYLine.transform.position.x, menu.wallList[0].transform.position.y, floor.transform.position.z - floor.transform.localScale.z / 2f - 0.8f);

        axisZLine.transform.localScale = new Vector3(axisZLine.transform.localScale.x, floor.transform.localScale.z, axisZLine.transform.localScale.z);
        axisZLine.transform.position = new Vector3(axisZLine.transform.position.x, floor.transform.TransformPoint(new Vector3(0, -5f, 0)).y, axisZLine.transform.position.z);

        textY.SetText("" + axisYLine.transform.localScale.y);
        textZ.SetText("" + axisZLine.transform.localScale.y);
    }

    public void LeftView()
    {
        if (wallsCenterPoint == Vector3.zero) wallsCenterPoint = menu.GetWallsCenterPoint();

        transform.position = new Vector3(-30f, wallsCenterPoint.y, wallsCenterPoint.z);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, 90f, 0);

        axisXLine.SetActive(false);
        axisYLine.SetActive(true);
        axisZLine.SetActive(true);
        axisZLine2.SetActive(false);

        textX.gameObject.SetActive(false);
        textY.gameObject.SetActive(true);
        textZ.gameObject.SetActive(true);
        textZ2.gameObject.SetActive(false);

        axisYLine.transform.localScale = new Vector3(axisYLine.transform.localScale.x, menu.wallList[0].transform.localScale.y, axisYLine.transform.localScale.z);
        axisYLine.transform.position = new Vector3(axisYLine.transform.position.x, menu.wallList[0].transform.position.y, floor.transform.position.z + floor.transform.localScale.z / 2f + 0.8f);

        axisZLine.transform.localScale = new Vector3(axisZLine.transform.localScale.x, floor.transform.localScale.z, axisZLine.transform.localScale.z);
        axisZLine.transform.position = new Vector3(axisZLine.transform.position.x, floor.transform.TransformPoint(new Vector3(0, -5f, 0)).y, axisZLine.transform.position.z);

        textY.SetText("" + axisYLine.transform.localScale.y);
        textZ.SetText("" + axisZLine.transform.localScale.y);
    }

    public void FrontView()
    {
        if (wallsCenterPoint == Vector3.zero) wallsCenterPoint = menu.GetWallsCenterPoint();

        transform.position = new Vector3(wallsCenterPoint.x, wallsCenterPoint.y, 30f);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, 180f, 0);

        axisXLine.SetActive(true);
        axisYLine.SetActive(true);
        axisZLine.SetActive(false);
        axisZLine2.SetActive(false);

        textX.gameObject.SetActive(true);
        textY.gameObject.SetActive(true);
        textZ.gameObject.SetActive(false);
        textZ2.gameObject.SetActive(false);

        axisXLine.transform.localScale = new Vector3(axisXLine.transform.localScale.x, floor.transform.localScale.x, axisXLine.transform.localScale.z);
        axisXLine.transform.position = new Vector3(axisXLine.transform.position.x, floor.transform.TransformPoint(new Vector3(0, -5f, 0)).y, axisXLine.transform.position.z);

        axisYLine.transform.localScale = new Vector3(axisYLine.transform.localScale.x, menu.wallList[0].transform.localScale.y, axisYLine.transform.localScale.z);
        axisYLine.transform.position = new Vector3(floor.transform.position.x + floor.transform.localScale.x / 2f + 0.8f, menu.wallList[0].transform.position.y, axisYLine.transform.position.z);

        textX.SetText("" + axisXLine.transform.localScale.y);
        textY.SetText("" + axisYLine.transform.localScale.y);
    }

    private void FilterWalls()
    {
        if (filteredWalls.Count > 0)
        {
            ResetWallLayers();
            filteredWalls = new List<BuildingWall>();
        }

        foreach (BuildingWall wall in menu.wallList)
        {
            if (wall.transform.InverseTransformPoint(transform.position).x < 0)
            {
                wall.gameObject.layer = LayerMask.NameToLayer("No Render");
                filteredWalls.Add(wall);
            }
        }
    }

    public void ResetWallLayers()
    {
        foreach (BuildingWall wall in filteredWalls)
        {
            wall.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    //public void ZNegAxisView()
    //{
    //    transform.position = new Vector3(wallsCenterPoint.x, wallsCenterPoint.y, -30);
    //    transform.eulerAngles = new Vector3(0, -180, 0);
    //}
}
