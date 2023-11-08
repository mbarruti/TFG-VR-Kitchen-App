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

    private Collider _hitCollider;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] private Material[] collisionMaterials;

    [SerializeField] private WorldMenuManager worldMenuManager;

    // -------------------------------------

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

    //public Vector3 offset = new Vector3(0, 0, 0);

    private void Start()
    {
        ray = rightController.GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hit.collider.bounds.size);
        //offset = Vector3.zero;
        // Si hay un objeto pendiente de colocar en la escena, lo posicionamos donde apunta el usuario
        //if (pendingObject != null)

        if (selectedBuildingObject != null)
        {
            parentObject.transform.position = _hitPos + GetOffset(hit.normal);
            //selectedBuildingObject.transform.position = _hitPos + GetOffset(hit.normal);

            // PRUEBA: los parametros son de prueba para el cubo, dependiendo del objeto el radio de la esfera deberia cambiar
            //detectedColliders = Physics.OverlapSphere(selectedBuildingObject.transform.position, 2);
            //Debug.Log(selectedBuildingObject.detectedColliders.Count);
            //if (parentObject.detectedColliders.Count == 1)
            //{
            //Debug.Log("no colisiona");
            //offset = Vector3.zero;
            //}
            //Debug.Log(detectedColliders.Length);

            UpdateOffset();

            //if (offset == Vector3.zero) Debug.Log(offset);
            //Debug.Log(offset);
            selectedBuildingObject.transform.position = parentObject.transform.position + offset;
            //selectedBuildingObject.transform.position = parentObject.transform.position + _hitPos + GetOffset(hit.normal) + offset;
            //selectedBuildingObject.transform.position = Vector3.Lerp(selectedBuildingObject.transform.position, _hitPos + GetOffset(hit.normal) + offset, 30f * Time.deltaTime);

            // Actualizar materiales de colision
            UpdateMaterials();
        }
    }

    private void FixedUpdate()
    {
        //RaycastHit hit;

        if (ray.TryGetCurrent3DRaycastHit(out hit))
        {
            _hitPos = hit.point;
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log("Nombre de objeto que choca con rayo: " + hit.collider.gameObject.name);
            // Ajustar la posicion para que el centro del objeto este en el punto de colision
            if (playerManager.state == PlayerState.isBuilding)
            {
                _hitPos = hit.point /*+ GetOffset(hit.normal)*/;
                //parentObject.transform.position = _hitPos + GetOffset(hit.normal);
            }

            // Activar el outline del objeto si está siendo apuntado con el mando
            if (playerManager.state == PlayerState.isFree && hit.collider.gameObject.TryGetComponent<BuildingObject>(out var auxObj))
            {
                if (hitObject != auxObj)
                {
                    //Debug.Log("Apunta a obj");
                    if (hitObject != null) hitObject.switchOutline(false);
                    hitObject = auxObj;
                    hitObject.switchOutline(true);
                }
            }
            else
            {
                //Debug.Log("No apunta a obj");
                if (hitObject != null)
                {
                    //Debug.Log("Outline desactivada");
                    hitObject.switchOutline(false);
                    hitObject = null;
                }
            }
            // Prueba de movimiento de objeto en FixedUpdate
            //if (selectedBuildingObject != null)
            //{
            //    selectedBuildingObject.objectRigidbody.MovePosition(_hitPos);
            //    //selectedBuildingObject.objectRigidbody.transform.position = _hitPos;

            //    // Actualizar materiales de colision
            //    UpdateMaterials();
            //}
        }
    }

    /// <summary>
    /// Si el objeto colisiona se le asigna un material rojo, si no se le asigna uno verde
    /// </summary>
    void UpdateMaterials()
    {
        if (selectedBuildingObject.canPlace)
        {
            selectedBuildingObject.assignMaterial(collisionMaterials[0]);
        }
        else
        {
            selectedBuildingObject.assignMaterial(collisionMaterials[1]);
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

    private Vector3 GetOffset(Vector3 normal)
    {
        // Encontrar el eje dominante de la normal
        float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        // Calcular el desplazamiento necesario para que el centro del objeto este alineado con la superficie
        Vector3 hitOffset = Vector3.zero;
        if (Mathf.Abs(normal.x) == maxAxis)
        {
            hitOffset = normal * parentObject.boxCollider.bounds.extents.x;
            //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.x;
        }
        else if (Mathf.Abs(normal.y) == maxAxis)
        {
            hitOffset = normal * parentObject.boxCollider.bounds.extents.y;
            //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.y;
        }
        else if (Mathf.Abs(normal.z) == maxAxis)
        {
            hitOffset = normal * parentObject.boxCollider.bounds.extents.z;
            //hitOffset = normal * selectedBuildingObject.boxCollider.bounds.extents.z;
        }

        return hitOffset;

    }

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
                    // Maximum distance between the center of both objects before they start clipping
                    float maxDistance = GetAxis(hitt.normal, objCollider.bounds.extents + parentObject.boxCollider.bounds.extents);
                    // Actual distance between the center of both objects
                    float actualDistance = GetAxis(hitt.normal, Vector3.Scale(-hitt.normal, parentObject.boxCollider.transform.InverseTransformPoint(objCollider.transform.position)));
                    Debug.Log(maxDistance);
                    if (actualDistance < maxDistance)
                    {
                        offset += hitt.normal * (maxDistance - actualDistance);
                    }
                    //Debug.Log(dir);
                    //Debug.Log("closestPoint es:" + closestPoint);
                    //Debug.Log("hit.point es:" + hit.point);
                    //Debug.Log("La normal del rayo que golpea en closestPoint es:" + hitt.normal);

                    Debug.DrawRay(parentObject.transform.position, dir, Color.red);
                    //Debug.DrawRay(hitt.point, hitt.normal, Color.blue);
                }
            }
        }
    }

    public void InstantiateModel(GameObject selectedModel)
    {
        if (pendingObject != null) Destroy(pendingObject);

        // Instancia para el objeto indicador que se proyecta en el mundo
        pendingObject = Instantiate(selectedModel, _hitPos, transform.rotation);
        //pendingObject = Instantiate(selectedModel, Vector3.zero, transform.rotation, parentObject.transform);

        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();

        // Guardamos su material en la lista de materiales de colision
        collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;

        // Match the scale of the colliders
        parentObject.SetScale(selectedBuildingObject);

        Debug.Log(parentObject.boxCollider.bounds.extents);
    }

    // FUNCIONES LLAMADAS EN PlayerActions

    // Colocar el objeto pendiente en la posicion indicada
    public void PlaceObject()
    {
        // Volvemos a asignarle su material original
        selectedBuildingObject.assignMaterial(collisionMaterials[2]);

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
        Debug.Log(parentObject.boxCollider.bounds.extents);
        // Reset the transform of the collision manager
        parentObject.Reset();
    }

    // Parar la colocacion del objeto pendiente
    //public void StopObjectPlacement()
    //{
    //    Destroy(pendingObject);
    //    Debug.Log(pendingObject);
    //    //pendingObject = null;
    //    selectedBuildingObject = null;
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
            collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;
            // Activamos isTrigger para que no haya conflicto con el Raycast
            //selectedBuildingObject.boxCollider.isTrigger = true;

            // Change the layer to Ignore Raycast so the Raycast can't interact with the object
            selectedBuildingObject.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            // Change to non-kinematic rigidbody so OnCollisionStay is called
            selectedBuildingObject.objectRigidbody.isKinematic = false;
            selectedBuildingObject.isPlaced = false;

            // Match the scale of the colliders
            parentObject.SetScale(selectedBuildingObject);
        }
    }

    // Cancelar la transformacion del objeto del mundo seleccionado
    public void CancelObjectTransform()
    {
        var auxObj = selectedBuildingObject;

        selectedBuildingObject = null;

        // Change the layer to Default so the Raycast can interact with the object
        auxObj.gameObject.layer = LayerMask.NameToLayer("Default");

        // Change to kinematic rigidbody so OnCollisionStay isn't called
        auxObj.objectRigidbody.isKinematic = true;
        auxObj.isPlaced = true;

        auxObj.SetPreviousTransform();
        // Volvemos a asignarle su material original
        auxObj.assignMaterial(collisionMaterials[2]);
        // Desactivamos isTrigger para que no haya conflicto con el Raycast
        //auxObj.boxCollider.isTrigger = false;

        // Reset the transform of the collision manager
        parentObject.Reset();
    }
}
