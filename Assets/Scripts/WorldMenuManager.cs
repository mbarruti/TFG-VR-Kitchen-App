using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenuManager : MonoBehaviour
{
    private BuildingObject buildingModel;

    [SerializeField] PlayerManager _playerManager;
    //[SerializeField] BuildingManager _buildingManager;
    [SerializeField] WallManager _wallManager;

    [SerializeField] GameObject layerCamera;

    [SerializeField] Views viewsCamera;

    [SerializeField] GameObject finishButtonObject;

    [SerializeField] List<ButtonAnimationToggler> mainButtonsList = new List<ButtonAnimationToggler>();
    [SerializeField] List<ButtonAnimationToggler> modelsButtonsList = new List<ButtonAnimationToggler>();
    [SerializeField] List<ButtonAnimationToggler> wallsButtonsList = new List<ButtonAnimationToggler>();
    [SerializeField] List<ButtonAnimationToggler> viewsButtonsList = new List<ButtonAnimationToggler>();
    [SerializeField] List<GameObject> secondaryButtonsSections = new List<GameObject>();

    [SerializeField] List<GameObject> toggleList = new ();
    [SerializeField] List<GameObject> wallToggleList = new();
    [SerializeField] List<GameObject> floorToggleList = new();

    // -------------------------------------------

    public BuildingState buildingState;

    public bool isOpened;

    // Objeto seleccionado a generar
    public GameObject selectedModel;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    // List of all walls
    public List<BuildingWall> wallList = new List<BuildingWall>();

    // List of all wall materials
    public List<Material> wallMaterials = new List<Material>();

    public List<Material> floorMaterials = new List<Material>();
    public GameObject floor;

    public BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerManager.state == PlayerState.isBuildingWalls)
        {
            mainButtonsList[0].transform.localPosition = new Vector3(0, -100f, 0);
            mainButtonsList[1].transform.localPosition = new Vector3(0, -100f, 0);
            mainButtonsList[2].transform.localPosition = new Vector3(0, -100f, 0);
            ShowSecondaryButtons(3);
        }
        else
        {
            mainButtonsList[4].transform.localPosition = new Vector3(0, -100f, 0);
        }

        foreach (GameObject model in modelsList)
        {
            buildingModel = model.GetComponent<BuildingObject>();

            buildingModel._buildingManager = buildingManager;
        }
        // Deseleccionar todos los objetos en el inicio
        //DeselectAllObjects();
    }

    public void ChangeActiveToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (toggleList.Count > 0)
        {
            foreach (GameObject thisToggle in toggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!toggleList.Contains(toggle)) toggleList.Add(toggle);
    }

    public void ChangeActiveWallToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (wallToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in wallToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!wallToggleList.Contains(toggle)) wallToggleList.Add(toggle);
    }

    public void ChangeActiveFloorToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (floorToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in floorToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!floorToggleList.Contains(toggle)) floorToggleList.Add(toggle);
    }

    public void SelectObjectFromMenu(int index)
    {
        // Deseleccionar todos los objetos
        DeselectAllObjects();

        // Seleccionar el objeto actual
        selectedModel = modelsList[index];

        // (Optional, just for organization) If there is a pending object and another model is selected, the placement of the current selected object is cancelled
        if (buildingManager.pendingObject != null) buildingManager.CancelObjectPlacement();

        //buildingManager.InstantiateModel(selectedModel);

        // Mientras el objeto este seleccionado, el menu estara cerrado
        //hideWorldMenu();
    }

    private void DeselectAllObjects()
    {
        //foreach (var obj in modelsList)
        //{
        //    obj.SetActive(false);
        //}
        selectedModel = null;
    }

    // Open menu
    public void showWorldMenu()
    {
        //gameObject.SetActive(true);

        //transform.position = new Vector3(0f, 1.9f, 0f);

        layerCamera.SetActive(true);
        transform.localPosition = new Vector3(0f, 0.75f, 4f);

        isOpened = true;
    }

    // Close menu
    public void hideWorldMenu()
    {
        //gameObject.SetActive(false);
        layerCamera.SetActive(false);
        transform.position = new Vector3(0f, -30f, 0f);
        isOpened = false;
    }

    // Finish the process of building every wall in the scene
    public void FinishBuildingWalls()
    {
        if (_wallManager.wallList.Count >= 4)
        {
            _wallManager.SetWallsDirections();
            _wallManager.SetCeilingAndFloor();
            _wallManager.DeleteAllPoles();
            _wallManager.gameObject.SetActive(false);

            buildingManager.gameObject.SetActive(true);

            wallList = _wallManager.wallList;

            mainButtonsList[0].transform.localPosition = new Vector3(-1.6f, 1.2f, 0);
            mainButtonsList[1].transform.localPosition = new Vector3(-0.7f, 1.2f, 0);
            mainButtonsList[2].transform.localPosition = new Vector3(0.2f, 1.2f, 0);
            mainButtonsList[4].transform.localPosition = new Vector3(0, -100f, 0);
            hideWorldMenu();

            Rigidbody playerRigidbody = _playerManager.GetComponent<Rigidbody>();
            playerRigidbody.constraints = ~RigidbodyConstraints.FreezePosition;

            DeselectMainButtons();
            finishButtonObject.SetActive(false);

            _playerManager.state = PlayerState.isFree;
        }
    }

    public void SelectWallMaterial(int index)
    {
        Material wallMaterial = wallMaterials[index];

        foreach (BuildingWall wall in wallList)
        {
            wall.wallRenderer.material = wallMaterial;

            wall.wallRenderer.material.mainTextureScale = new Vector2(wall.transform.localScale.z / 2f, wall.transform.localScale.y / 2f);
        }
    }

    public void SelectFloorMaterial(int index)
    {
        Material floorMaterial = floorMaterials[index];

        var floorRenderer = floor.GetComponent<Renderer>();

        floorRenderer.material = floorMaterial;

        floorRenderer.material.mainTextureScale = new Vector2(floor.transform.localScale.x / 2f, floor.transform.localScale.z / 2f);
    }

    /// <summary>
    /// Deselect all main buttons
    /// </summary>
    public void DeselectMainButtons()
    {
        foreach (ButtonAnimationToggler button in mainButtonsList)
        {
            button.DeactivateIsSelected();
        }
    }

    /// <summary>
    /// Deselect all secondary buttons depending on the selected main button
    /// </summary>
    public void DeselectSecondaryButtons(int number)
    {
        switch (number)
        {
            case 1:
                foreach (ButtonAnimationToggler button in modelsButtonsList)
                {
                    button.DeactivateIsSelected();
                }
                break;

            case 2:
                foreach (ButtonAnimationToggler button in wallsButtonsList)
                {
                    button.DeactivateIsSelected();
                }
                break;

            case 3:
                foreach (ButtonAnimationToggler button in viewsButtonsList)
                {
                    button.DeactivateIsSelected();
                }
                break;
        }
    }

    /// <summary>
    /// Show the secondary buttons depending on the selected main button
    /// </summary>
    public void ShowSecondaryButtons(int index)
    {
        switch (index)
        {
            case 0:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, 0f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

            case 1:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, 0f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

            case 2:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, 0f, 0f);
                break;

            case 3:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

        }
    }
}
