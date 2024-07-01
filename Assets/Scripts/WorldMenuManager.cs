using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenuManager : MonoBehaviour
{
    private BuildingObject buildingModel;

    private List<RoomData> roomDataList;

    [SerializeField] List<bool> roomList;

    [SerializeField] DataManager dataManager;

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
    [SerializeField] List<ButtonAnimationToggler> configButtonsList = new List<ButtonAnimationToggler>();
    [SerializeField] List<GameObject> secondaryButtonsSections = new List<GameObject>();

    [SerializeField] List<GameObject> toggleList = new ();
    [SerializeField] List<GameObject> wallToggleList = new();
    [SerializeField] List<GameObject> floorToggleList = new();
    [SerializeField] List<GameObject> rotationTypeToggleList = new();
    [SerializeField] List<GameObject> placementTypeToggleList = new();
    [SerializeField] List<GameObject> mainControllerToggleList = new();
    [SerializeField] List<GameObject> assistedControlToggleList = new();

    [SerializeField] GameObject continuousRotationToggle;
    [SerializeField] GameObject staticRotationToggle;

    [SerializeField] GameObject offsetPlacementToggle;
    [SerializeField] GameObject physicsPlacementToggle;
    [SerializeField] GameObject noCollisionPlacementToggle;

    [SerializeField] GameObject leftControllerToggle;
    [SerializeField] GameObject rightControllerToggle;

    [SerializeField] List<GameObject> saveButtonList = new();
    [SerializeField] List<GameObject> loadButtonList = new();

    // -------------------------------------------

    public GameObject yesControlToggle;
    public GameObject noControlToggle;

    public BuildingState buildingState;

    public bool isOpened;

    // Objeto seleccionado a generar
    public GameObject selectedModel;

    public GameObject wallPrefab;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    // List of all walls
    public List<BuildingWall> wallList = new List<BuildingWall>();

    // List of all wall materials
    public List<Material> wallMaterials = new List<Material>();

    public List<Material> floorMaterials = new List<Material>();
    public GameObject floor;

    public GameObject ceiling;

    public BuildingManager buildingManager;

    public int modelIndex;

    public bool assistedControl;

    public bool continuousRotation;
    public float staticRotationValue;

    // Start is called before the first frame update
    void Awake()
    {
        if (_playerManager.state == PlayerState.isBuildingWalls)
        {
            mainButtonsList[0].transform.localPosition = new Vector3(0, -100f, 0);
            mainButtonsList[1].transform.localPosition = new Vector3(0, -100f, 0);
            mainButtonsList[2].transform.localPosition = new Vector3(0, -100f, 0);
            ShowSecondaryButtons(4);
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

        if (continuousRotation) ChangeActiveRotationTypeToggle(continuousRotationToggle);
        else ChangeActiveRotationTypeToggle(staticRotationToggle);

        if (buildingState == BuildingState.withOffset) ChangeActivePlacementToggle(offsetPlacementToggle);
        else if (buildingState == BuildingState.withPhysics) ChangeActivePlacementToggle(physicsPlacementToggle);
        else if (buildingState == BuildingState.withTrigger) ChangeActivePlacementToggle(noCollisionPlacementToggle);

        if (continuousRotation) ChangeActiveRotationTypeToggle(continuousRotationToggle);
        else ChangeActiveRotationTypeToggle(staticRotationToggle);

        if (_playerManager.mainController = _playerManager.rightController) ChangeActiveControllerToggle(rightControllerToggle);
        else ChangeActiveControllerToggle(leftControllerToggle);
    }

    //public void CheckIfRoomEmpty(int index)
    //{
    //    if (!roomList[index])
    //    {
    //        GenerateRoom(index);
    //    }
    //}

    //public void GenerateRoom(int index)
    //{
    //    dataManager.Save
    //}

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

    public void ChangeActiveRotationTypeToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (rotationTypeToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in rotationTypeToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!rotationTypeToggleList.Contains(toggle)) rotationTypeToggleList.Add(toggle);
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

    public void ChangeActivePlacementToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (placementTypeToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in placementTypeToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!placementTypeToggleList.Contains(toggle)) placementTypeToggleList.Add(toggle);
    }

    public void ChangeActiveControllerToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (mainControllerToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in mainControllerToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!mainControllerToggleList.Contains(toggle)) mainControllerToggleList.Add(toggle);
    }

    public void ChangeAssistedControlToggle(GameObject toggle)
    {
        toggle.SetActive(true);
        if (assistedControlToggleList.Count > 0)
        {
            foreach (GameObject thisToggle in assistedControlToggleList)
            {
                if (toggle != thisToggle) thisToggle.SetActive(false);
            }
        }
        if (!assistedControlToggleList.Contains(toggle)) assistedControlToggleList.Add(toggle);
    }

    public void ChangeRotationType(bool value)
    {
        continuousRotation = value;
    }

    public void ChangeAssistedControlValye(bool value)
    {
        assistedControl = value;
    }

    public void ChangePlacementType(int number)
    {
        switch (number)
        {
            case 1:
                buildingState = BuildingState.withOffset;
                break;
            case 2:
                buildingState = BuildingState.withPhysics;
                break;
            case 3:
                buildingState = BuildingState.withTrigger;
                break;
        }
    }

    public void SelectObjectFromMenu(int index)
    {
        // Deseleccionar todos los objetos
        DeselectAllObjects();

        // Seleccionar el objeto actual
        selectedModel = modelsList[index];
        modelIndex = index;

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

    public Vector3 GetWallsCenterPoint()
    {
        Vector3 bottomLeftFront = wallList[0].transform.position;
        Vector3 topRightBack = wallList[0].transform.position;

        foreach (BuildingWall currentWall in wallList)
        {
            Vector3 wallPosition = currentWall.transform.position;

            // Calculate the first two corners
            bottomLeftFront = Vector3.Min(bottomLeftFront, wallPosition);
            topRightBack = Vector3.Max(topRightBack, wallPosition);
        }

        // Calculate the other two corners
        Vector3 bottomRightFront = new Vector3(topRightBack.x, bottomLeftFront.y, bottomLeftFront.z);
        Vector3 topLeftBack = new Vector3(bottomLeftFront.x, topRightBack.y, topRightBack.z);

        // Calculate the center point of the four corners
        //Vector3 centerPoint = Vector3.zero;
        Vector3 centerPoint = (bottomLeftFront + topRightBack + bottomRightFront + topLeftBack) / 4f;

        return centerPoint;
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

            dataManager.SaveWallsData(floor, floor.GetComponent<Renderer>().material);
            dataManager.SaveWallsData(ceiling, ceiling.GetComponent<Renderer>().material);
            foreach (BuildingWall wall in wallList)
            {
                dataManager.SaveWallsData(wall.gameObject, wall.wallRenderer.material);
            }

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

            case 4:
                foreach (ButtonAnimationToggler button in configButtonsList)
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
                secondaryButtonsSections[3].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

            case 1:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, 0f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[3].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

            case 2:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, 0f, 0f);
                secondaryButtonsSections[3].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;

            case 3:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[3].transform.localPosition = new Vector3(0f, 0f, 0f);
                break;

            case 4:
                secondaryButtonsSections[0].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[1].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[2].transform.localPosition = new Vector3(0f, -30f, 0f);
                secondaryButtonsSections[3].transform.localPosition = new Vector3(0f, -30f, 0f);
                break;
        }
    }

    public void showSaveAndLoadButtons(int number)
    {
        for (int i = 0; i < saveButtonList.Count; i++)
        {
            if (number - 1 == i) saveButtonList[i].SetActive(true);
            else saveButtonList[i].SetActive(false);
        }
        for (int i = 0; i < loadButtonList.Count; i++)
        {
            if (number - 1 == i) loadButtonList[i].SetActive(true);
            else loadButtonList[i].SetActive(false);
        }
    }
}
