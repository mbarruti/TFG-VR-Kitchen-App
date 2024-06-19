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
    [SerializeField] InputActionReference _leftTriggerPressedAction;
    [SerializeField] InputActionReference _yAction;
    [SerializeField] InputActionReference _xAction;
    [SerializeField] InputActionReference _startAction;
    [SerializeField] InputActionReference _leftTouchpadAction;
    [SerializeField] InputActionReference _leftGripAction;

    // Referencias a botones del mando derecho
    [SerializeField] InputActionReference _rightTriggerAction;
    [SerializeField] InputActionReference _rightTriggerPressedAction;
    [SerializeField] InputActionReference _bAction;
    [SerializeField] InputActionReference _aAction;
    [SerializeField] InputActionReference _rightTouchpadAction;
    [SerializeField] InputActionReference _rightGripAction;

    // Move providers
    [SerializeField] ActionBasedContinuousMoveProvider leftMoveProvider;
    [SerializeField] ActionBasedContinuousMoveProvider rightMoveProvider;

    XRRayInteractor rightControllerRay;
    XRRayInteractor leftControllerRay;

    public bool leftTriggerPressed = false;

    //private PlayerState previousState;

    // Mandos de realidad virtual
    //[SerializeField] private GameObject rightController;
    //[SerializeField] private GameObject leftController;

    // -----------------------------------------------------

    public PlayerState state;

    public GameObject mainController;

    // VR Controllers
    public GameObject rightController;
    public GameObject leftController;

    public bool rightGripPressed;
    public bool leftGripPressed;

    public LayerMask newLayerMask;
    public LayerMask actualLayerMask;

    public Interactable selectedInteractable;

    void Awake()
    {
        rightControllerRay = rightController.GetComponent<XRRayInteractor>();
        leftControllerRay = leftController.GetComponent<XRRayInteractor>();

        if (mainController == rightController)
        {
            //moveProvider.leftHandMoveAction.reference.Set(leftHandActionReference);
            //moveProvider.rightHandMoveAction.reference.Set(rightHandActionReference);

            leftMoveProvider.enabled = true;
            rightMoveProvider.enabled = false;

            // Left Controller
            _startAction.action.performed += OnStartAction;

            _leftTriggerAction.action.performed -= OnRightTriggerAction;
            _leftTriggerPressedAction.action.performed += OnLeftTriggerPressedAction;

            _xAction.action.performed += OnXAction;
            _yAction.action.performed += OnYAction;
            _leftTouchpadAction.action.performed -= OnRightTouchpadAction;
            _leftGripAction.action.performed += OnLeftGripAction;

            // Right Controller
            _rightTriggerPressedAction.action.performed -= OnLeftTriggerPressedAction;
            _rightTriggerAction.action.performed += OnRightTriggerAction;

            _bAction.action.performed += OnBAction;
            _aAction.action.performed += OnAAction;
            _rightTouchpadAction.action.performed += OnRightTouchpadAction;
            _rightGripAction.action.performed += OnRightGripAction;
        }
        else if (mainController == leftController)
        {
            leftMoveProvider.enabled = false;
            rightMoveProvider.enabled = true;

            // Left Controller
            _startAction.action.performed += OnStartAction;

            _leftTriggerPressedAction.action.performed -= OnLeftTriggerPressedAction;
            _leftTriggerAction.action.performed += OnRightTriggerAction;

            _xAction.action.performed += OnXAction;
            _yAction.action.performed += OnYAction;
            _leftTouchpadAction.action.performed += OnRightTouchpadAction;
            _leftGripAction.action.performed += OnLeftGripAction;

            // Right Controller
            _rightTriggerAction.action.performed -= OnRightTriggerAction;
            _rightTriggerPressedAction.action.performed += OnLeftTriggerPressedAction;

            _bAction.action.performed += OnBAction;
            _aAction.action.performed += OnAAction;
            _rightTouchpadAction.action.performed -= OnRightTouchpadAction;
            _rightGripAction.action.performed += OnRightGripAction;
        }
    }

    private void Update()
    {
        UpdateStates();
    }

    //private void OnDestroy()
    //{
    //    _startAction.action.performed -= OnStartAction;
    //    _leftTriggerAction.action.performed -= OnLeftTriggerAction;
    //    _xAction.action.performed += OnXAction;
    //    _yAction.action.performed -= OnYAction;
    //    _leftGripAction.action.performed -= OnLeftGripAction;

    //    _rightTriggerAction.action.performed -= OnRightTriggerAction;
    //    _bAction.action.performed -= OnBAction;
    //    _aAction.action.performed -= OnAAction;
    //    _rightTouchpadAction.action.performed -= OnRightTouchpadAction;
    //    _rightGripAction.action.performed -= OnRightGripAction;
    //}

    // Actualiza el estado del jugador, dependiendo de la situacion
    public void UpdateStates()
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

    // Change main controller
    public void ChangeToLeftMainController()
    {
        mainController = leftController;

        _wallManager.mainRay = mainController.GetComponent<XRRayInteractor>();
        _buildingManager.ray = mainController.GetComponent<XRRayInteractor>();

        leftMoveProvider.enabled = false;
        rightMoveProvider.enabled = true;

        // Left Controller
        _leftTriggerPressedAction.action.performed -= OnLeftTriggerPressedAction;
        _leftTriggerAction.action.performed += OnRightTriggerAction;

        _leftTouchpadAction.action.performed += OnRightTouchpadAction;

        // Right Controller
        _rightTriggerAction.action.performed -= OnRightTriggerAction;
        _rightTriggerPressedAction.action.performed += OnLeftTriggerPressedAction;

        _rightTouchpadAction.action.performed -= OnRightTouchpadAction;
    }

    public void ChangeToRightMainController()
    {
        mainController = rightController;

        _wallManager.mainRay = mainController.GetComponent<XRRayInteractor>();
        _buildingManager.ray = mainController.GetComponent<XRRayInteractor>();

        // Left Controller
        _leftTriggerAction.action.performed -= OnRightTriggerAction;
        _leftTriggerPressedAction.action.performed += OnLeftTriggerPressedAction;

        _leftTouchpadAction.action.performed -= OnRightTouchpadAction;

        // Right Controller
        _rightTriggerPressedAction.action.performed -= OnLeftTriggerPressedAction;
        _rightTriggerAction.action.performed += OnRightTriggerAction;

        _rightTouchpadAction.action.performed += OnRightTouchpadAction;
    }

    // Funcion de cada boton
    void OnRightTriggerAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuilding /*&& _buildingManager.selectedBuildingObject.canPlace == true*/)
        {
            if (_worldMenuManager.buildingState == BuildingState.withTrigger && _buildingManager.parentObject.detectedColliders.Count == 2) _buildingManager.PlaceObject();
            else if (_worldMenuManager.buildingState == BuildingState.withOffset || _worldMenuManager.buildingState == BuildingState.withPhysics) _buildingManager.PlaceObject();
        }
        else if (state == PlayerState.isBuildingWalls)
        {
            if (!_worldMenuManager.isOpened && _wallManager.hit.collider != null)
            {
                if (_wallManager.finish == false) _wallManager.SetStartPole();
                else _wallManager.SetEndPole();
            }
        }
    }

    void OnLeftTriggerPressedAction(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed()) leftTriggerPressed = true;
        else leftTriggerPressed = false;
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
            if (_worldMenuManager.isOpened == true)
                _worldMenuManager.hideWorldMenu();
            else if (_wallManager.finish == true)
                _wallManager.CancelWallPlacement();
        }
    }

    void OnXAction(InputAction.CallbackContext context)
    {
        if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.poleList.Count == 0)
            {
                //_wallManager.wall.axisX = true;
                //_wallManager.wall.axisZ = false;
                _wallManager.planeHit.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            }
            else if (_wallManager.hit.collider.tag == "Wall")
            {
                _wallManager.DestroyWall(_wallManager.hit.collider.GetComponent<BuildingWall>());
            }
        }
        else if (state == PlayerState.isFree)
        {
            _buildingManager.DestroyObject();
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
            //if (_wallManager.poleList.Count > 0)
            //{
            //    Debug.Log("TO-DO");
            //}
            /*else*/ if (_wallManager.poleList.Count <= 0)
            {
                //_wallManager.wall.axisX = false;
                //_wallManager.wall.axisZ = true;
                _wallManager.planeHit.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }

    void OnRightTouchpadAction(InputAction.CallbackContext context)
    {
        //if (_buildingManager.selectedBuildingObject != null)
        if (state == PlayerState.isBuilding)
        {
            if (_worldMenuManager.buildingState == BuildingState.withPhysics)
            {
                if (leftTriggerPressed)
                {
                    _buildingManager.selectedBuildingObject.ScaleObject(context.action.ReadValue<Vector2>(), leftTriggerPressed);
                    _buildingManager.parentObject.SetScale(_buildingManager.selectedBuildingObject);
                }
                else
                {
                    _buildingManager.selectedBuildingObject.SetTouchpadValues(context.action.ReadValue<Vector2>().x, context.action.ReadValue<Vector2>().y);
                    _buildingManager.selectedBuildingObject.MoveWithTouchpad();
                }
            }
            else if (_worldMenuManager.buildingState == BuildingState.withOffset)
            {
                _buildingManager.selectedBuildingObject.ScaleObject(context.action.ReadValue<Vector2>(), leftTriggerPressed);
                _buildingManager.parentObject.SetScale(_buildingManager.selectedBuildingObject);
            }
        }
        else if (state == PlayerState.isBuildingWalls)
        {
            if (_wallManager.wallList.Count == 0 && _wallManager.wall.startPole != null)
            {
                _wallManager.wall.SetHeight(context.action.ReadValue<Vector2>().y);

                // Set the same height for the originPole in the world so every new pole gets the same height (even if it's the first one again)
                _wallManager.originPole.transform.localScale = _wallManager.wall.startPole.transform.localScale;

                // Replace the active poles and wall so they stick to the floor
                _wallManager.wall.startPole.transform.position = new Vector3(_wallManager.wall.startPole.transform.position.x, _wallManager.hit.point.y + _wallManager.wall.startPole.boxCollider.bounds.extents.y, _wallManager.wall.startPole.transform.position.z);
                _wallManager.wall.endPole.transform.position = new Vector3(_wallManager.wall.endPole.transform.position.x, _wallManager.hit.point.y + _wallManager.wall.endPole.boxCollider.bounds.extents.y, _wallManager.wall.endPole.transform.position.z);

                _wallManager.wall.transform.position = new Vector3(_wallManager.wall.transform.position.x, _wallManager.wall.transform.position.y + _wallManager.wall.endPole.boxCollider.bounds.extents.y, _wallManager.wall.transform.position.z);
            }
        }
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

    void OnRightGripAction(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && leftGripPressed == false)
        {
            rightGripPressed = true;

            if (state == PlayerState.isFree && mainController.Equals(rightController))
            {
                rightControllerRay.raycastMask = newLayerMask;

                // If there is not an interactable selected and one is hit while the grip is pressed, it is selected
                if (selectedInteractable == null && rightControllerRay.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    selectedInteractable = interactable;
                    selectedInteractable.interactingControllerPosition = rightController.transform.localPosition;
                }
            }
            else if (_worldMenuManager.continuousRotation == false)
            {
                if (state == PlayerState.isBuilding) _buildingManager.selectedBuildingObject.RotateObject(-_worldMenuManager.staticRotationValue);
                else if (state == PlayerState.isBuildingWalls) _wallManager.wall.SetRotation(-_worldMenuManager.staticRotationValue);
            }
        }
        else if (rightGripPressed == true && selectedInteractable != null)
        {
            selectedInteractable = null;
            rightControllerRay.raycastMask = actualLayerMask;
            rightGripPressed = false;
        }
        else
        {
            rightControllerRay.raycastMask = actualLayerMask;
            rightGripPressed = false;
        }
    }

    void OnLeftGripAction(InputAction.CallbackContext context)
    {
        if (context.action.IsPressed() && rightGripPressed == false)
        {
            leftGripPressed = true;

            if (state == PlayerState.isFree && mainController.Equals(leftController))
            {
                // If there is not an interactable selected and one is hit while the grip is pressed, it is selected
                if (selectedInteractable == null && leftControllerRay.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    selectedInteractable = interactable;
                    selectedInteractable.interactingControllerPosition = leftController.transform.localPosition;
                }
            }
            else if (_worldMenuManager.continuousRotation == false)
            {
                if (state == PlayerState.isBuilding) _buildingManager.selectedBuildingObject.RotateObject(_worldMenuManager.staticRotationValue);
                else if (state == PlayerState.isBuildingWalls && _wallManager.wall != null) _wallManager.wall.SetRotation(_worldMenuManager.staticRotationValue);
            }
        }
        else if (leftGripPressed == true && selectedInteractable != null)
        {
            selectedInteractable = null;
            leftGripPressed = false;
        }
        else
        {
            leftGripPressed = false;
        }
    }
}
