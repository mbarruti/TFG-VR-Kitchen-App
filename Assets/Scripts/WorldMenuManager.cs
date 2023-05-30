using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMenuManager : MonoBehaviour
{

    // Objeto seleccionado a generar
    public GameObject selectedObject;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    public BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        // Deseleccionar todos los objetos en el inicio
        DeselectAllObjects();
    }

    // de prueba
    public void SelectCube()
    {
        selectedObject = modelsList[0];

        buildingManager.InstantiateObject(selectedObject);
        //crear un buildingobject -> bar
        //instantiate el game object -> foo
        //foo-> add component(bar)

        // Mientras el objeto este seleccionado, el menu estara cerrado
        hideWorldMenu();
    }

    // Sin terminar, igual hay que hacerlo con tags o algo
    public void SelectObjectFromMenu(int index)
    {
        if (index >= 0 && index < modelsList.Count)
        {
            // Deseleccionar todos los objetos
            DeselectAllObjects();

            // Seleccionar el objeto actual
            selectedObject = modelsList[index - 1];
            //selectedObject.SetActive(true);
        }
    }

    private void DeselectAllObjects()
    {
        //foreach (var obj in modelsList)
        //{
        //    obj.SetActive(false);
        //}
        selectedObject = null;
    }

    // Funciones de input que dependen del usuario (llamadas en PlayerActions)
    public void showWorldMenu()
    {
        gameObject.SetActive(true);
    }

    // Cerrar el menu
    public void hideWorldMenu()
    {
        gameObject.SetActive(false);
    }
}
