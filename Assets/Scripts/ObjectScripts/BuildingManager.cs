using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingManager : MonoBehaviour
{
    // Posicion donde ray colisiona con un objeto de la escena
    public Vector3 _hitPos;

    // Hit object from raycast
    // private RaycastHit hit;

    //private Collider _hitCollider;

    [SerializeField] DataManager dataManager;

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
    public GameObject objectHit;

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

    // Raycast from main controller
    public XRRayInteractor ray;

    private void Start()
    {
        ray = playerManager.mainController.GetComponent<XRRayInteractor>();
    }

    //// Para dibujar el bounding box del objeto elegido (o de su collider)
    //void DrawBoundingBox(Bounds bounds)
    //{
    //    // Get the center and extents of the bounds
    //    Vector3 center = bounds.center;
    //    Vector3 extents = bounds.extents;

    //    // Calculate the corner positions of the bounding box
    //    Vector3 frontBottomLeft = center - extents;
    //    Vector3 frontBottomRight = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
    //    Vector3 frontTopLeft = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
    //    Vector3 frontTopRight = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
    //    Vector3 backBottomLeft = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
    //    Vector3 backBottomRight = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
    //    Vector3 backTopLeft = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
    //    Vector3 backTopRight = center + extents;

    //    // Draw the bounding box wireframe
    //    Debug.DrawLine(frontBottomLeft, frontBottomRight);
    //    Debug.DrawLine(frontBottomRight, frontTopRight);
    //    Debug.DrawLine(frontTopRight, frontTopLeft);
    //    Debug.DrawLine(frontTopLeft, frontBottomLeft);

    //    Debug.DrawLine(backBottomLeft, backBottomRight);
    //    Debug.DrawLine(backBottomRight, backTopRight);
    //    Debug.DrawLine(backTopRight, backTopLeft);
    //    Debug.DrawLine(backTopLeft, backBottomLeft);

    //    Debug.DrawLine(frontBottomLeft, backBottomLeft);
    //    Debug.DrawLine(frontBottomRight, backBottomRight);
    //    Debug.DrawLine(frontTopLeft, backTopLeft);
    //    Debug.DrawLine(frontTopRight, backTopRight);
    //}

    // Update is called once per frame
    void Update()
    {
        if (playerManager.state == PlayerState.isBuilding)
        {
            //DrawBoundingBox(selectedBuildingObject.boxCollider.bounds);

            // Check for rotations if not locked
            if (selectedBuildingObject.rotationLocked == false && worldMenuManager.continuousRotation == true)
            {
                if (playerManager.rightGripPressed == true) selectedBuildingObject.RotateObject(-1f);
                else if (playerManager.leftGripPressed == true) selectedBuildingObject.RotateObject(1f);
            }

            if (worldMenuManager.buildingState == BuildingState.withOffset)
            {
                parentObject.transform.position = _hitPos;

                parentObject.transform.position = UpdateOffset(SetHitObjectOffset(hit), 0f);

                if (selectedBuildingObject.canPlace == true) selectedBuildingObject.transform.position = parentObject.transform.position;

            }
            else if (worldMenuManager.buildingState == BuildingState.withTrigger)
            {
                parentObject.transform.position = _hitPos;

                selectedBuildingObject.transform.position = SetHitObjectOffset(hit);
            }
        }

        if (playerManager.state == PlayerState.isFree)
        {
            if (playerManager.rightGripPressed && playerManager.mainController.Equals(rightController))
            {
                playerManager.rightControllerRay.raycastMask = playerManager.newLayerMask;

                // If there is not an interactable selected and one is hit while the grip is pressed, it is selected
                if (playerManager.selectedInteractable == null && playerManager.rightControllerRay.TryGetCurrent3DRaycastHit(out RaycastHit auxHit))
                {
                    auxHit.collider.gameObject.TryGetComponent<Interactable>(out playerManager.selectedInteractable);
                    if (playerManager.selectedInteractable != null) playerManager.selectedInteractable.interactingControllerPosition = rightController.transform.localPosition;
                }
            }
            else if (playerManager.leftGripPressed && playerManager.mainController.Equals(leftController))
            {
                playerManager.leftControllerRay.raycastMask = playerManager.newLayerMask;

                // If there is not an interactable selected and one is hit while the grip is pressed, it is selected
                if (playerManager.selectedInteractable == null && playerManager.leftControllerRay.TryGetCurrent3DRaycastHit(out RaycastHit auxHit))
                {
                    auxHit.collider.gameObject.TryGetComponent<Interactable>(out playerManager.selectedInteractable);
                    if (playerManager.selectedInteractable != null) playerManager.selectedInteractable.interactingControllerPosition = leftController.transform.localPosition;
                }
            }

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

    private void FixedUpdate()
    {
        if (ray.TryGetCurrent3DRaycastHit(out hit))
        {
            //objectHit = hit.collider.gameObject;
            //Debug.Log(hit.collider.gameObject.transform.InverseTransformDirection(hit.normal));
            _hitPos = hit.point;

            // Activar el outline del objeto si esta siendo apuntado con el mando
            // If a BuildingObject is hit, we can select it
            if (playerManager.state == PlayerState.isFree && hit.collider != null)
            {
                BuildingObject auxObj = FindBuildingObject(hit.collider.transform);

                if (auxObj != null)
                {
                    if (hitObject != auxObj)
                    {
                        //Debug.Log("Apunta a obj");
                        //if (hitObject != null) hitObject.switchOutline(false);
                        hitObject = auxObj;
                        //hitObject.switchOutline(true);
                    }
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

    public Vector3 UpdateOffset(Vector3 parentPos, float counter)
    {
        // Limit of the amount of callings of this recursive function
        if (counter > 40) return parentPos; //40 empieza a lagear, 45 lagea un poco, 50 lagea un poco mas, 60 lagea mucho

        parentObject.transform.position = parentPos;

        offset = Vector3.zero;

        Collider[] colliderList;
        colliderList = Physics.OverlapBox(parentObject.transform.position, parentObject.globalColliderSize / 2, parentObject.transform.rotation);
        foreach (Collider collider in colliderList)
        {
            if (/*collider != hit.collider  &&If not ignored, doesn't work in small angles*/ collider.gameObject != selectedBuildingObject.boxCollider.gameObject && collider.gameObject != parentObject.gameObject)
            {
                Vector3 objectCenter = selectedBuildingObject.transform.TransformPoint(selectedBuildingObject.boxCollider.center);
                Vector3 closestPoint = collider.ClosestPoint(objectCenter);
                Vector3 diff = closestPoint - objectCenter;
                Vector3 dir = diff.normalized;

                Ray ray = new Ray(selectedBuildingObject.transform.position, dir);
                //ray.direction = ray.direction.normalized;

                if (collider.Raycast(ray, out RaycastHit planeHit, 100f))
                {
                    Debug.DrawRay(closestPoint, planeHit.normal, Color.blue);


                    localPlaneHit.transform.position = closestPoint;
                    Quaternion targetRotation = Quaternion.LookRotation(planeHit.normal, Vector3.up);
                    //targetRotation = Quaternion.LookRotation(planeHit.normal, Vector3.up);
                    localPlaneHit.transform.rotation = targetRotation;

                    //Vector3 nextLocalPlanePosition = GetNextPosition(localPlaneHit);
                    Vector3 localPlaneOffset = GetNextPosition(localPlaneHit);

                    if (Mathf.Abs(localPlaneOffset.z) > 0.0001f)
                    {
                        selectedBuildingObject.canPlace = false;
                        parentObject.canPlace = false;

                        // Collision manager position in the local coordinates of the plane
                        Vector3 localPlaneObjectPosition = localPlaneHit.transform.InverseTransformPoint(parentObject.transform.position);
                        // Sum the offset of the distance between vertex and plane point in the Z axis to the collision manager position
                        Vector3 nextLocalPlanePosition = localPlaneObjectPosition + new Vector3(0f, 0f, Mathf.Abs(localPlaneOffset.z));
                        // Next position in world coordinates
                        Vector3 worldPosition = localPlaneHit.transform.TransformPoint(nextLocalPlanePosition);
                        // Change world position to the collision manager coordinates
                        // This way we get the offset needed to add in world coordinates through the collision manager coordinates
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

    public void InstantiateModel(GameObject selectedModel)
    {
        // Instancia para el objeto indicador que se proyecta en el mundo
        pendingObject = Instantiate(selectedModel, _hitPos, selectedModel.transform.rotation);
        //pendingObject = Instantiate(selectedModel, Vector3.zero, transform.rotation, parentObject.transform);

        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();
        //selectedBuildingObject.transform.position = new Vector3(0, 50f, 0);
        selectedBuildingObject._buildingManager = this;

        if (worldMenuManager.buildingState == BuildingState.withOffset)
        {
            // Set the rotation of the object so its local Z axis points at the same direction as the normal hit
            selectedBuildingObject.surfaceNormal = hit.normal;
            selectedBuildingObject.surfaceObject = hit.collider.gameObject;
            selectedBuildingObject.SetForwardAxisDirection();
            selectedBuildingObject.DisableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withTrigger)
        {
            // Set the rotation of the object so its local Z axis points at the same direction as the normal hit
            selectedBuildingObject.surfaceNormal = hit.normal;
            selectedBuildingObject.surfaceObject = hit.collider.gameObject;
            selectedBuildingObject.SetForwardAxisDirection();
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

        // Match the scale of the colliders
        parentObject.SetScale(selectedBuildingObject);
        parentObject.SetRotation(selectedBuildingObject);
    }

    public void InstantiateWorldObject()
    {
        pendingObject = Instantiate(hitObject.gameObject, SetHitObjectOffset(hit), hitObject.transform.rotation);
        SetLayerRecursively(pendingObject, LayerMask.NameToLayer("Ignore Raycast"));

        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();
        selectedBuildingObject._buildingManager = this;

        if (worldMenuManager.buildingState == BuildingState.withOffset)
        {
            selectedBuildingObject.DisableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withTrigger)
        {
            selectedBuildingObject.DisableColliders();
        }
        else if (worldMenuManager.buildingState == BuildingState.withPhysics)
        {
            selectedBuildingObject.objectRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
        }

        // Match the scale of the colliders
        parentObject.SetScale(selectedBuildingObject);
        parentObject.SetRotation(selectedBuildingObject);
    }

    // Colocar el objeto pendiente en la posicion indicada
    public void PlaceObject()
    {
        // Change the layer to Default so the Raycast can interact with the object
        selectedBuildingObject.gameObject.layer = LayerMask.NameToLayer("Default");

        // Change to kinematic rigidbody so OnCollisionStay isn't called
        selectedBuildingObject.objectRigidbody.isKinematic = true;
        selectedBuildingObject.isPlaced = true;

        if (worldMenuManager.buildingState == BuildingState.withPhysics)
            selectedBuildingObject.objectRigidbody.constraints = RigidbodyConstraints.FreezePosition;

        selectedBuildingObject.ChangeObjectLayer();

        if (!dataManager.currentObjects.Contains(pendingObject))
        {
            dataManager.SaveObjectsData(pendingObject, worldMenuManager.modelsList[worldMenuManager.modelIndex]);
            dataManager.currentObjects.Add(pendingObject);
            selectedBuildingObject.dataIndex = dataManager.currentObjects.IndexOf(pendingObject);
        }


        // "Soltamos" el objeto seleccionado
        selectedBuildingObject = null;
        pendingObject = null;
        //offset = new Vector3(0, 0, 0);

        // Reset the transform of the collision manager
        parentObject.Reset();
        //}
    }

    public BuildingObject FindBuildingObject(Transform transform)
    {
        while (transform != null)
        {
            BuildingObject bObj = transform.GetComponent<BuildingObject>();
            if (bObj != null)
            {
                return bObj;
            }
            transform = transform.parent;
        }
        return null;
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

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

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
            SetLayerRecursively(selectedBuildingObject.gameObject, LayerMask.NameToLayer("Ignore Raycast"));

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
        auxObj.ChangeObjectLayer();

        // Change to kinematic rigidbody so OnCollisionStay isn't called
        auxObj.objectRigidbody.isKinematic = true;
        auxObj.isPlaced = true;

        auxObj.SetPreviousTransform();

        auxObj.ChangeObjectLayer();

        // Reset the transform of the collision manager
        parentObject.Reset();
    }

    /// <summary>
    /// Destroy the object from the world
    /// </summary>
    public void DestroyObject()
    {
        if (dataManager.currentObjects.Contains(hitObject.gameObject))
        {
            dataManager.currentObjects.Remove(hitObject.gameObject);
            dataManager.RemoveObjectData(hitObject.dataIndex);
        }

        if (hitObject != null)
        {
            Destroy(hitObject.gameObject);
        }
    }
}
