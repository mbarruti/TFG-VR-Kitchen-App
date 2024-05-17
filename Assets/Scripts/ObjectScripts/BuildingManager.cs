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
    public Vector3 newPosition;

    private void Start()
    {
        ray = playerManager.mainController.GetComponent<XRRayInteractor>();
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
        //parentObject.transform.position = new Vector3(10f, 0f, 0f);
        //if (Physics.Raycast(new Vector3(0, 1.78f, 0), Vector3.forward, out var auxHit))
        //{
        //    Debug.Log(auxHit.collider.gameObject.name);
        //    //    DrawBoundingBox(auxHit.collider.gameObject.GetComponent<MeshRenderer>().bounds);
        //    //    //DrawBoundingBox(auxHit.collider.bounds);
        //    //    //cubos[0].transform.position = new Vector3(auxHit.collider.transform.position.x + auxHit.collider.bounds.extents.x, auxHit.collider.transform.position.y, auxHit.collider.transform.position.z);
        //    //    //cubos[1].transform.position = new Vector3(auxHit.collider.transform.position.x, auxHit.collider.transform.position.y, auxHit.collider.transform.position.z + auxHit.collider.bounds.extents.z);
        //    //}
        //    //if (selectedBuildingObject != null)
        //    //if (ray.TryGetCurrent3DRaycastHit(out hit))
        //    //{
        //    //    //Debug.Log(hit.collider.gameObject.transform.InverseTransformDirection(hit.normal));
        //    //    _hitPos = hit.point;
        //}

        if (playerManager.state == PlayerState.isBuilding)
        {
            DrawBoundingBox(selectedBuildingObject.boxCollider.bounds);

            // Check for rotations if not locked
            if (selectedBuildingObject.rotationLocked == false)
            {
                if (playerManager.rightGripPressed == true) selectedBuildingObject.RotateObject(-1f);
                else if (playerManager.leftGripPressed == true) selectedBuildingObject.RotateObject(1f);
            }

            if (worldMenuManager.buildingState == BuildingState.withOffset)
            {
                parentObject.transform.position = _hitPos;
                //parentObject.transform.position = SetFirstObjectPosition();
                parentObject.transform.position = UpdateOffset(SetHitObjectOffset(hit), 0f);

                //if (selectedBuildingObject.canPlace == true)
                //{
                if (selectedBuildingObject.canPlace == true) selectedBuildingObject.transform.position = parentObject.transform.position;
                //}

                //if (parentObject.canPlace == true)
                //{
                //    if (selectedBuildingObject.canPlace == true)
                //    {
                //        selectedBuildingObject.transform.position = parentObject.transform.position;
                //    }
                //    //Debug.Log("entra");
                //    parentObject.transform.position = _hitPos;
                //    parentObject.transform.position = SetFirstObjectPosition();
                //    parentObject.transform.position = UpdateOffset(hit.point);
                //}
                //else
                //{
                //    //Debug.Log("entra");
                //    if (parentObject.detectedColliders.Contains(hit.collider)) SetFirstObjectPosition();
                //    parentObject.transform.position = UpdateOffset(hit.point);
                //}

                //UpdateOffset(hit.point);
                //selectedBuildingObject.transform.position = parentObject.transform.position + offset;
                //if (newPosition == Vector3.zero) selectedBuildingObject.transform.position = parentObject.transform.position;
                //else selectedBuildingObject.transform.position = newPosition;
                //if (parentObject.canPlace == true) selectedBuildingObject.transform.position = parentObject.transform.position;

                //UpdateOffset();
                //selectedBuildingObject.transform.position = parentObject.transform.position + offset;
            }
            else if (worldMenuManager.buildingState == BuildingState.withTrigger)
            {
                parentObject.transform.position = _hitPos;

                selectedBuildingObject.transform.position = SetHitObjectOffset(hit);
            }

            // Actualizar materiales de colision
            //UpdateMaterials();
        }

        if (playerManager.state == PlayerState.isFree)
        {
            // If there is an interactable selected, the interaction happens
            if (playerManager.selectedInteractable != null)
            {
                if (playerManager.selectedInteractable.rotationInteractable == true)
                {
                    playerManager.selectedInteractable.RotateInteraction(playerManager.mainController);
                }
                else
                {
                    playerManager.selectedInteractable.MoveInteraction(playerManager.mainController);
                }
            }
        }
    }

    // Try with mouse
    //private void Update()
    //{
    //    if (playerManager.state == PlayerState.isBuilding)
    //    {
    //        DrawBoundingBox(selectedBuildingObject.boxCollider.bounds);
    //        if (worldMenuManager.buildingState == BuildingState.withOffset)
    //        {
    //            if (Input.GetMouseButton(0))
    //            {
    //                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //                RaycastHit mouseHit;

    //                if (Physics.Raycast(ray, out mouseHit))
    //                {
    //                    parentObject.transform.position = mouseHit.point;
    //                    //parentObject.transform.position = SetFirstObjectPosition(mouseHit);
    //                    parentObject.transform.position = UpdateOffset(SetFirstObjectPosition(mouseHit), 0f);

    //                    if (selectedBuildingObject.canPlace == true) selectedBuildingObject.transform.position = parentObject.transform.position;
    //                }
    //            }
    //        }
    //    }
    //}

    private void FixedUpdate()
    {
        //RaycastHit hit;

        //parentObject.detectedColliders2 = Physics.OverlapBox(parentObject.transform.position, parentObject.transform.localScale / 2, parentObject.transform.rotation);

        // parentObject.transform.position = new Vector3(10f, 10f, 0f);
        //parentObject.detectedColliders2 = Physics.OverlapBox(parentObject.transform.position, parentObject.transform.localScale / 2, parentObject.transform.rotation);
        //if (parentObject.detectedColliders2.Length == 2) Debug.Log(parentObject.detectedColliders2.Length);
        //parentObject.transform.position = new Vector3(10f, 10f, 0f);

        //Debug.Log(parentObject.detectedColliders2.Length);

        //Collider[] lista = 

        if (ray.TryGetCurrent3DRaycastHit(out hit))
        {
            //Debug.Log(hit.collider.gameObject.transform.InverseTransformDirection(hit.normal));
            _hitPos = hit.point;

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

    //public Vector3 UpdateOffset2(Vector3 parentPos, float counter)
    //{
    //    // Limit of the amount of callings of this recursive function
    //    if (counter > 40) return parentPos; //40 empieza a lagear, 45 lagea un poco, 50 lagea un poco mas, 60 lagea mucho

    //    // Set the first position
    //    parentObject.transform.position = parentPos;

    //    List<Vector3> directionsList;

    //    // Get the colliders colliding with the object in its position;
    //    Collider[] colliderList;
    //    colliderList = Physics.OverlapBox(parentObject.transform.position, parentObject.transform.localScale / 2, parentObject.transform.rotation);

    //    foreach (Collider collider in colliderList)
    //    {
    //        if (/*collider != hit.collider && */collider.gameObject != selectedBuildingObject.boxCollider.gameObject && collider.gameObject != parentObject.gameObject)
    //        {
    //            Vector3 closestPoint = collider.ClosestPoint(selectedBuildingObject.transform.position);
    //            Vector3 diff = closestPoint - selectedBuildingObject.transform.position;
    //            Vector3 dir = diff.normalized;

    //            if (Physics.Raycast(selectedBuildingObject.transform.position, dir, out var planeHit))
    //            {
    //                Debug.DrawRay(planeHit.point, planeHit.normal, Color.blue);

    //                localPlaneHit.transform.position = planeHit.point;
    //                Quaternion targetRotation = Quaternion.LookRotation(planeHit.normal, Vector3.up);
    //                localPlaneHit.transform.rotation = targetRotation;

    //                directionsList.Add(localPlaneHit);
    //            }
    //        }
    //    }

    //    return parentObject.transform.position;
    //}

    public Vector3 UpdateOffset(Vector3 parentPos, float counter)
    {
        // Limit of the amount of callings of this recursive function
        if (counter > 40) return parentPos; //40 empieza a lagear, 45 lagea un poco, 50 lagea un poco mas, 60 lagea mucho

        parentObject.transform.position = parentPos;

        //Vector3 firstPosition = SetFirstObjectPosition();

        offset = Vector3.zero;
        Vector3 nextLocalObjectPosition = parentObject.transform.position;
        //Vector3 nextLocalObjectPosition = Vector3.zero;

        List<(Vector3, Vector3)> planesNormals = new List<(Vector3, Vector3)>(); // (Normal of the plane, Offset in the direction of the normal)

        //parentObject.transform.position = SetFirstObjectPosition();
        Collider[] colliderList;
        colliderList = Physics.OverlapBox(parentObject.transform.position, parentObject.globalColliderSize / 2, parentObject.transform.rotation);
        foreach (Collider collider in colliderList)
        //foreach (Collider collider in parentObject.detectedColliders)
        {
            if (/*collider != hit.collider &&   If not ignored, doesn't work in small angles*/ collider.gameObject != selectedBuildingObject.boxCollider.gameObject && collider.gameObject != parentObject.gameObject)
            {
                //Vector3 closestPoint = collider.ClosestPoint(selectedBuildingObject.transform.position);
                //Vector3 diff = closestPoint - selectedBuildingObject.transform.position;
                //Vector3 dir = diff.normalized;

                Vector3 objectCenter = selectedBuildingObject.transform.TransformPoint(selectedBuildingObject.boxCollider.center);
                Vector3 closestPoint = collider.ClosestPoint(objectCenter);
                Vector3 diff = closestPoint - objectCenter;
                Vector3 dir = diff.normalized;

                //if (Physics.BoxCast(selectedBuildingObject.transform.position, parentObject.boxCollider.bounds.extents, dir, out var hitt))
                //if (Physics.Raycast(selectedBuildingObject.transform.position, dir, out var planeHit))
                Ray ray = new Ray(selectedBuildingObject.transform.position, dir);
                if (collider.Raycast(ray, out RaycastHit planeHit, 100f))
                {
                    Debug.DrawRay(closestPoint, planeHit.normal, Color.blue);

                    //if (planeHit.normal == -hit.normal) return parentObject.transform.position;

                    localPlaneHit.transform.position = closestPoint;
                    Quaternion targetRotation = Quaternion.LookRotation(planeHit.normal, Vector3.up);
                    //targetRotation = Quaternion.LookRotation(planeHit.normal, Vector3.up);
                    localPlaneHit.transform.rotation = targetRotation;

                    //Vector3 nextLocalPlanePosition = GetNextPosition(localPlaneHit);
                    Vector3 localPlaneOffset = GetNextPosition(localPlaneHit);

                    //(Vector3, Vector3) planeInfo = HasCorrectVertices(planesNormals, planeHit.normal, localPlaneOffset);

                    //if (planeInfo.Item1 != Vector3.zero && planeInfo.Item2 != Vector3.zero)
                    ////if (/*localPlaneOffset != Vector3.zero &&*/ planesNormals.Count > 0)
                    ////if (parentObject.detectedColliders.Count > 3)
                    //{
                    //    Debug.Log("entra");
                    //    planesNormals.Remove((planeInfo.Item1, planeInfo.Item2));
                    //    planesNormals.Add((planeHit.normal, localPlaneOffset));

                    //    // Collision manager position in the local coordinates of the plane
                    //    Vector3 localPlaneObjectPosition = localPlaneHit.transform.InverseTransformPoint(parentObject.transform.position);
                    //    // Sum the offset of the distance between vertex and plane point in the Z axis to the collision manager position
                    //    Vector3 nextLocalPlanePosition = localPlaneObjectPosition + new Vector3(0f, 0f, Mathf.Abs(localPlaneOffset.z));
                    //    // Next position in world coordinates
                    //    Vector3 worldPosition = localPlaneHit.transform.TransformPoint(nextLocalPlanePosition);
                    //    // Change world position to the collision manager coordinates
                    //    // This way we get the offset needed to add in world coordinates through the collision manager coordinates
                    //    nextLocalObjectPosition += parentObject.transform.InverseTransformPoint(worldPosition);

                    //}
                    //else if (planeInfo.Item1 == Vector3.zero)
                    //{
                    //if (localPlaneOffset.z < 0f)
                    if (Mathf.Abs(localPlaneOffset.z) > 0.0001f)
                    //if (!Mathf.Approximately(localPlaneOffset.z, 0f))
                    //if (localPlaneOffset != Vector3.zero)
                    //if (localPlaneOffset.z + 0f < 0)
                    //if (localPlaneOffset.z * localPlaneOffset.y < 0f)
                    {
                        //Debug.Log(localPlaneOffset.z * localPlaneOffset.y);
                        selectedBuildingObject.canPlace = false;
                        parentObject.canPlace = false;

                        //planesNormals.Add((planeHit.normal, localPlaneOffset));
                        // Collision manager position in the local coordinates of the plane
                        Vector3 localPlaneObjectPosition = localPlaneHit.transform.InverseTransformPoint(parentObject.transform.position);
                        // Sum the offset of the distance between vertex and plane point in the Z axis to the collision manager position
                        Vector3 nextLocalPlanePosition = localPlaneObjectPosition + new Vector3(0f, 0f, Mathf.Abs(localPlaneOffset.z));
                        // Next position in world coordinates
                        Vector3 worldPosition = localPlaneHit.transform.TransformPoint(nextLocalPlanePosition);
                        // Change world position to the collision manager coordinates
                        // This way we get the offset needed to add in world coordinates through the collision manager coordinates
                        //nextLocalObjectPosition += parentObject.transform.InverseTransformPoint(worldPosition);
                        //nextLocalObjectPosition = worldPosition;
                        //parentObject.transform.position = worldPosition;
                        parentObject.transform.position = UpdateOffset(worldPosition, counter+1);
                        return parentObject.transform.position;
                    }
                }
            }
        }
        selectedBuildingObject.canPlace = true;
        parentObject.canPlace = true;
        return parentObject.transform.position;
        //selectedBuildingObject.transform.position = nextLocalObjectPosition;
        //offset = nextLocalObjectPosition;
        //Debug.Log(offset);
    }

    private Vector3 SetHitObjectOffset(RaycastHit auxHit)
    {
        //localPlaneHit.transform.position = _hitPos;
        localPlaneHit.transform.position = auxHit.point;
        Quaternion targetRotation = Quaternion.LookRotation(auxHit.normal, Vector3.up);
        localPlaneHit.transform.rotation = targetRotation;

        float selectedLocalVertex = 0f;

        foreach (Vector3 vertex in parentObject.vertices)
        {
            // Current vertex in world coordinates
            Vector3 worldVertex = parentObject.transform.TransformPoint(vertex);

            // Current vertex in the local coordinates of the plane
            Vector3 localVertex = localPlaneHit.transform.InverseTransformPoint(worldVertex);

            // If the current vertex is in the other side of the plane and farther than de selected vertex,
            //the new selected vertex is the current one
            if (localVertex.z < 0f && Mathf.Abs(localVertex.z) > 0.0001f && localVertex.z < selectedLocalVertex)
            {
                selectedLocalVertex = localVertex.z;
            }
        }

        // Collision manager position in the local coordinates of the plane
        Vector3 hitPlaneObjectPosition = localPlaneHit.transform.InverseTransformPoint(parentObject.transform.position);
        // Sum the offset of the distance between vertex and plane point in the Z axis to the collision manager position
        Vector3 nextHitPlanePosition = hitPlaneObjectPosition + new Vector3(0f, 0f, Mathf.Abs(selectedLocalVertex));
        // Next position in world coordinates
        Vector3 worldPosition = localPlaneHit.transform.TransformPoint(nextHitPlanePosition);
        // Change world position to the collision manager coordinates
        // This way we get the offset needed to add in world coordinates through the collision manager coordinates
        //Vector3 localObjectPosition = parentObject.transform.InverseTransformPoint(worldPosition);

        return worldPosition;
    }

    private Vector3 GetNextPosition(GameObject plane)
    {
        //Vector3 nextLocalPosition = Vector3.zero;
        float selectedLocalVertex = 0f;

        float positiveLocalVertex = 0f;

        // Iterate through the vertices of the collision manager
        foreach (Vector3 vertex in parentObject.vertices)
        {
            // Current vertex in world coordinates
            Vector3 worldVertex = parentObject.transform.TransformPoint(vertex);

            // Current vertex in the local coordinates of the plane
            Vector3 localVertex = plane.transform.InverseTransformPoint(worldVertex);

            // If the current vertex is in the other side of the plane and farther than de selected vertex,
            //the new selected vertex is the current one
            //if (localVertex.z < 0f && localVertex.z < selectedLocalVertex)
            if (localVertex.z < 0f && Mathf.Abs(localVertex.z) > 0.0001f && localVertex.z < selectedLocalVertex)
            {
                selectedLocalVertex = localVertex.z;

                // Collision manager position in the local coordinates of the plane
                //Vector3 localObjectPosition = plane.transform.InverseTransformPoint(parentObject.transform.position);
                // Sum the offset of the distance between vertex and plane point in the Z axis to the collision manager position
                //nextLocalPosition = localObjectPosition + new Vector3(0f, 0f, Mathf.Abs(selectedLocalVertex));


                //Vector3 nextPosition = plane.transform.TransformPoint(newLocalPosition);
                //return nextLocalPosition;
            }
            else if (localVertex.z > 0f)
            {
                positiveLocalVertex = localVertex.z;
            }
        }
        return new Vector3(0f, positiveLocalVertex, selectedLocalVertex);
    }

    private (Vector3, Vector3) HasCorrectVertices(List<(Vector3, Vector3)> planesNormals, Vector3 planeHitNormal, Vector3 localPlaneOffset)
    {
        if (planesNormals.Count == 0) return (Vector3.zero, Vector3.zero);

        foreach ((Vector3 planeNormal, Vector3 planeOffset) in planesNormals)
        {
            if (planeHitNormal == planeNormal)
            {
                if (localPlaneOffset.z > planeOffset.z) return (planeNormal, planeOffset);
                if (localPlaneOffset.z < planeOffset.z) return (planeNormal, Vector3.zero);
            }
        }
        return (Vector3.zero, Vector3.zero);
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

    public void InstantiateModel(GameObject selectedModel)
    {
        //if (pendingObject != null) Destroy(pendingObject);

        // Instancia para el objeto indicador que se proyecta en el mundo
        pendingObject = Instantiate(selectedModel, _hitPos, selectedModel.transform.rotation);
        //pendingObject = Instantiate(selectedModel, Vector3.zero, transform.rotation, parentObject.transform);

        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();
        //selectedBuildingObject.transform.position = new Vector3(0, 50f, 0);
        selectedBuildingObject._buildingManager = this;

        if (worldMenuManager.buildingState == BuildingState.withOffset)
        {
            selectedBuildingObject.DisableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withTrigger)
        {
            //selectedBuildingObject.boxCollider.isTrigger = true;
            selectedBuildingObject.DisableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withPhysics)
        {
            // Set the rotation of the object so its local Z axis points at the same direction as the normal hit
            selectedBuildingObject.surfaceNormal = hit.normal;
            selectedBuildingObject.surfaceObject = hit.collider.gameObject;
            selectedBuildingObject.SetForwardAxisDirection();

            selectedBuildingObject.objectRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
        }

        // Guardamos su material en la lista de materiales de colision
        //collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;

        // Match the scale of the colliders
        parentObject.SetScale(selectedBuildingObject);
        parentObject.SetRotation(selectedBuildingObject);
    }

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
        
        //if (selectedBuildingObject.canBePlaced) // Por ahora este if solo es para el estado withTrigger
        //{
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

            // Reset the transform of the collision manager
            parentObject.Reset();
        //}
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

            if (worldMenuManager.buildingState == BuildingState.withOffset)
            {
                selectedBuildingObject.boxCollider.enabled = true;
                selectedBuildingObject.DisableColliders();
            }
            else if (worldMenuManager.buildingState == BuildingState.withTrigger)
            {
                //selectedBuildingObject.boxCollider.enabled = true;
                //selectedBuildingObject.boxCollider.isTrigger = true;
                selectedBuildingObject.DisableColliders();
            }
            else if (worldMenuManager.buildingState == BuildingState.withPhysics)
                selectedBuildingObject.objectRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;

            // Match the scale of the colliders
            parentObject.SetScale(selectedBuildingObject);
            parentObject.SetRotation(selectedBuildingObject);
        }
    }

    // Cancelar la transformacion del objeto del mundo seleccionado
    public void CancelObjectTransform()
    {
        if (worldMenuManager.buildingState == BuildingState.withOffset)
        {
            selectedBuildingObject.boxCollider.enabled = false;
            selectedBuildingObject.EnableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withTrigger)
        {
            //selectedBuildingObject.boxCollider.enabled = false;
            //selectedBuildingObject.boxCollider.isTrigger = false;
            selectedBuildingObject.EnableColliders();
        }
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

    /// <summary>
    /// Destroy the object from the world
    /// </summary>
    public void DestroyObject()
    {
        if (hitObject != null)
        {
            Destroy(hitObject.gameObject);
        }
    }
}
