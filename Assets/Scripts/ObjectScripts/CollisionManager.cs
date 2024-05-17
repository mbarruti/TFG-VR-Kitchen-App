using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] BuildingManager _buildingManager;

    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    //private Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

    // -------------------------------------------

    public Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

    public bool canPlace;

    public BoxCollider boxCollider;

    public Vector3 globalColliderSize;

    public List<Collider> detectedColliders;

    private void Start()
    {
        SetBoxVertices();
    }

    //private void Update()
    //{
    //    _buildingManager.cubos[0].transform.position = transform.TransformPoint(vertices[0]);
    //    _buildingManager.cubos[1].transform.position = transform.TransformPoint(vertices[1]);
    //    _buildingManager.cubos[2].transform.position = transform.TransformPoint(vertices[2]);
    //    _buildingManager.cubos[3].transform.position = transform.TransformPoint(vertices[3]);
    //    _buildingManager.cubos[4].transform.position = transform.TransformPoint(vertices[4]);
    //    _buildingManager.cubos[5].transform.position = transform.TransformPoint(vertices[5]);
    //    _buildingManager.cubos[6].transform.position = transform.TransformPoint(vertices[6]);
    //    _buildingManager.cubos[7].transform.position = transform.TransformPoint(vertices[7]);
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    //for (int i = 0; i < collision.contactCount; i++)
    //    //{
    //    //    _buildingManager.cubos[i].transform.position = collision.GetContact(i).point;
    //    //}

    //    //if (collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.selectedBuildingObject.boxCollider.gameObject)
    //    //{
    //    //    canPlace = IsInLimit(collision.collider, collision.GetContact(0));
    //    //}
    //}

    private void OnCollisionStay(Collision collision)
    {
        Collider collider = collision.collider;
        if (/*collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.selectedBuildingObject.boxCollider.gameObject && */!detectedColliders.Contains(collider))
        {
            detectedColliders.Add(collider);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        if (detectedColliders.Contains(collider))
        {
            detectedColliders.Remove(collider);
        }
    }

    // Set the collider vertices
    void SetBoxVertices()
    {
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size * 0.5f; // Divide to get half of the collider

        //Vector3[] vertices = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            float x = ((i & 1) == 0) ? size.x : -size.x;
            float y = ((i & 2) == 0) ? size.y : -size.y;
            float z = ((i & 4) == 0) ? size.z : -size.z;

            vertices[i] = center + new Vector3(x, y, z);

            //Debug.Log(vertices[i]);
        }


    }

    /// <summary>
    /// Rotate the object in the Y axis
    /// </summary>
    public void RotateObject(float value)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + value, transform.eulerAngles.z);
    }

    // Escala el collider según el valor del eje Y del mando derecho
    public void ScaleColliderManager(float value)
    {
        float scaleAmount = value * Time.deltaTime;
        transform.localScale += Vector3.one * scaleAmount;
    }

    //public void SavePreviousTransform()
    //{
    //    _lastPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    //    _lastRot = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    //    _lastScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    //}

    //public void SetPreviousTransform()
    //{
    //    gameObject.transform.position = new Vector3(_lastPos.x, _lastPos.y, _lastPos.z);
    //    gameObject.transform.eulerAngles = new Vector3(_lastRot.x, _lastRot.y, _lastRot.z);
    //    gameObject.transform.localScale = new Vector3(_lastScale.x, _lastScale.y, _lastScale.z);
    //}

    /// <summary>
    /// Reset the transform of this object and remove all detected colliders from the list
    /// </summary>
    public void Reset()
    {
        //boxCollider.transform.localScale = new Vector3(1, 1, 1);
        boxCollider.size = new Vector3(1, 1, 1);
        transform.position = new Vector3(0, -3, 0);
        transform.eulerAngles = new Vector3(0, 0, 0);

        detectedColliders.Clear();

        //Debug.Log(detectedColliders.Count);
    }

    /// <summary>
    /// Set the size of the box collider equal to the scale of the selected object
    /// </summary>
    public void SetScale(BuildingObject selectedObject)
    {
        //boxCollider.transform.position = new Vector3(selectedObject.boxCollider..x, _lastPos.y, _lastPos.z);
        //boxCollider.transform.eulerAngles = new Vector3(selectedObject.x, _lastRot.y, _lastRot.z);
        //boxCollider.transform.localScale = new Vector3(selectedObject.boxCollider.transform.localScale.x, selectedObject.boxCollider.transform.localScale.y, selectedObject.boxCollider.transform.localScale.z);

        // Por ahora funciona, pero hay que probar con objetos de cocina con distintas formas por si acaso
        //boxCollider.size = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
        transform.localScale = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
        boxCollider.size = new Vector3(selectedObject.boxCollider.size.x, selectedObject.boxCollider.size.y, selectedObject.boxCollider.size.z);

        // Update the vertices of the collider
        SetBoxVertices();

        globalColliderSize = Vector3.Scale(boxCollider.size, transform.lossyScale);
    }

    public void SetRotation(BuildingObject selectedObject)
    {
        transform.rotation = selectedObject.transform.rotation;
    }
}
