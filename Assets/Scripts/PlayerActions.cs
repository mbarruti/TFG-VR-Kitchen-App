using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] WorldMenuManager _worldMenuManager;
    [SerializeField] BuildingManager _buildingManager;


    // Referencias a botones del mando izquierdo
    [SerializeField] InputActionReference _startAction;
    [SerializeField] InputActionReference _leftTriggerAction;

    // Referencias a botones del mando derecho
    [SerializeField] InputActionReference _rightTriggerAction;
    [SerializeField] InputActionReference _bAction;

    // -----------------------------------------------------

    void Awake()
    {
        _startAction.action.performed += OnStartAction;
        _bAction.action.performed += OnBAction;
        _leftTriggerAction.action.performed += OnLeftTriggerAction;

        _rightTriggerAction.action.performed += OnRightTriggerAction;
    }

    private void OnDestroy()
    {
        _startAction.action.performed -= OnStartAction;
        _bAction.action.performed -= OnBAction;
        _leftTriggerAction.action.performed -= OnLeftTriggerAction;

        _rightTriggerAction.action.performed -= OnRightTriggerAction;
    }

    // Funciones de BuildingManager y BuildingObject
    void OnRightTriggerAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingBuildingObject != null && _buildingManager.pendingBuildingObject.canPlace == true)
        {
            _buildingManager.PlaceObject();
        }
    }

    void OnLeftTriggerAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingBuildingObject != null)
        {
            _buildingManager.pendingBuildingObject.RotateObject();
        }
    }

    void OnStartAction(InputAction.CallbackContext context)
    {
        if (_worldMenuManager.isOpened == false)
        {
            // Si hay un objeto pendiente de colocar y abrimos el menu, se controla que ese proceso sigue pendiente
            if (_buildingManager.pendingBuildingObject != null)
            {
                _buildingManager.StopObjectPlacement();
            }

            _worldMenuManager.showWorldMenu();
        }
        // provisional
        else
        {
            Debug.Log("No se puede abrir el menu mientras estas colocando un objeto");
        }
    }

    void OnBAction(InputAction.CallbackContext context)
    {
        // Cerrar el menu
        if (_worldMenuManager.isOpened)
        {
            // Si hay un objeto pendiente de colocar, continua ese proceso al cerrar el menu
            if (_worldMenuManager.selectedObject != null && _buildingManager.pendingBuildingObject == null)
            {
                _buildingManager.InstantiateObject(_worldMenuManager.selectedObject);
            }

            _worldMenuManager.hideWorldMenu();
        }

        // Cancelar la colocacion del objeto
        else if (_worldMenuManager.isOpened == false && _buildingManager.pendingBuildingObject != null)
        {
            _buildingManager.CancelObjectPlacement();
        }
    }
}
