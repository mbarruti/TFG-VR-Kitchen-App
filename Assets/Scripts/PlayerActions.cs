using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] WorldMenuManager _worldMenuManager;
    [SerializeField] BuildingManager _buildingManager;

    [SerializeField] InputActionReference _openMenu;
    [SerializeField] InputActionReference _closeMenu;
    [SerializeField] InputActionReference _placeObject;
    [SerializeField] InputActionReference _rotateObject;

    // -----------------------------------------------------

    void Awake()
    {
        _openMenu.action.performed += OpenWorldMenu;
        _closeMenu.action.performed += CloseWorldMenu;
        _placeObject.action.performed += OnPlaceObject;
        _rotateObject.action.performed += OnRotateObject;
    }

    private void OnDestroy()
    {
        _openMenu.action.performed -= OpenWorldMenu;
        _closeMenu.action.performed -= CloseWorldMenu;
        _placeObject.action.performed -= OnPlaceObject;
    }

    // Funciones de BuildingManager y BuildingObject
    void OnPlaceObject(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingBuildingObject != null && _buildingManager.pendingBuildingObject.canPlace == true)
        {
            _buildingManager.PlaceObject();
        }
    }

    void OnRotateObject(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingBuildingObject != null)
        {
            _buildingManager.pendingBuildingObject.RotateObject();
        }
    }

    //void PlaceObject(InputAction.CallbackContext context)
    //{
    //    buildingManager.PlaceObject();
    //}

    // Funciones de WorldMenuManager
    void OpenWorldMenu(InputAction.CallbackContext context)
    {
        if (_buildingManager.pendingBuildingObject == null)
        {
            _worldMenuManager.showWorldMenu();
        }
        // provisional
        else
        {
            Debug.Log("No se puede abrir el menu mientras estas colocando un objeto");
        }
    }

    void CloseWorldMenu(InputAction.CallbackContext context)
    {
        _worldMenuManager.hideWorldMenu();
    }
}
