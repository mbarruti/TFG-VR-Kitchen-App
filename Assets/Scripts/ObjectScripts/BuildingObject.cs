using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{
    //[SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    // Aqui se guardan las transformaciones previas a la edicion del objeto seleccionado
    //private Transform lastTransform;
    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    //private Collider hitCollider;

    private Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

    // ------------------------------------------------

    public BuildingManager _buildingManager;

    // Indica si se puede colocar el objeto o no
    public bool canPlace;

    public MeshRenderer meshRenderer;

    public BoxCollider boxCollider;

    public Rigidbody objectRigidbody;

    public bool isPlaced;

    public bool isInLimit;

    //public Vector3 offset;

    public List<Collider> detectedColliders;

    private void Start()
    {
        //Debug.Log("hitPos del Update:" + offset);
        //transform.position = _buildingManager._hitPos;
        //transform.position = new Vector3(0, 0, 0);

        SetBoxVertices();
    }

    private void Update()
    {
        //SetBoxVertices();

        //_buildingManager.cubos[0].transform.position = transform.TransformPoint(vertices[0]);
        //_buildingManager.cubos[1].transform.position = transform.TransformPoint(vertices[1]);
        //_buildingManager.cubos[2].transform.position = transform.TransformPoint(vertices[2]);
        //_buildingManager.cubos[3].transform.position = transform.TransformPoint(vertices[3]);
        //_buildingManager.cubos[4].transform.position = transform.TransformPoint(vertices[4]);
        //_buildingManager.cubos[5].transform.position = transform.TransformPoint(vertices[5]);
        //_buildingManager.cubos[6].transform.position = transform.TransformPoint(vertices[6]);
        //_buildingManager.cubos[7].transform.position = transform.TransformPoint(vertices[7]);
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    if (/*collision.collider != _buildingManager.hit.collider && */!detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Add(collider);
    //    }

    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    Debug.Log("sale");
    //    if (detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Remove(collider);
    //    }
    //}

    // Si el objeto colisiona con otros objetos, no se puede colocar
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Object"))
    //    {
    //        //Debug.Log("No se puede");
    //        canPlace = false;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Object"))
    //    {
    //        canPlace = true;
    //    }
    //}

    public bool IsInLimit(Collider collider)
    {
        //bool allSameSide = false;
        float distance;

        //Vector3 aux;

        //BoxCollider auxCollider = collider as BoxCollider;
        //auxCollider.center = 
        //Vector3[] directions = { Vector3.up };

        //Vector3[] vertices = GetBoxVertices();

        foreach (Vector3 vertex in vertices)
        {
            //aux = collider.transform.InverseTransformPoint(vertex);

            distance = Vector3.Dot(vertex - collider.transform.position, Vector3.up);
            if (distance < 0)
            {
                Debug.Log(distance);
                return false;
            }
        }

        return true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider != _buildingManager.hit.collider)
        {
            canPlace = IsInLimit(collision.collider);
            //Vector3[] auxVertices = vertices;

            //// Obtener el BoxCollider del objeto con el que choca
            //BoxCollider otherCollider = collision.collider as BoxCollider;

            //if (boxCollider != null && otherCollider != null)
            //{

            //    // Obtener la matriz de transformaci�n del espacio local al espacio del otro BoxCollider
            //    Matrix4x4 localToOtherMatrix = otherCollider.transform.worldToLocalMatrix * boxCollider.transform.localToWorldMatrix;

            //    // Transformar los v�rtices al espacio local del otro BoxCollider
            //    for (int i = 0; i < auxVertices.Length; i++)
            //    {
            //        auxVertices[i] = localToOtherMatrix.MultiplyPoint3x4(auxVertices[i]);
            //    }

            //    // Verificar si alguno de los v�rtices atraviesa cualquier plano del otro BoxCollider
            //    for (int i = 0; i < auxVertices.Length; i++)
            //    {
            //        if (auxVertices[i].z < 0)
            //        {
            //            Debug.Log("El v�rtice " + auxVertices[i] + " atraviesa el plano Z del otro objeto.");
            //            Debug.Log(auxVertices[i]);
            //            canPlace = false;
            //        }
            //    }
            //}
        }
    }

    // Get the collider vertices
    Vector3[] GetBoxVertices()
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

        return vertices;
    }

    // Get the collider vertices
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

    //Con el trigger izquierdo se rota el objeto en el eje Y (30 grados)
    public void RotateObject()
    {
        //Debug.Log(transform.TransformPoint(boxCollider.bounds.extents));
        gameObject.transform.Rotate(Vector3.up, 30);
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAA" + transform.TransformVector(boxCollider.size));
    }

    // Escala seg�n el valor del eje Y del mando derecho (falta prohibir que se escale a menor o igual que 0)
    public void ScaleObject(float value)
    {
        float scaleAmount = value * Time.deltaTime;
        transform.localScale += Vector3.one * scaleAmount;
        Debug.Log(transform.TransformVector(boxCollider.size));
    }

    public void assignMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public void switchOutline(bool enabled)
    {
        outline.enabled = enabled;
    }

    public void SavePreviousTransform()
    {
        _lastPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        _lastRot = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        _lastScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    public void SetPreviousTransform()
    {
        gameObject.transform.position = new Vector3(_lastPos.x, _lastPos.y, _lastPos.z);
        gameObject.transform.eulerAngles = new Vector3(_lastRot.x, _lastRot.y, _lastRot.z);
        gameObject.transform.localScale = new Vector3(_lastScale.x, _lastScale.y, _lastScale.z);
    }
}
