using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{
    //[SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    [SerializeField] Material[] collisionMaterials;

    [SerializeField] bool twoAxisScalling;

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

    public bool rotationLocked;

    public Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider
    public Vector3[] faces = new Vector3[6];

    public BoxCollider[] interactables;

    public BuildingManager _buildingManager;

    // Indica si se puede colocar el objeto o no
    public bool canPlace;

    public MeshRenderer meshRenderer;

    public BoxCollider boxCollider;

    public List<BoxCollider> colliderList = new List<BoxCollider>();

    public Rigidbody objectRigidbody;

    public bool isInteractable;

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

    //private void Update()
    //{
    //    if (_buildingManager.selectedBuildingObject != null && this == _buildingManager.selectedBuildingObject)
    //    {
    //        _buildingManager.cubos[0].transform.position = transform.TransformPoint(vertices[0]);
    //        _buildingManager.cubos[1].transform.position = transform.TransformPoint(vertices[1]);
    //        _buildingManager.cubos[2].transform.position = transform.TransformPoint(vertices[2]);
    //        _buildingManager.cubos[3].transform.position = transform.TransformPoint(vertices[3]);
    //        _buildingManager.cubos[4].transform.position = transform.TransformPoint(vertices[4]);
    //        _buildingManager.cubos[5].transform.position = transform.TransformPoint(vertices[5]);
    //        _buildingManager.cubos[6].transform.position = transform.TransformPoint(vertices[6]);
    //        _buildingManager.cubos[7].transform.position = transform.TransformPoint(vertices[7]);
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    if (/*collision.collider != _buildingManager.hit.collider && collision.collider.gameObject != _buildingManager.selectedBuildingObject.boxCollider.gameObject && */!detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Add(collider);
    //    }

    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Collider collider = collision.collider;
    //    if (detectedColliders.Contains(collider))
    //    {
    //        detectedColliders.Remove(collider);
    //    }
    //}

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

    void SetBoxFaces()
    {
        Vector3 centerTop = GetFaceMidpoint(vertices[0], vertices[1], vertices[2], vertices[3]);
        faces[0] = centerTop;
        Vector3 centerBottom = GetFaceMidpoint(vertices[4], vertices[5], vertices[6], vertices[7]);
        faces[1] = centerBottom;
        Vector3 centerFront = GetFaceMidpoint(vertices[0], vertices[1], vertices[5], vertices[4]);
        faces[2] = centerFront;
        Vector3 centerBack = GetFaceMidpoint(vertices[2], vertices[3], vertices[7], vertices[6]);
        faces[3] = centerBack;
        Vector3 centerLeft = GetFaceMidpoint(vertices[0], vertices[3], vertices[7], vertices[4]);
        faces[4] = centerLeft;
        Vector3 centerRight = GetFaceMidpoint(vertices[1], vertices[2], vertices[6], vertices[5]);
        faces[5] = centerRight;
    }

    /// <summary>
    /// Calculate the midpoint of a face given its four vertices
    /// </summary>
    Vector3 GetFaceMidpoint(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        return (v1 + v2 + v3 + v4) / 4.0f;
    }

    /// <summary>
    /// Rotate the objects forward vector so its direction is the same as the perpendicular vector of the surface
    /// </summary>
    public void SetForwardAxisDirection()
    {
        if (surfaceObject.CompareTag("Wall")) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(surfaceNormal, Vector3.up);
            transform.rotation = targetRotation;

            rotationLocked = true;
        }
        else if (surfaceObject.CompareTag("Floor") || surfaceObject.CompareTag("Ceiling"))
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);

            rotationLocked = false;
        }
    }

    public void SetTouchpadValues(float valueX, float valueY)
    {
        touchpadValueX = valueX;
        touchpadValueY = valueY;
    }

    /// <summary>
    /// Move the physics based object with the touchpad
    /// </summary>
    public void MoveWithTouchpad()
    {
        Vector3 worldMovement = Vector3.zero;

        if (surfaceObject.CompareTag("Wall"))
        {
            //Vector3 localMovement = Vector3.zero;

            Vector3 localMovement = new Vector3(-touchpadValueX, touchpadValueY, 0f);

            // Convertir el desplazamiento local a coordenadas globales
            worldMovement = transform.TransformDirection(localMovement);

            // Calcular la nueva posición sumando el desplazamiento a la posición actual
            // nextPosition = 5f * Time.deltaTime * worldMovement + transform.position;

            // Mover el objeto utilizando Rigidbody.MovePosition
            //objectRigidbody.MovePosition(nextPosition);
        }
        else if (surfaceObject.CompareTag("Floor") || surfaceObject.CompareTag("Ceiling"))
            worldMovement = new Vector3(touchpadValueX, 0f, touchpadValueY);

        Vector3 nextPosition = 5f * Time.deltaTime * worldMovement + transform.position;

        // Mover el objeto utilizando el rigidbody
        objectRigidbody.MovePosition(nextPosition);
    }

    /// <summary>
    /// Rotate the object in the Y axis
    /// </summary>
    public void RotateObject(float value)
    {
        //Debug.Log(transform.TransformPoint(boxCollider.bounds.extents));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + value, transform.eulerAngles.z);
        _buildingManager.parentObject.RotateObject(value);
    }

    // Scale object
    public void ScaleObject(Vector2 value, bool leftTriggerPressed)
    {
        //float scaleAmountY = 0f;
        //float scaleAmountX = 0f;

        Vector3 scaleAmount = Vector3.zero;

        if (twoAxisScalling == true && leftTriggerPressed)
        {
            // Y axis scaling depends on Y value of the touchpad
            if (value.y >= 0.8f || value.y <= -0.8f)
            {
                Debug.Log(value.y);
                //if (transform.localScale.y - scaleAmount.y >= 0.15f) scaleAmount.y = value.y * Time.deltaTime;
                //else scaleAmount.y += 0.15f - transform.localScale.y;
                scaleAmount.y = value.y * Time.deltaTime;
            }
            // X axis scaling depends on X value of the touchpad
            if (value.x >= 0.8f || value.x <= -0.8f)
            {
                //if (transform.localScale.x - scaleAmount.x >= 0.15f) scaleAmount.x = value.x * Time.deltaTime;
                //else scaleAmount.x += 0.15f - transform.localScale.x;
                scaleAmount.x = value.x * Time.deltaTime;
            }
        }
        else // Every axis is scaled depending on the Y value of the touchpad
        {
            //if (transform.localScale.x - scaleAmount.x >= 0.15f) scaleAmount.x = value.y * Time.deltaTime;
            //else scaleAmount.x += 0.15f - transform.localScale.x;

            //if (transform.localScale.y - scaleAmount.y >= 0.15f) scaleAmount.y = value.y * Time.deltaTime;
            //else scaleAmount.y += 0.15f - transform.localScale.y;

            //if (transform.localScale.z - scaleAmount.z >= 0.15f) scaleAmount.z = value.y * Time.deltaTime;
            //else scaleAmount.z += 0.15f - transform.localScale.z;

            scaleAmount.x = value.y * Time.deltaTime;
            scaleAmount.y = value.y * Time.deltaTime;
            scaleAmount.z = value.y * Time.deltaTime;
        }

        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + scaleAmount.x, 0.15f, 2f), Mathf.Clamp(transform.localScale.y + scaleAmount.y, 0.15f, 2f), Mathf.Clamp(transform.localScale.z + scaleAmount.z, 0.15f, 2f));
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

    public void DisableColliders()
    {
        foreach(BoxCollider subCollider in colliderList)
        {
            subCollider.enabled = false;
        }
    }
}
