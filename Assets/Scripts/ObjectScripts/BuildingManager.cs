using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingManager : MonoBehaviour
{
    // Posicion donde ray colisiona con un objeto de la escena
    public Vector3 _hitPos;

    // Raycast from right controller
    private XRRayInteractor ray;

    // Hit object from raycast
    // private RaycastHit hit;

    //private Collider _hitCollider;

    [SerializeField] GameObject localPlaneHit;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] private Material[] collisionMaterials;

    [SerializeField] private WorldMenuManager worldMenuManager;

    // -------------------------------------

    // TEST
    public GameObject[] cubos;
    //

    // Perpendicular vector to the normal of the RaycastHit that sets the direction fo the movement of an object with physics
    public Vector3 movementDirection;

    public RaycastHit hit;

    // Objeto instanciado que indica donde se colocara el modelo seleccionado
    public GameObject pendingObject;
    // Componente BuildingObject del objeto pendiente o del objeto del mundo seleccionado
    public BuildingObject selectedBuildingObject;

    public CollisionManager parentObject;

    public BuildingObject hitObject;
    
    // Mandos de realidad virtual
    public GameObject rightController;
    public GameObject leftController;

    public float rotateAmount;

    public Vector3 offset = Vector3.zero;

    private void Start()
    {
        ray = rightController.GetComponent<XRRayInteractor>();
    }

    // Para dibujar el bounding box del objeto elegido (o de su collider)
    void DrawBoundingBox(Bounds bounds)
    {
        // Get the center and extents of the bounds
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        // Calculate the corner positions of the bounding box
        Vector3 frontBottomLeft = center - extents;
        Vector3 frontBottomRight = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
        Vector3 frontTopLeft = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
        Vector3 frontTopRight = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
        Vector3 backBottomLeft = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
        Vector3 backBottomRight = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
        Vector3 backTopLeft = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
        Vector3 backTopRight = center + extents;

        // Draw the bounding box wireframe
        Debug.DrawLine(frontBottomLeft, frontBottomRight);
        Debug.DrawLine(frontBottomRight, frontTopRight);
        Debug.DrawLine(frontTopRight, frontTopLeft);
        Debug.DrawLine(frontTopLeft, frontBottomLeft);

        Debug.DrawLine(backBottomLeft, backBottomRight);
        Debug.DrawLine(backBottomRight, backTopRight);
        Debug.DrawLine(backTopRight, backTopLeft);
        Debug.DrawLine(backTopLeft, backBottomLeft);

        Debug.DrawLine(frontBottomLeft, backBottomLeft);
        Debug.DrawLine(frontBottomRight, backBottomRight);
        Debug.DrawLine(frontTopLeft, backTopLeft);
        Debug.DrawLine(frontTopRight, backTopRight);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Physics.Raycast(new Vector3(0, 5f, 0), Vector3.forward, out var auxHit))
        //{
        //    DrawBoundingBox(auxHit.collider.gameObject.GetComponent<MeshRenderer>().bounds);
        //    //DrawBoundingBox(auxHit.collider.bounds);
        //    //cubos[0].transform.position = new Vector3(auxHit.collider.transform.position.x + auxHit.collider.bounds.extents.x, auxHit.collider.transform.position.y, auxHit.collider.transform.position.z);
        //    //cubos[1].transform.position = new Vector3(auxHit.collider.transform.position.x, auxHit.collider.transform.position.y, auxHit.collider.transform.position.z + auxHit.collider.bounds.extents.z);
        //}
        //if (selectedBuildingObject != null)
        //if (ray.TryGetCurrent3DRaycastHit(out hit))
        //{
        //    //Debug.Log(hit.collider.gameObject.transform.InverseTransformDirection(hit.normal));
        //    _hitPos = hit.point;
        //}

        if (playerManager.state == PlayerState.isBuilding)
        {
            DrawBoundingBox(selectedBuildingObject.boxCollider.bounds);
            if (worldMenuManager.buildingState == BuildingState.withOffset)
            {
                //selectedBuildingObject.transform.position = _hitPos + Vector3.Scale(hit.normal, selectedBuildingObject.boxCollider.bounds.extents);
                parentObject.transform.position = _hitPos + Vector3.Scale(hit.normal, parentObject.boxCollider.bounds.extents);
                IsInLimit(hit.point);
                if (offset == Vector3.zero) selectedBuildingObject.transform.position = parentObject.transform.position;
                else selectedBuildingObject.transform.position = offset;
                //if (parentObject.canPlace == true) selectedBuildingObject.transform.position = parentObject.transform.position;

                    //UpdateOffset();
                    //selectedBuildingObject.transform.position = parentObject.transform.position + offset;
            }

            // Actualizar materiales de colision
            //UpdateMaterials();
        }
    }

    private void FixedUpdate()
    {
        //RaycastHit hit;

        if (ray.TryGetCurrent3DRaycastHit(out hit))
        {
            //Debug.Log(hit.collider.gameObject.transform.InverseTransformDirection(hit.normal));
            _hitPos = hit.point;
            //Debug.Log(hit.collider.transform.InverseTransformDirection(hit.normal));
            //movementDirection = Vector3.Cross(hit.normal, Vector3.left);
            //Debug.DrawRay(hit.point, movementDirection, Color.blue);
            //Debug.Log("Vector perpendicular: " + movementDirection);

            //if (playerManager.state == PlayerState.isBuilding) selectedBuildingObject.transform.position = _hitPos;

            //if (playerManager.state == PlayerState.isBuilding)
            //{
            //    Vector3 hitOffset = _hitPos + Vector3.Scale(hit.normal, selectedBuildingObject.boxCollider.bounds.extents);
            //    Vector3 direction = hitOffset - selectedBuildingObject.transform.position;
            //    direction.Normalize();
            //    Vector3 nextPosition = selectedBuildingObject.transform.position + direction * 5f * Time.deltaTime;
            //    selectedBuildingObject.objectRigidbody.MovePosition(nextPosition);
            //}
            //cubos[0].transform.position = hit.collider.bounds.max;
            //cubos[1].transform.position = hit.collider.bounds.min;
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log("Nombre de objeto que choca con rayo: " + hit.collider.gameObject.name);
            // Ajustar la posicion para que el centro del objeto este en el punto de colision
            //if (playerManager.state == PlayerState.isBuilding)
            //{
            //    _hitPos = hit.point /*+ GetOffset(hit.normal)*/;
            //    //parentObject.transform.position = _hitPos + GetOffset(hit.normal);
            //}

            // Activar el outline del objeto si esta siendo apuntado con el mando
            // If a BuildingObject is hit, we can select it
            if (playerManager.state == PlayerState.isFree && hit.collider.gameObject.TryGetComponent<BuildingObject>(out var auxObj))
            {
                if (hitObject != auxObj)
                {
                    //Debug.Log("Apunta a obj");
                    //if (hitObject != null) hitObject.switchOutline(false);
                    hitObject = auxObj;
                    //hitObject.switchOutline(true);
                }
            }
            else
            {
                //Debug.Log("No apunta a obj");
                if (hitObject != null)
                {
                    //Debug.Log("Outline desactivada");
                    //hitObject.switchOutline(false);
                    hitObject = null;
                }
            }
        }
    }

    public void IsInLimit(Vector3 planePoint)
    {
        //bool allSameSide = false;

        // Reset offset back to zero so it doesn't stack
        //Vector3 updatedOffset = Vector3.zero;
        offset = Vector3.zero;

        //Vector3 auxVertex;

        //BoxCollider auxCollider = collider as BoxCollider;
        //auxCollider.center = 
        //Vector3[] directions = { Vector3.up };

        //Vector3[] vertices = GetBoxVertices();
        foreach (Collider collider in parentObject.detectedColliders)
        {
            if (collider != hit.collider && collider.gameObject != selectedBuildingObject.boxCollider.gameObject)
            {
                Vector3 auxVertex;
                float previousNum = 0;
                float num = 0;

                Vector3 closestPoint = collider.ClosestPoint(parentObject.transform.position);
                Vector3 diff = closestPoint - parentObject.transform.position;
                Vector3 dir = diff.normalized;

                // El raycast a lo mejor hay que tirarlo desde objCollider.Raycast
                //if (Physics.BoxCast(selectedBuildingObject.transform.position, parentObject.boxCollider.bounds.extents, dir, out var hitt))
                if (Physics.Raycast(parentObject.transform.position, dir, out var planeHit))
                {
                    //Debug.Log(planeHit.normal);
                    Debug.DrawRay(planeHit.point, planeHit.normal, Color.blue);

                    foreach (Vector3 vertex in parentObject.vertices)
                    {
                        // (A, B, C) vector perpendicular al plano del objeto que esta quieto
                        //Vector3 normal = contactPoint.normal;
                        Vector3 normal = planeHit.normal;

                        // (x2, y2, z2) punto del vertice del objeto que pretendes mover
                        auxVertex = parentObject.transform.TransformPoint(vertex);

                        //// A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
                        //// D = -A*x - B*y - C*z Calculo la D de esta forma
                        //float d = Vector3.Dot(-normal, planePoint);

                        //// A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
                        //num = Vector3.Dot(normal, auxVertex) + d;
                        //if (num != 0 && previousNum == 0) previousNum = num;

                        ////Debug.Log(num * previousNum);
                        //// Si la siguiente operacion es negativa, significa que el vertice asociado al valor num esta al otro lado del plano
                        //if (num * previousNum < 0)
                        //{
                            //return false;
                        localPlaneHit.transform.position = planeHit.point;
                        Quaternion targetRotation = Quaternion.LookRotation(normal, Vector3.up);
                        localPlaneHit.transform.rotation = targetRotation;

                        //Vector3 localHitNormal = planeHit.transform.InverseTransformDirection(planeHit.normal);
                        Vector3 localVertex = localPlaneHit.transform.InverseTransformPoint(auxVertex);

                        if (localVertex.z < 0)
                        {
                            Vector3 localObjectPosition = localPlaneHit.transform.InverseTransformPoint(parentObject.transform.position);
                            Vector3 newLocalPosition = localObjectPosition + new Vector3 (0f, 0f, Mathf.Abs(localVertex.z));

                            Vector3 newPosition = localPlaneHit.transform.TransformPoint(newLocalPosition);
                            offset = newPosition;
                        }

                        //updatedOffset = Vector3.Scale(localHitNormal, localVertex);

                        //Vector3 planePointVertexDistance = planePoint - auxVertex;
                        //Vector3 planePointVertexOffset = Vector3.Scale(planeHit.normal, planePointVertexDistance);

                        // Actual distance between the center of both objects
                        //float actualDistance = GetAxis(planeHit.normal, Vector3.Scale(-planeHit.normal, parentObject.boxCollider.transform.InverseTransformPoint(collider.transform.position)));

                        //updatedOffset += planeHit.normal * (maxDistance - actualDistance);
                        //updatedOffset = planePointVertexOffset;

                        //return updatedOffset;
                        //return Vector3.zero;

                        //}

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
            }
        }
        //return true;
        //Debug.Log(updatedOffset);
        //return updatedOffset;
        //return parentObject.transform.position;
    }

    /// <summary>
    /// Si el objeto colisiona se le asigna un material rojo, si no se le asigna uno verde
    /// </summary>
    void UpdateMaterials()
    {
        if (selectedBuildingObject.canPlace)
        {
            selectedBuildingObject.AssignMaterial(collisionMaterials[0]);
        }
        else
        {
            selectedBuildingObject.AssignMaterial(collisionMaterials[1]);
        }
    }

    private float GetAxis(Vector3 normal, Vector3 vector)
    {
        // Encontrar el eje dominante de la normal
        float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        if (Mathf.Abs(normal.x) == maxAxis)
        {
            return vector.x;
        }
        else if (Mathf.Abs(normal.y) == maxAxis)
        {
            return vector.y;
        }
        else if (Mathf.Abs(normal.z) == maxAxis)
        {
            return vector.z;
        }

        return 0;
    }

    //private Vector3 GetOffset(Vector3 normal, BoxCollider boxCollider)
    //{
    //    // Encontrar el eje dominante de la normal
    //    float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

    //    // Calcular el desplazamiento necesario para que el centro del objeto este alineado con la superficie
    //    Vector3 hitOffset = Vector3.zero;
    //    if (Mathf.Abs(normal.x) == maxAxis)
    //    {
    //        hitOffset = normal * boxCollider.bounds.extents.x;
    //        //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.x;
    //    }
    //    else if (Mathf.Abs(normal.y) == maxAxis)
    //    {
    //        hitOffset = normal * boxCollider.bounds.extents.y;
    //        //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.y;
    //    }
    //    else if (Mathf.Abs(normal.z) == maxAxis)
    //    {
    //        hitOffset = normal * boxCollider.bounds.extents.z;
    //        //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.z;
    //    }

    //    return hitOffset;
    //}

    //private Vector3 GetOffset(RaycastHit planeHit, BuildingObject buildingObject)
    //{
    //    Vector3 hitOffset = Vector3.zero;
    //    Vector3 auxVertex;

    //    //float previousNum = 0;
    //    //float num = 0;

    //    foreach (Vector3 vertex in buildingObject.vertices)
    //    {
    //        // (A, B, C) vector perpendicular al plano del objeto que esta quieto
    //        Vector3 normal = planeHit.normal;

    //        // (x, y, z) un punto del plano del objeto que esta quieto en la direccion del vector perpendicular
    //        Vector3 point = Vector3.Scale(normal, planeHit.point);
    //        //Debug.DrawRay(point, normal, Color.blue);

    //        // (x2, y2, z2) punto del vertice del objeto que pretendes mover
    //        auxVertex = transform.TransformPoint(vertex);

    //        //// A*x + B*y + C*z + D = 0 ecuacion del plano del objeto que esta quieto
    //        //// D = -A*x - B*y - C*z Calculo la D de esta forma
    //        //float d = Vector3.Dot(-normal, point);

    //        //// A*x2 + B*y2 + C*z2 + D Es un numero con el que puedo saber si todos los vertices estan en un mismo lado del plano
    //        //if (num != 0) previousNum = num;
    //        //num = Vector3.Dot(normal, auxVertex) + d;

    //        //// Si algun valor tiene signo distinto a los demas, esta al otro lado del plano
    //        //if (num > 0 && previousNum < 0)
    //        //{
    //        //    //Debug.Log(vertex);
    //        //    hitOffset =
    //        //}
    //        //if (num < 0 && previousNum > 0)
    //        //{
    //        //    //Debug.Log(vertex);

    //        //}
    //    }

    //    return hitOffset;
    //}

    private void UpdateOffset()
    {
        // Reset offset back to zero so it doesn't stack
        offset = Vector3.zero;

        foreach (Collider objCollider in parentObject.detectedColliders)
        {
            if (/*objCollider != selectedBuildingObject.boxCollider &&*/ objCollider != hit.collider)
            {
                //Debug.Log(objCollider.name);
                Vector3 closestPoint = objCollider.ClosestPoint(parentObject.transform.position);
                Vector3 diff = closestPoint - parentObject.transform.position;
                Vector3 dir = diff.normalized;

                // El raycast a lo mejor hay que tirarlo desde objCollider.Raycast
                //if (Physics.BoxCast(selectedBuildingObject.transform.position, parentObject.boxCollider.bounds.extents, dir, out var hitt))
                if (Physics.Raycast(parentObject.transform.position, dir, out var hitt))
                {
                    Debug.Log(hitt.normal);
                    Debug.DrawRay(hitt.point, hitt.normal, Color.blue);
                    // Maximum distance between the center of both objects before they start clipping
                    float maxDistance = GetAxis(hitt.normal, objCollider.bounds.extents + parentObject.boxCollider.bounds.extents);
                    // Actual distance between the center of both objects
                    float actualDistance = GetAxis(hitt.normal, Vector3.Scale(-hitt.normal, parentObject.boxCollider.transform.InverseTransformPoint(objCollider.transform.position)));
                    //Debug.Log(maxDistance);
                    if (actualDistance < maxDistance)
                    {
                        offset += hitt.normal * (maxDistance - actualDistance);
                    }
                    //Debug.Log(dir);
                    //Debug.Log("closestPoint es:" + closestPoint);
                    //Debug.Log("hit.point es:" + hit.point);
                    //Debug.Log("La normal del rayo que golpea en closestPoint es:" + hitt.normal);

                    //Debug.DrawRay(parentObject.transform.position, dir, Color.red);
                    //Debug.DrawRay(hitt.point, hitt.normal, Color.blue);
                }
            }
        }
    }

    public void InstantiateModel(GameObject selectedModel)
    {
        //if (pendingObject != null) Destroy(pendingObject);

        // Instancia para el objeto indicador que se proyecta en el mundo
        pendingObject = Instantiate(selectedModel, _hitPos, transform.rotation);
        //pendingObject = Instantiate(selectedModel, Vector3.zero, transform.rotation, parentObject.transform);

        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();
        //selectedBuildingObject.transform.position = new Vector3(0, 50f, 0);
        selectedBuildingObject._buildingManager = this;

        if (worldMenuManager.buildingState == BuildingState.withPhysics)
        {
            selectedBuildingObject.surfaceNormal = hit.normal;
            selectedBuildingObject.surfaceObject = hit.collider.gameObject;
            selectedBuildingObject.SetObjectRotation();
            selectedBuildingObject.objectRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
        }

        // Guardamos su material en la lista de materiales de colision
        //collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;

        // Match the scale of the colliders
        parentObject.SetScale(selectedBuildingObject);
    }

    // FUNCIONES LLAMADAS EN PlayerActions

    // Colocar el objeto pendiente en la posicion indicada
    public void PlaceObject()
    {
        // Volvemos a asignarle su material original
        //selectedBuildingObject.assignMaterial(collisionMaterials[2]);

        //// Instanciamos el objeto pendiente de colocacion
        //if (pendingObject != null)
        //{
        //    GameObject obj = Instantiate(pendingObject, _hitPos, transform.rotation);

        //    obj.GetComponent<BoxCollider>().isTrigger = false;
        //}
        //else 
        //{
        //selectedBuildingObject.boxCollider.isTrigger = false;

        // Change the layer to Default so the Raycast can interact with the object
        selectedBuildingObject.gameObject.layer = LayerMask.NameToLayer("Default");

        // Change to kinematic rigidbody so OnCollisionStay isn't called
        selectedBuildingObject.objectRigidbody.isKinematic = true;
        selectedBuildingObject.isPlaced = true;

        if (worldMenuManager.buildingState == BuildingState.withPhysics)
            selectedBuildingObject.objectRigidbody.constraints = RigidbodyConstraints.FreezePosition;

        // "Soltamos" el objeto seleccionado
        selectedBuildingObject = null;
        //}

        pendingObject = null;
        //offset = new Vector3(0, 0, 0);

        Debug.Log(parentObject.boxCollider.bounds.extents);

        // Reset the transform of the collision manager
        parentObject.Reset();
    }

    // Cancelar la colocacion del objeto pendiente
    public void CancelObjectPlacement()
    {
        Destroy(pendingObject);
        pendingObject = null;
        selectedBuildingObject = null;

        //worldMenuManager.selectedModel = null;
        // Reset the transform of the collision manager
        parentObject.Reset();
    }

    // Parar la colocacion del objeto pendiente
    //public void StopObjectPlacement()
    //{
    //    //pendingObject = null;
    //    selectedBuildingObject.gameObject.SetActive(false);
    //}

    // Seleccion de objeto en el mundo
    public void SelectObject()
    {
        if (hitObject != null)
        {
            //var auxTransform = hitObject.gameObject.transform;
            hitObject.SavePreviousTransform();

            selectedBuildingObject = hitObject;
            // Guardamos su material en la lista de materiales de colision
            //collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;
            // Activamos isTrigger para que no haya conflicto con el Raycast
            //selectedBuildingObject.boxCollider.isTrigger = true;

            // Change the layer to Ignore Raycast so the Raycast can't interact with the object
            selectedBuildingObject.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            // Change to non-kinematic rigidbody so OnCollisionStay is called
            selectedBuildingObject.objectRigidbody.isKinematic = false;
            selectedBuildingObject.isPlaced = false;

            if (worldMenuManager.buildingState == BuildingState.withPhysics)
                selectedBuildingObject.objectRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;

            // Match the scale of the colliders
            parentObject.SetScale(selectedBuildingObject);
        }
    }

    // Cancelar la transformacion del objeto del mundo seleccionado
    public void CancelObjectTransform()
    {
        if (worldMenuManager.buildingState == BuildingState.withPhysics)
            selectedBuildingObject.objectRigidbody.constraints = RigidbodyConstraints.FreezePosition;

        var auxObj = selectedBuildingObject;

        selectedBuildingObject = null;

        // Change the layer to Default so the Raycast can interact with the object
        auxObj.gameObject.layer = LayerMask.NameToLayer("Default");

        // Change to kinematic rigidbody so OnCollisionStay isn't called
        auxObj.objectRigidbody.isKinematic = true;
        auxObj.isPlaced = true;

        auxObj.SetPreviousTransform();
        // Volvemos a asignarle su material original
        //auxObj.assignMaterial(collisionMaterials[2]);
        // Desactivamos isTrigger para que no haya conflicto con el Raycast
        //auxObj.boxCollider.isTrigger = false;

        // Reset the transform of the collision manager
        parentObject.Reset();
    }
}
