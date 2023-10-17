using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    public BoxCollider boxCollider;

    public List<Collider> detectedColliders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        Collider collider = collision.collider;
        if (/*collision.collider != _buildingManager.hit.collider && */!detectedColliders.Contains(collider))
        {
            detectedColliders.Add(collider);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        Debug.Log("sale");
        if (detectedColliders.Contains(collider))
        {
            detectedColliders.Remove(collider);
        }
    }
}
