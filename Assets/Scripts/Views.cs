using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Views : MonoBehaviour
{
    [SerializeField] WorldMenuManager menu;

    List<BuildingWall> filteredWalls = new ();

    // ----------------------------------

    public Vector3 wallsCenterPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TopView()
    {
        transform.position = new Vector3(wallsCenterPoint.x, 30f, wallsCenterPoint.z);
        transform.eulerAngles = new Vector3(90f, 0, 0);
    }

    public void RightView()
    {
        transform.position = new Vector3(30f, wallsCenterPoint.y, wallsCenterPoint.z);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, -90f, 0);
    }

    public void LeftView()
    {
        transform.position = new Vector3(-30f, wallsCenterPoint.y, wallsCenterPoint.z);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, 90f, 0);
    }

    public void FrontView()
    {
        transform.position = new Vector3(wallsCenterPoint.x, wallsCenterPoint.y, 30f);

        FilterWalls();

        transform.eulerAngles = new Vector3(0, 180f, 0);
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
