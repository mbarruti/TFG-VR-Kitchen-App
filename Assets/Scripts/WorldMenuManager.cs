using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuManager : MonoBehaviour
{
    private BuildingObject buildingModel;

    [SerializeField] PlayerManager _playerManager;
    //[SerializeField] BuildingManager _buildingManager;
    [SerializeField] WallManager _wallManager;

    // -------------------------------------------

    public BuildingState buildingState;

    public bool isOpened;

    // Objeto seleccionado a generar
    public GameObject selectedModel;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    public BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject model in modelsList)
        {
            buildingModel = model.GetComponent<BuildingObject>();

            buildingModel._buildingManager = buildingManager;
        }
        // Deseleccionar todos los objetos en el inicio
        DeselectAllObjects();
    }

    // Sin terminar, igual hay que hacerlo con tags o algo
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

    // Funciones de input que dependen del usuario (llamadas en PlayerActions)
    public void showWorldMenu()
    {
        gameObject.SetActive(true);
        isOpened = true;
    }

    // Cerrar el menu
    public void hideWorldMenu()
    {
        gameObject.SetActive(false);
        isOpened = false;
    }

    public void FinishBuildingWalls()
    {
        _wallManager.SetCeiling();
        _wallManager.DeleteAllPoles();
        _wallManager.gameObject.SetActive(false);

        buildingManager.gameObject.SetActive(true);

        _playerManager.state = PlayerState.isFree;
    }
}
