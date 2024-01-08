using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] WorldMenuManager _worldMenuManager;
    [SerializeField] BuildingManager _buildingManager;
    [SerializeField] WallManager _wallManager;

    // Referencias a botones del mando izquierdo
    [SerializeField] InputActionReference _leftTriggerAction;
    [SerializeField] InputActionReference _yAction;
    [SerializeField] InputActionReference _xAction;
    [SerializeField] InputActionReference _startAction;

    // Referencias a botones del mando derecho
    [SerializeField] InputActionReference _rightTriggerAction;
    [SerializeField] InputActionReference _bAction;
    [SerializeField] InputActionReference _aAction;
    [SerializeField] InputActionReference _rightTouchpadAction;

    //private PlayerState previousState;

    // Mandos de realidad virtual
    //[SerializeField] private GameObject rightController;
    //[SerializeField] private GameObject leftController;

    // -----------------------------------------------------

    public PlayerState state;

    void Awake()
    {
        _startAction.action.performed += OnStartAction;
        _leftTriggerAction.action.performed += OnLeftTriggerAction;
        _xAction.action.performed += OnXAction;
        _yAction.action.performed += OnYAction;

        _rightTriggerAction.action.performed += OnRightTriggerAction;
        _bAction.action.performed += OnBAction;
        _aAction.action.performed += OnAAction;
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
        _xAction.action.performed += OnXAction;
        _yAction.action.performed -= OnYAction;

        _rightTriggerAction.action.performed -= OnRightTriggerAction;
        _bAction.action.performed -= OnBAction;
        _aAction.action.performed -= OnAAction;
        _rightTouchpadAction.action.performed -= OnRightTouchpadAction;
    }

    // Actualiza el estado del jugador, dependiendo de la situacion
    public void updateStates()
    {
        if (state != PlayerState.isBuildingWalls)
        {
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
    }

    // Funcion de cada boton
    void OnRightTriggerAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding && _buildingManager.selectedBuildingObject.canPlace == true)
        {
            _buildingManager.PlaceObject();
        }
        else if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.finish == false) _wallManager.SetStartPole();
            else _wallManager.SetEndPole();
        }
    }

    void OnLeftTriggerAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding)
        {
            _buildingManager.selectedBuildingObject.RotateObject();
            _buildingManager.parentObject.RotateObject();
        }
    }

    void OnStartAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding)
        {
            //// Si hay un objeto pendiente de colocar y abrimos el menu, se controla que ese proceso sigue pendiente
            //if (_buildingManager.selectedBuildingObject != null)
            //{
            //if (_buildingManager.pendingObject != null)
            //{
            //    //_buildingManager.selectedBuildingObject = null;
            //    //_buildingManager.pendingObject.SetActive(false);
            //    //}

            //    _worldMenuManager.showWorldMenu();
            //}
            //if (_buildingManager.selectedBuildingObject != null)
            //{

            if (_buildingManager.pendingObject != null)
            {
                // Stops the object placement
                _buildingManager.selectedBuildingObject.gameObject.SetActive(false);
            }
            else if (_buildingManager.selectedBuildingObject != null)
            {
                _buildingManager.CancelObjectTransform();
            }

            _worldMenuManager.showWorldMenu();
            //}
            //else Debug.Log("No se puede abrir si estas editando");
        }
        else if (state == PlayerState.isFree || state == PlayerState.isBuildingWalls)
        {
            _worldMenuManager.showWorldMenu();
        }
        //else
        //{
        //    Debug.Log("No se puede abrir el menu mientras estas colocando un objeto / El menu ya esta abierto");
        //}
    }

    void OnBAction(InputAction.CallbackContext context)
    {
        // Cerrar el menu
        if (state == PlayerState.isInMenu)
        {
            // Si hay un modelo del menu seleccionado, continua ese proceso al cerrarlo
            //if (_buildingManager.pendingObject != null)
            //{
            //    //_buildingManager.pendingObject.SetActive(true);
            //    //_buildingManager.CancelObjectPlacement();
            //    _buildingManager.InstantiateModel(_worldMenuManager.selectedModel);
            //}

            _worldMenuManager.hideWorldMenu();

            if (_buildingManager.pendingObject != null)
            {
                _buildingManager.selectedBuildingObject.gameObject.SetActive(true);
            }
        }

        // Cancelar la transformacion del objeto
        else if (state == PlayerState.isBuilding)
        {
            if (_buildingManager.pendingObject != null) _buildingManager.CancelObjectPlacement();
            else
            {
                _buildingManager.CancelObjectTransform();
            }
        }

        else if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.finish == true)
                _wallManager.CancelWallPlacement();
            else if (_worldMenuManager.isOpened == true)
                _worldMenuManager.hideWorldMenu();
        }
    }

    void OnXAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.hit.collider.tag == "Wall")
            {
                _wallManager.DestroyWall(_wallManager.hit.collider.GetComponent<BuildingWall>());
            }
            else if (_wallManager.poleList.Count == 0)
            {
                _wallManager.wall.axisX = true;
                _wallManager.wall.axisZ = false;
            }
        }
    }

    void OnYAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isFree)
        {
            _buildingManager.SelectObject();
        }
        else if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.poleList.Count > 0)
            {
                Debug.Log("TO-DO");
            }
            else
            {
                _wallManager.wall.axisX = false;
                _wallManager.wall.axisZ = true;
            }
        }
    }

    void OnRightTouchpadAction(InputAction.CallbackContext context)
    {
        //if (_buildingManager.selectedBuildingObject != null)
        if (state == PlayerState.isBuilding /*&& _buildingManager.selectedBuildingObject != null*/)
        {
            if (_worldMenuManager.buildingState == BuildingState.withPhysics)
            {
                _buildingManager.selectedBuildingObject.SetTouchpadValues(context.action.ReadValue<Vector2>().x, context.action.ReadValue<Vector2>().y);
                _buildingManager.selectedBuildingObject.MoveWithTouchpad();
            }
            //else
            //{
            //    _buildingManager.selectedBuildingObject.ScaleObject(context.action.ReadValue<Vector2>().y);
            //    _buildingManager.parentObject.ScaleCollider(context.action.ReadValue<Vector2>().y);
            //}
        }
        //else
        //{
        //    Debug.Log("No hay objeto seleccionado para escalar");
        //}
    }

    void OnAAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isFree)
        {
            if (_worldMenuManager.selectedModel != null)
            {
                _buildingManager.InstantiateModel(_worldMenuManager.selectedModel);
            }
        }
    }
}
