using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingManager : MonoBehaviour
{
    // Posicion donde ray colisiona con un objeto de la escena
    private Vector3 _hitPos;

    // Raycast from right controller
    private XRRayInteractor ray;
    
    // Hit object from raycast
    // private RaycastHit hit;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] private Material[] collisionMaterials;

    [SerializeField] private WorldMenuManager worldMenuManager;

    // -------------------------------------

    // Objeto instanciado que indica donde se colocara el modelo seleccionado
    public GameObject pendingObject;
    // Componente BuildingObject del objeto pendiente o del objeto del mundo seleccionado
    public BuildingObject selectedBuildingObject;

    public BuildingObject hitObject;

    // Mandos de realidad virtual
    public GameObject rightController;
    public GameObject leftController;

    public float rotateAmount;

    private void Start()
    {
       ray = rightController.GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si hay un objeto pendiente de colocar en la escena, lo posicionamos donde apunta el usuario
        //if (pendingObject != null)
        if (selectedBuildingObject != null)
        {
            selectedBuildingObject.transform.position = _hitPos;
            //selectedBuildingObject.boxCollider.transform.position = _hitPos;

            // Prueba de ComputePenetration
            //if (Physics.ComputePenetration(selectedBuildingObject.boxCollider, selectedBuildingObject.boxCollider.transform.position, selectedBuildingObject.boxCollider.transform.rotation, hit.collider, hit.transform.position, hit.transform.rotation, out var direction, out var distance))
            //{
            //    selectedBuildingObject.boxCollider.transform.position += direction * distance;
            //}

            // Actualizar materiales de colision
            UpdateMaterials();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        
        if (ray.TryGetCurrent3DRaycastHit(out hit))
        {
            _hitPos = hit.point;

            // // Ajustar la posicion para que el centro del objeto este en el punto de colision
            if (playerManager.state == PlayerState.isBuilding) 
            {
                _hitPos = hit.point + FindMaxAxis(hit.normal);
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

    Vector3 FindMaxAxis(Vector3 normal)
    {
        // Encontrar el eje dominante de la normal
        float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        // Calcular el desplazamiento necesario para que el centro del objeto este alineado con la superficie
        Vector3 offset = Vector3.zero;
        if (Mathf.Abs(normal.x) == maxAxis)
        {
            offset = normal * selectedBuildingObject.boxCollider.bounds.extents.x;
        }
        else if (Mathf.Abs(normal.y) == maxAxis)
        {
            offset = normal * selectedBuildingObject.boxCollider.bounds.extents.y;
        }
        else if (Mathf.Abs(normal.z) == maxAxis)
        {
            offset = normal * selectedBuildingObject.boxCollider.bounds.extents.z;
        }

        return offset;

    }

    public void InstantiateModel(GameObject selectedModel)
    {
        if (pendingObject != null) Destroy(pendingObject);

        // Instancia para el objeto indicador que se proyecta en el mundo
        pendingObject = Instantiate(selectedModel, _hitPos, transform.rotation);
        
        selectedBuildingObject = pendingObject.GetComponent<BuildingObject>();

        // Guardamos su material en la lista de materiales de colision
        collisionMaterials[2] = selectedBuildingObject.meshRenderer.material;
    }


    // Provisional, falta implementar el sistema grid de verdad
    //float RoundToNearestGrid(float pos)
    //{
    //    float xDiff = pos % gridSize;
    //    pos -= xDiff;

    //    if (xDiff > (gridSize / 2))
    //    {
    //        pos += gridSize;
    //    }
    //    return pos;
    //}

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

        // "Soltamos" el objeto seleccionado
        selectedBuildingObject = null;
        //}

        pendingObject = null;
    }

    // Cancelar la colocacion del objeto pendiente
    public void CancelObjectPlacement()
    {
        Destroy(pendingObject);
        pendingObject = null;
        selectedBuildingObject = null;

        //worldMenuManager.selectedModel = null;
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
            selectedBuildingObject.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    //Cancelar la transformacion del objeto del mundo seleccionado
    public void CancelObjectTransform()
    {
        var auxObj = selectedBuildingObject;

        selectedBuildingObject = null;

        auxObj.SetPreviousTransform();
        // Volvemos a asignarle su material original
        auxObj.assignMaterial(collisionMaterials[2]);
        // Desactivamos isTrigger para que no haya conflicto con el Raycast
        //auxObj.boxCollider.isTrigger = false;
    }
}
