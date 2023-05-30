using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// No hace mucha falta, solo para ubicarme
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

public class CameraScript : MonoBehaviour
{
    #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
    #endif

    //public GameObject cubePrefab;

    //public float width;
    //public float height;

    public float speedHorizontal = 2.0f;
    public float speedVertical = 2.0f;

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // No hace mucha falta, solo para ubicarme
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // Obtener el centro de la pantalla en píxeles
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;

        Vector2 editorWindowPos = new Vector2(
            UnityEditor.EditorWindow.focusedWindow.position.x,
            UnityEditor.EditorWindow.focusedWindow.position.y);

        screenCenter += editorWindowPos;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        SetCursorPos((int)screenCenter.x, (int)screenCenter.y);
        Cursor.visible = false;
        #endif
        // Hasta aqui
    }

    // Update is called once per frame
    void Update()
    {   
        // Control de la camara
        _yaw += speedHorizontal * Input.GetAxis("Mouse X");
        _pitch -= speedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
    }

    //public void GenerateObject(GameObject obj)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    // Si se apunta hacia un objeto
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        // Obtenemos el tamañao de la bounding box del objeto a generar
    //        Vector3 objSize = obj.GetComponent<Renderer>().bounds.size;

    //        // Calculamos el desplazamiento de la posicion en la que se generara el objeto
    //        Vector3 spawnPosition = hit.point + new Vector3(objSize.x / 2, objSize.y / 2, objSize.z / 2);

    //        // Instanciamos el objeto en dicha posicion
    //        Instantiate(obj, spawnPosition, Quaternion.identity);

    //        //Instantiate(obj, hit.point, Quaternion.identity);
    //    }
    //}
}
