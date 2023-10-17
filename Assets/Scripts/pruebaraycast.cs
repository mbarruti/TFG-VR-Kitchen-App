using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaraycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(gameObject.GetComponent<BoxCollider>().bounds.center, new Vector3(0f, -1f, 0f), out var hit, 1000))
        {
            Debug.DrawRay(gameObject.GetComponent<BoxCollider>().bounds.center, new Vector3(0f, -1f, 0f));
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
