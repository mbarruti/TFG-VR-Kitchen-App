using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] WorldMenuManager _worldMenuManager;
    [SerializeField] BuildingManager _buildingManager;

    // Referencias a botones del mando izquierdo
    [SerializeField] InputActionReference _startAction;
    [SerializeField] InputActionReference _leftTriggerAction;
    [SerializeField] InputActionReference _yAction;

    // Referencias a botones del mando derecho
    [SerializeField] InputActionReference _rightTriggerAction;
    [SerializeField] InputActionReference _bAction;
    [SerializeField] InputActionReference _rightTouchpadAction;

    private PlayerState previousState;

    // -----------------------------------------------------

    public PlayerState state;

    void Awake()
    {
        state = PlayerState.isFree;

        _startAction.action.performed += OnStartAction;
        _leftTriggerAction.action.performed += OnLeftTriggerAction;
        _yAction.action.performed += OnYAction;

        _rightTriggerAction.action.performed += OnRightTriggerAction;
        _bAction.action.performed += OnBAction;
        _rightTouchpadAction.action.performed += OnRightTouchpadAction;
    }

    private void Update()
    {
        updateStates();
    }

    private void OnDestroy()
    {
        _startAction.action.performed -= OnStartAction;
        _leftTriggerAction.action.performed -= OnLeftTriggerAction;
        _yAction.action.performed -= OnYAction;

        _rightTriggerAction.action.performed -= OnRightTriggerAction;
        _bAction.action.performed -= OnBAction;
        _rightTouchpadAction.action.performed -= OnRightTouchpadAction;
    }

    // Actualiza el estado del jugador, dependiendo de la situacion
    public void updateStates()
    {

        //if (_worldMenuManager.isOpened && _buildingManager.selectedBuildingObject != null)
        //{
        //    state = PlayerState.isInMenuAndBuilding;
        //}
        if (_worldMenuManager.isOpened)
        {
            state = PlayerState.isInMenu;
        }
        else if (_buildingManager.selectedBuildingObject != null)
        {
            state = PlayerState.isBuilding;
        }
        else
        {
            state = PlayerState.isFree;
        }
    }

    // Funciones de cada boton
    void OnRightTriggerAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding && _buildingManager.selectedBuildingObject.canPlace == true)
        {
            _buildingManager.PlaceObject();
        }
    }

    void OnLeftTriggerAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding)
        {
            _buildingManager.selectedBuildingObject.RotateObject();
        }
    }

    void OnStartAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding)
        {
            //// Si hay un objeto pendiente de colocar y abrimos el menu, se controla que ese proceso sigue pendiente
            //if (_buildingManager.selectedBuildingObject != null)
            //{
                _buildingManager.StopObjectPlacement();
            //}

            _worldMenuManager.showWorldMenu();
        }
        // provisional
        else if (state == PlayerState.isFree)
        {
            _worldMenuManager.showWorldMenu();
        }
        else
        {
            Debug.Log("No se puede abrir el menu mientras estas colocando un objeto / El menu ya esta abierto");
        }
    }

    void OnBAction(InputAction.CallbackContext context)
    {
        // Cerrar el menu
        if (state == PlayerState.isInMenu)
        {
            // Si hay un objeto pendiente de colocar, continua ese proceso al cerrar el menu
            if (_buildingManager.selectedBuildingObject == null)
            {
                _buildingManager.InstantiateModel(_worldMenuManager.selectedModel);
            }

            _worldMenuManager.hideWorldMenu();
        }

        // Cancelar la transformacion del objeto
        else if (state == PlayerState.isBuilding)
        {
            if (_buildingManager.pendingObject != null) _buildingManager.CancelObjectPlacement();
            else
            {
                Debug.Log("hola");
                _buildingManager.CancelObjectTransform();
            }
        }
    }

    void OnYAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isFree)
        {
            _buildingManager.SelectObject();
        }
    }

    void OnRightTouchpadAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.selectedBuildingObject != null)
        {
            _buildingManager.selectedBuildingObject.ScaleObject();
        }
        else
        {
            Debug.Log("No hay objeto seleccionado para escalar");
        }
    }
}
