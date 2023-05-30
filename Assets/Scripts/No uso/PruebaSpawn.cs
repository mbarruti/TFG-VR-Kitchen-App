using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerscript : MonoBehaviour
{

    public Vector3 clickPosition;
    public GameObject cubePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity);
        }
    }

    public void GenerateObject()
    {
        Instantiate(cubePrefab, transform.position, Quaternion.identity);
    }
}
