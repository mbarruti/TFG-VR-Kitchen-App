using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] BuildingManager _buildingManager;

    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    private Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

    // -------------------------------------------

    public bool canPlace;

    public BoxCollider boxCollider;

    public List<Collider> detectedColliders;

    //private void OnCollisionStay(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    if (/*collision.collider != _buildingManager.hit.collider && */!detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Add(collider);
    //    }

    //}

    private void Start()
    {
        SetBoxVertices();
    }

    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            _buildingManager.cubos[i].transform.position = collision.GetContact(i).point;
        }

        if (collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.selectedBuildingObject.boxCollider.gameObject)
        {
            canPlace = IsInLimit(collision.collider, collision.GetContact(0));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.selectedBuildingObject.boxCollider.gameObject) canPlace = true;
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

    public bool IsInLimit(Collider collider, ContactPoint contactPoint)
    {
        //bool allSameSide = false;

        Vector3 auxVertex;
        //BoxCollider auxCollider = collider as BoxCollider;
        //auxCollider.center = 
        //Vector3[] directions = { Vector3.up };

        //Vector3[] vertices = GetBoxVertices();
        float previousNum = 0;
        float num = 0;
        foreach (Vector3 vertex in vertices)
        {
            Vector3 closestPoint = collider.ClosestPoint(transform.position);
            Vector3 diff = closestPoint - transform.position;
            Vector3 dir = diff.normalized;

            //if (Physics.Raycast(transform.position, dir, out var planeHit))
            //{
                // (A, B, C) vector perpendicular al plano del objeto que esta quieto
                Vector3 normal = contactPoint.normal;
                //Vector3 normal = planeHit.normal;

                // (x, y, z) un punto del plano del objeto que esta quieto /*+ Vector3.Dot(planeHit.normal, collider.bounds.extents);*/
                Vector3 point = contactPoint.point;
                Debug.DrawRay(point, normal, Color.blue);
                //Vector3 offset = Vector3.Scale(planeHit.normal, collider.bounds.extents);
                //Vector3 point = collider.transform.position + offset;

                // (x2, y2, z2) punto del vertice del objeto que pretendes mover
                auxVertex = transform.TransformPoint(vertex);

                // A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
                // D = -A*x - B*y - C*z Calculo la D de esta forma
                float d = Vector3.Dot(-normal, point);

                // A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
                if (num != 0) previousNum = num;
                num = Vector3.Dot(normal, auxVertex) + d;

                // Si algun valor tiene signo distinto a los demas, esta al otro lado del plano
                if (num > 0 && previousNum < 0)
                {
                    //Debug.Log(vertex);
                    return false;
                }
                if (num < 0 && previousNum > 0)
                {
                    //Debug.Log(vertex);
                    return false;
                }
            //}
        }
        return true;
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    //Debug.Log("sale");
    //    if (detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Remove(collider);
    //    }
    //}

    //Con el trigger izquierdo se rota el objeto en el eje Y (30 grados)
    public void RotateObject()
    {
        gameObject.transform.Rotate(Vector3.up, 30);
    }

    // Escala el collider según el valor del eje Y del mando derecho
    public void ScaleCollider(float value)
    {
        float scaleAmount = value * Time.deltaTime;
        boxCollider.size += Vector3.one * scaleAmount;
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
        boxCollider.size = selectedObject.transform.localScale;
    }
}
