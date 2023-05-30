using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Como no he terminado la interfaz, esto servira como sustituto por ahora
public class KeysManager : MonoBehaviour
{
    // Objeto seleccionado a generar
    public GameObject selectedObject;

    // Camara principal
    public CameraScript cam;

    // Lista de todos los modelos que se pueden utilizar
    public List<GameObject> modelsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Deseleccionar todos los objetos en el inicio
        DeselectAllObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            string keyCodeString = KeyCode.Alpha1.ToString();
            int numeroSeleccionado = int.Parse(keyCodeString.Replace("Alpha", ""));

            SelectObjectFromMenu(numeroSeleccionado);

            Debug.Log("Cubo1 seleccionado");
        }

        //if (Input.GetKeyUp(KeyCode.Alpha2))
        //{
        //    string keyCodeString = KeyCode.Alpha2.ToString();
        //    int numeroSeleccionado = int.Parse(keyCodeString.Replace("Alpha", ""));

        //    SelectObjectFromMenu(numeroSeleccionado);

        //    Debug.Log("Cubo2 seleccionado");
        //}

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            // Deseleccionar todos los objetos
            DeselectAllObjects();

            Debug.Log("Objeto deseleccionado");
        }
    }

    // Funcion que va a pasar a VRMenu
    public void SelectObjectFromMenu(int index)
    {
        if (index >= 0 && index < modelsList.Count)
        {
            // Deseleccionar todos los objetos
            DeselectAllObjects();

            // Seleccionar el objeto actual
            selectedObject = modelsList[index-1];
            //selectedObject.SetActive(true);
        }
    }

    // Funcion que va a pasar a VRMenu
    private void DeselectAllObjects()
    {
        //foreach (var obj in modelsList)
        //{
        //    obj.SetActive(false);
        //}
        selectedObject = null;
    }
}
