using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuManager : MonoBehaviour
{

    // -------------------------------------------

    public bool isOpened;

    // Objeto seleccionado a generar
    public GameObject selectedModel;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    public BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
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
        buildingManager.InstantiateModel(selectedModel);

        // Mientras el objeto este seleccionado, el menu estara cerrado
        hideWorldMenu();
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
}
