using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingManager : MonoBehaviour
{
    // Posicion donde ray colisiona con un objeto de la escena
    private Vector3 _hitPos;

    // Objeto instanciado que indica donde se colocara el objeto seleccionado
    private GameObject _pendingObject;
    //private BuildingObject _pendingBuildingObject;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Material[] collisionMaterials;

    [SerializeField] private WorldMenuManager worldMenuManager;

    // -------------------------------------

    // Componente BuildingObject del objeto instanciado a colocar
    public BuildingObject pendingBuildingObject;

    // Mandos de realidad virtual
    public GameObject rightController;
    public GameObject leftController;

    public bool gridOn; // Por ahora siempre va a ser true

    public float gridSize;

    public float rotateAmount;

    // Update is called once per frame
    void Update()
    {
        //// Mientras no tenga Menu, utilizo esto para probar cosas
        //if (keysManager.selectedObject != null)
        //{

        //    //Debug.Log(_pendingObject.gameObject);
        //    InstantiateObject(keysManager.selectedObject);
        //    keysManager.selectedObject = null;
        //}

        // Si hay un objeto pendiente de colocar en la escena, lo posicionamos donde apunta el usuario
        if (_pendingObject != null)
        {
            // Falta darle transparencia al objeto pendiente

            if (gridOn)
            {
                _pendingObject.transform.position = new Vector3(RoundToNearestGrid(_hitPos.x),
                                                                RoundToNearestGrid(_hitPos.y),
                                                                RoundToNearestGrid(_hitPos.z));
            }
            else
            {
                _pendingObject.transform.position = _hitPos; // Sin grid
            }

            // Actualizar materiales de colision
            UpdateMaterials();
        }
    }

    private void FixedUpdate()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        XRRayInteractor ray = rightController.GetComponent<XRRayInteractor>();
        RaycastHit hit;

        if(ray.TryGetCurrent3DRaycastHit(out hit))
        {
            _hitPos = hit.point;
        }
        //if (Physics.Raycast(ray, out hit, 1000, layerMask))
        //{
        //    _hitPos = hit.point;
        //}
    }

    // Si el objeto colisiona se le asigna un material rojo, si no se le asigna uno verde
    void UpdateMaterials()
    {
        if (pendingBuildingObject.canPlace)
        {
            pendingBuildingObject.assignMaterial(collisionMaterials[0]);
        }
        else
        {
            pendingBuildingObject.assignMaterial(collisionMaterials[1]);
        }
    }

    public void InstantiateObject(GameObject selectedObject)
    {
        // Instancia para el objeto indicador que se proyecta en el mundo
        _pendingObject = Instantiate(selectedObject, _hitPos, transform.rotation);
        pendingBuildingObject = _pendingObject.GetComponent<BuildingObject>();

        // Guardamos su material en la lista de materiales de colision
        collisionMaterials[2] = pendingBuildingObject.meshRenderer.material;
    }

    // Provisional, falta implementar el sistema grid de verdad
    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;

        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    // FUNCIONES LLAMADAS EN PlayerActions

    // Colocar el objeto pendiente en la posicion indicada
    public void PlaceObject()
    {
        // Volvemos a asignarle su material original
        pendingBuildingObject.assignMaterial(collisionMaterials[2]);

        // Instanciamos el objeto pendiente de colocacion
        Instantiate(_pendingObject, _hitPos, transform.rotation);

        // "Soltamos" el objeto
        //_pendingObject = null;
        //pendingBuildingObject = null;
    }

    // Cancelar la colocacion del objeto seleccionado
    public void CancelObjectPlacement()
    {
        Destroy(_pendingObject);
        _pendingObject = null;
        pendingBuildingObject = null;

        worldMenuManager.selectedObject = null;
    }

    // Parar la colocacion del objeto pendiente
    public void StopObjectPlacement()
    {
        Destroy(_pendingObject);
        _pendingObject = null;
        pendingBuildingObject = null;
    }
}
