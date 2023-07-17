using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public void YAxisView()
    {
        transform.position = new Vector3(0, 30, 0);
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void XAxisView()
    {
        transform.position = new Vector3(30, 0, 0);
        transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void XNegAxisView()
    {
        transform.position = new Vector3(-30, 0, 0);
        transform.eulerAngles = new Vector3(0, 90, 0);
    }

    public void ZAxisView()
    {
        transform.position = new Vector3(0, 0, 30);
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void ZNegAxisView()
    {
        transform.position = new Vector3(0, 0, -30);
        transform.eulerAngles = new Vector3(0, -180, 0);
    }

    //public void DeactivateView()
    //{

    //}
}
