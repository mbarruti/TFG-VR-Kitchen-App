using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Views : MonoBehaviour
{
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
        transform.position = new Vector3(wallsCenterPoint.x, 30, wallsCenterPoint.z);
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void RightView()
    {
        transform.position = new Vector3(30, wallsCenterPoint.y, wallsCenterPoint.z);
        transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void LeftView()
    {
        transform.position = new Vector3(-30, wallsCenterPoint.y, wallsCenterPoint.z);
        transform.eulerAngles = new Vector3(0, 90, 0);
    }

    public void FrontView()
    {
        transform.position = new Vector3(wallsCenterPoint.x, wallsCenterPoint.y, -30);
        transform.eulerAngles = new Vector3(0, -180, 0);
    }

    //public void ZNegAxisView()
    //{
    //    transform.position = new Vector3(wallsCenterPoint.x, wallsCenterPoint.y, -30);
    //    transform.eulerAngles = new Vector3(0, -180, 0);
    //}
}
