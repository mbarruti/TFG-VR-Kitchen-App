using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

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
    [SerializeField] InputActionReference _aAction;
    [SerializeField] InputActionReference _rightTouchpadAction;

    private PlayerState previousState;

    // Mandos de realidad virtual
    //[SerializeField] private GameObject rightController;
    //[SerializeField] private GameObject leftController;

    // -----------------------------------------------------

    public PlayerState state;

    void Awake()
    {
        state = PlayerState.isFree;

        //_startAction.action.performed += OnStartAction;
        //_leftTriggerAction.action.performed += OnLeftTriggerAction;
        //_yAction.action.performed += OnYAction;

        //_rightTriggerAction.action.performed += OnRightTriggerAction;
        //_bAction.action.performed += OnBAction;
        //_aAction.action.performed += OnAAction;
        //_rightTouchpadAction.action.performed += OnRightTouchpadAction;
    }

    private void Update()
    {
        updateStates();
    }

    private void OnDestroy()
    {
        _startAction.action.performed -= OpenWorldMenuAction;

        _leftTriggerAction.action.performed -= RotateObjectAction;

        _yAction.action.performed -= SelectObjectAction;

        _rightTriggerAction.action.performed -= PlaceObjectAction;

        _bAction.action.performed -= CancelObjectAction;
        _bAction.action.performed -= CloseWorldMenuAction;

        _aAction.action.performed -= InstantiateModelAction;

        _rightTouchpadAction.action.performed -= ScaleObjectAction;

    }

    // Actualiza el estado del jugador, dependiendo de la situacion
    private void updateStates()
    {

        //if (_worldMenuManager.isOpened && _buildingManager.selectedBuildingObject != null)
        //{
        //    state = PlayerState.isInMenuAndBuilding;
        //}
        if (_worldMenuManager.isOpened)
        {
            state = PlayerState.isInMenu;

            // Remove actions from other player states
            _bAction.action.performed -= CancelObjectAction;

            // Add actions from this player state
            _bAction.action.performed += CloseWorldMenuAction;
        }
        else if (_buildingManager.selectedBuildingObject != null)
        {
            state = PlayerState.isBuilding;

            // Remove actions from other player states
            _bAction.action.performed -= CloseWorldMenuAction;

            // Add actions from this player state
            _rightTriggerAction.action.performed += PlaceObjectAction;
            _leftTriggerAction.action.performed += RotateObjectAction;
            _startAction.action.performed += OpenWorldMenuAction;
            _bAction.action.performed += CancelObjectAction;
            _rightTouchpadAction.action.performed += ScaleObjectAction;
        }
        else
        {
            state = PlayerState.isFree;

            // Remove actions from other player states

            // Add actions from this player state
            _startAction.action.performed += OpenWorldMenuAction;
            _yAction.action.performed += SelectObjectAction;
            _aAction.action.performed += InstantiateModelAction;
        }
    }

    // Right trigger actions

    /// <summary>
    /// Place the selected object in the world with the right trigger input
    /// </summary>
    void PlaceObjectAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.selectedBuildingObject.canPlace == true)
        {
            _buildingManager.PlaceObject();
        }
    }

    // Left trigger actions

    /// <summary>
    /// Rotate the selected object with the left trigger input
    /// </summary>
    void RotateObjectAction(InputAction.CallbackContext context)
    {
        _buildingManager.selectedBuildingObject.RotateObject();
    }

    // Start input actions

    /// <summary>
    /// Open the world menu from start input
    /// </summary>
    void OpenWorldMenuAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding)
        {
            //// Si hay un objeto pendiente de colocar y abrimos el menu, se controla que ese proceso sigue pendiente
            //if (_buildingManager.selectedBuildingObject != null)
            //{
            _buildingManager.selectedBuildingObject = null;
            _buildingManager.pendingObject.SetActive(false);
            //}

            _worldMenuManager.showWorldMenu();
        }
        else if (state == PlayerState.isFree)
        {
            _worldMenuManager.showWorldMenu();
        }
        else
        {
            Debug.Log("No se puede abrir el menu mientras estas colocando un objeto / El menu ya esta abierto");
        }
    }

    // B input actions

    /// <summary>
    /// Close world menu with B input
    /// </summary>
    void CloseWorldMenuAction(InputAction.CallbackContext context)
    {
        // Si hay un modelo del menu seleccionado, continua ese proceso al cerrarlo
        if (_buildingManager.pendingObject != null)
        {
            //_buildingManager.pendingObject.SetActive(true);
            //_buildingManager.CancelObjectPlacement();
            _buildingManager.InstantiateModel(_worldMenuManager.selectedModel);
        }

        _worldMenuManager.hideWorldMenu();
    }

    /// <summary>
    /// Cancel object transform with B input
    /// </summary>
    void CancelObjectAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingObject != null) _buildingManager.CancelObjectPlacement();
        else
        {
            _buildingManager.CancelObjectTransform();
        }
    }

    // Y input actions
    void SelectObjectAction(InputAction.CallbackContext context)
    {
        _buildingManager.SelectObject();
    }

    // Right touchpad actions
    void ScaleObjectAction(InputAction.CallbackContext context)
    {
        if (_buildingManager.selectedBuildingObject != null)
        {
            _buildingManager.selectedBuildingObject.ScaleObject(context.action.ReadValue<Vector2>().y);
        }
        else
        {
            Debug.Log("No hay objeto seleccionado para escalar");
        }
    }

    // A input actions
    void InstantiateModelAction(InputAction.CallbackContext context)
    {
        if (_worldMenuManager.selectedModel != null)
        {
            _buildingManager.InstantiateModel(_worldMenuManager.selectedModel);
        }
    }
}
