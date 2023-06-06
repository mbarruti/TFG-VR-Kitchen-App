using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingManager : MonoBehaviour
{
    // Posicion donde ray colisiona con un objeto de la escena
    private Vector3 _hitPos;

    // Objeto instanciado que indica donde se colocara el modelo seleccionado
    private GameObject _pendingObject;
    //private BuildingObject _selectedBuildingObject;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] private Material[] collisionMaterials;

    [SerializeField] private WorldMenuManager worldMenuManager;

    // -------------------------------------

    // Componente BuildingObject del objeto pendiente o del objeto del mundo seleccionado
    public BuildingObject selectedBuildingObject;

    public BuildingObject hitObject;

    // Mandos de realidad virtual
    public GameObject rightController;
    public GameObject leftController;

    public bool gridOn; // Por ahora siempre va a ser true

    public float gridSize;

    public float rotateAmount;

    // Update is called once per frame
    void Update()
    {
        // Si hay un objeto pendiente de colocar en la escena, lo posicionamos donde apunta el usuario
        //if (_pendingObject != null)
        if (selectedBuildingObject != null)
        {
            //if (gridOn)
            //{
            //    _pendingObject.transform.position = new Vector3(RoundToNearestGrid(_hitPos.x),
            //                                                    RoundToNearestGrid(_hitPos.y),
            //                                                    RoundToNearestGrid(_hitPos.z));
            //}
            //else
            //{
            //    _pendingObject.transform.position = _hitPos; // Sin grid
            //}

            selectedBuildingObject.gameObject.transform.position = _hitPos;

            // Actualizar materiales de colision
            UpdateMaterials();
        }
        //else if (selectedObject != null)
        //{
        //    selectedObject.transform.position = _hitPos;
        //    UpdateMaterials();
        //}
    }

    private void FixedUpdate()
    {
        XRRayInteractor ray = rightController.GetComponent<XRRayInteractor>();
        RaycastHit hit;

        if(ray.TryGetCurrent3DRaycastHit(out hit))
        {
            _hitPos = hit.point;

            // Activar el outline del objeto si está siendo apuntado con el mando
            if (playerManager.state == PlayerState.isFree && hit.collider.gameObject.TryGetComponent<BuildingObject>(out var auxObj))
            {
                if (hitObject != auxObj)
                {
                    //Debug.Log("Apunta a obj");
                    if (hitObject != null) { hitObject.disableOutline(); }
                    hitObject = auxObj;
                    hitObject.enableOutline();
                }
            }
            else
            {
                //Debug.Log("No apunta a obj");
                if (hitObject != null)
                {
                    //Debug.Log("Outline desactivada");
                    hitObject.disableOutline();
                    hitObject = null;
                }
            }
        }
    }

    // Si el objeto colisiona se le asigna un material rojo, si no se le asigna uno verde
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

    public void InstantiateModel(GameObject selectedModel)
    {
        // Instancia para el objeto indicador que se proyecta en el mundo
        _pendingObject = Instantiate(selectedModel, _hitPos, transform.rotation);
        selectedBuildingObject = _pendingObject.GetComponent<BuildingObject>();

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

        // Instanciamos el objeto pendiente de colocacion
        if (_pendingObject != null)
        {
            GameObject obj = Instantiate(_pendingObject, _hitPos, transform.rotation);

            obj.GetComponent<BoxCollider>().isTrigger = false;
        }
        else selectedBuildingObject = null; // "Soltamos" el objeto seleccionado
    }

    // Cancelar la colocacion del objeto pendiente
    public void CancelObjectPlacement()
    {
        Destroy(_pendingObject);
        _pendingObject = null;
        selectedBuildingObject = null;

        worldMenuManager.selectedModel = null;
    }

    // Parar la colocacion del objeto pendiente
    public void StopObjectPlacement()
    {
        Destroy(_pendingObject);
        _pendingObject = null;
        selectedBuildingObject = null;
    }

    // TODO
    public void SelectObject()
    {
        if (hitObject != null)
        {
            selectedBuildingObject = hitObject;
            selectedBuildingObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
