using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{
    //[SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    [SerializeField] private Material[] collisionMaterials;

    // Aqui se guardan las transformaciones previas a la edicion del objeto seleccionado
    //private Transform lastTransform;
    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    //private Vector3 localMovement;
    private float touchpadValueX;
    private float touchpadValueY;

    //private Collider hitCollider;

    //private Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

    //private Vector3 boxColliderCenter;

    // ------------------------------------------------

    public GameObject surfaceObject;
    public Vector3 surfaceNormal;

    public Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider

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
        _buildingManager.cubos[0].transform.position = transform.TransformPoint(vertices[0]);
        _buildingManager.cubos[1].transform.position = transform.TransformPoint(vertices[1]);
        _buildingManager.cubos[2].transform.position = transform.TransformPoint(vertices[2]);
        _buildingManager.cubos[3].transform.position = transform.TransformPoint(vertices[3]);
        _buildingManager.cubos[4].transform.position = transform.TransformPoint(vertices[4]);
        _buildingManager.cubos[5].transform.position = transform.TransformPoint(vertices[5]);
        _buildingManager.cubos[6].transform.position = transform.TransformPoint(vertices[6]);
        _buildingManager.cubos[7].transform.position = transform.TransformPoint(vertices[7]);
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

    //public bool IsInLimit(Collider collider)
    //{
    //    //bool allSameSide = false;

    //    Vector3 auxVertex;
    //    //BoxCollider auxCollider = collider as BoxCollider;
    //    //auxCollider.center = 
    //    //Vector3[] directions = { Vector3.up };

    //    //Vector3[] vertices = GetBoxVertices();

    //    foreach (Vector3 vertex in vertices)
    //    {
    //        //auxVertex = transform.TransformPoint(vertex);
    //        //distance = Vector3.Dot(auxVertex - collider.transform.position + new Vector3(0, 0, 0.05f), Vector3.up);

    //        // (A, B, C) vector perpendicular al plano del objeto que esta quieto
    //        Vector3 up = Vector3.up;
    //        // (x, y, z) un punto del plano del objeto que esta quieto
    //        Vector3 point;
    //        point.x = collider.transform.position.x;
    //        point.y = collider.transform.position.y + collider.bounds.extents.y;
    //        point.z = collider.transform.position.z;
    //        // (x2, y2, z2) punto del vertice del objeto que pretendes mover
    //        auxVertex = transform.TransformPoint(vertex);
    //        // A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
    //        // D = -A*x - B*y - C*z Calculo la D de esta forma
    //        float d = Vector3.Dot(-up, point);
    //        // A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
    //        float num = Vector3.Dot(up, auxVertex) + d;
    //        //auxVertex = collider.transform.InverseTransformPoint(vertex);
    //        //distance = Vector3.Dot(auxVertex, Vector3.up);
    //        //if (distance < 0)
    //        if (num < 0)
    //        {
    //            // (A, B, C) vector perpendicular al plano del objeto que esta quieto
    //            // (x, y, z) un punto del plano del objeto que esta quieto
    //            // (x2, y2, z2) punto del vertice del objeto que pretendes mover
    //            // A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
    //            // D = -A*x - B*y - C*z Calculo la D de esta forma
    //            // A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
    //            Debug.Log(vertex);
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    //for (int i = 0; i < collision.contactCount; i++)
    //    //{
    //    //    _buildingManager.cubos[i].transform.position = collision.GetContact(i).point;
    //    //}

    //    if (collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.parentObject.gameObject)
    //    {
    //        canPlace = IsInLimit(collision.collider, collision.GetContact(0));
    //        //if (canPlace == false)
    //        //{
    //        //    AssignMaterial(collisionMaterials[0]);
    //        //}
    //        //else
    //        //{
    //        //    AssignMaterial(collisionMaterials[1]);
    //        //}
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.parentObject.gameObject)
    //    {
    //        canPlace = true;
    //    }
    //}

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

            if (Physics.Raycast(transform.position, dir, out var planeHit))
            {
                // (A, B, C) vector perpendicular al plano del objeto que esta quieto
                //Vector3 normal = contactPoint.normal;
                Vector3 normal = planeHit.normal;

                // (x, y, z) un punto del plano del objeto que esta quieto /*+ Vector3.Dot(planeHit.normal, collider.bounds.extents);*/
                //Vector3 point = contactPoint.point;
                Vector3 offset = Vector3.Scale(planeHit.normal, collider.bounds.extents);
                Vector3 point = collider.transform.position + offset;
                Debug.DrawRay(point, normal, Color.blue);

                // (x2, y2, z2) punto del vertice del objeto que pretendes mover
                auxVertex = transform.TransformPoint(vertex);

                // A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
                // D = -A*x - B*y - C*z Calculo la D de esta forma
                float d = Vector3.Dot(-normal, point);

                // A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
                num = Vector3.Dot(normal, auxVertex) + d;
                if (num != 0 && previousNum == 0) previousNum = num;

                Debug.Log(num * previousNum);
                // Si la siguiente operacion es negativa, significa que el vertice asociado al valor num esta al otro lado del plano
                if (num * previousNum < 0) return false;

                // Si algun valor tiene signo distinto a los demas, esta al otro lado del plano
                //if (num > 0 && previousNum < 0)
                //{
                //    //Debug.Log(vertex);
                //    return false;
                //}
                //if (num < 0 && previousNum > 0)
                //{
                //    //Debug.Log(vertex);
                //    return false;
                //}
            }
        }
        return true;
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

    public void SetTouchpadValues(float valueX, float valueY)
    {
        touchpadValueX = valueX;
        touchpadValueY = valueY;
    }

    /// <summary>
    /// Rotate the objects forward vector so its direction is the same as the perpendicular vector of the surface
    /// </summary>
    public void SetObjectRotation()
    {
        if (surfaceObject.CompareTag("Wall")) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(surfaceNormal, Vector3.up);
            transform.rotation = targetRotation;
        }
        else if (surfaceObject.CompareTag("Floor"))
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    public void MoveWithTouchpad()
    {
        Vector3 localMovement = Vector3.zero;

        if (surfaceObject.CompareTag("Wall")) localMovement = new Vector3(-touchpadValueX, touchpadValueY, 0f);
        else if (surfaceObject.CompareTag("Floor")) localMovement = new Vector3(-touchpadValueX, 0f, -touchpadValueY);

        // Convertir el desplazamiento local a coordenadas globales
        Vector3 worldMovement = transform.TransformDirection(localMovement);

        // Calcular la nueva posición sumando el desplazamiento a la posición actual
        Vector3 nextPosition = transform.position + worldMovement * 5f * Time.deltaTime;

        // Mover el objeto utilizando Rigidbody.MovePosition
        objectRigidbody.MovePosition(nextPosition);


        ////movement.Normalize();

        //Vector3 surfaceLocalMovement = Vector3.zero;

        ////Transformar surface normal a

        //if (surfaceNormal.x != 0)
        //{
        //    surfaceLocalMovement = new Vector3(0f, valueY, valueX);
        //}
        //else if (surfaceNormal.y != 0)
        //{
        //    surfaceLocalMovement = new Vector3(valueX, 0f, valueY);
        //}
        //else if (surfaceNormal.z != 0)
        //{
        //    surfaceLocalMovement = new Vector3(valueX, valueY, 0f);
        //}

        //Vector3 objectInSurfaceLocalPosition = surfaceObject.transform.InverseTransformPoint(transform.position);
        //Vector3 nextLocalPosition = objectInSurfaceLocalPosition + surfaceLocalMovement * 2f * Time.deltaTime;
        ////Vector3 worldMovement = surfaceObject.transform.TransformPoint(nextLocalPosition) * 5f * Time.deltaTime;
        ////Vector3 nextPosition = movement * 5f * Time.deltaTime;

        //objectRigidbody.MovePosition(surfaceObject.transform.TransformPoint(nextLocalPosition));
        //objectRigidbody.MovePosition(worldMovement);
    }

    //Con el trigger izquierdo se rota el objeto en el eje Y (30 grados)
    public void RotateObject()
    {
        //Debug.Log(transform.TransformPoint(boxCollider.bounds.extents));
        gameObject.transform.Rotate(Vector3.up, 30);
    }

    // Escala según el valor del eje Y del mando derecho (falta prohibir que se escale a menor o igual que 0)
    public void ScaleObject(float value)
    {
        float scaleAmount = value * Time.deltaTime;
        transform.localScale += Vector3.one * scaleAmount;
        Debug.Log(transform.TransformVector(boxCollider.size));
    }

    public void AssignMaterial(Material material)
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
