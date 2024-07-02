using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public List<GameObject> currentObjects;

    // -----------------------------------

    private GeneralData generalData;
    private List<ObjectData> objectDataList = new();
    private List<WallData> wallDataList = new();
    public SaveSystem saveSystem;

    [SerializeField] WorldMenuManager menu;
    [SerializeField] PlayerManager user;
    [SerializeField] WallManager wallManager;
    [SerializeField] Views viewsCamera;

    //private void Awake()
    //{
    //    saveSystem = new SaveSystem();

    //    LoadGeneralData();
    //}

    //private void OnApplicationQuit()
    //{
    //    if (user.mainController == user.rightController) SaveGeneralDataInFile(user.state, menu.buildingState, menu.continuousRotation, "RightHand Controller", menu.assistedControl);
    //    else if (user.mainController == user.leftController) SaveGeneralDataInFile(user.state, menu.buildingState, menu.continuousRotation, "LeftHand Controller", menu.assistedControl);
    //}

    public void RemoveObjectData(int index)
    {
        objectDataList.RemoveAt(index);
    }

    public void UpdateWallMaterialData(int index, int dataIndex)
    {
        //if (index != 0) wallDataList[dataIndex].materialIndex = index.ToString();
        //else wallDataList[dataIndex].materialIndex = index.ToString();

        wallDataList[dataIndex].materialIndex = index.ToString();

        Debug.Log(index);

        //wallDataList[dataIndex].shader = material.shader.name;
        //wallDataList[dataIndex].texture = material.mainTexture.name;
    }

    public void SaveObjectsData(GameObject obj, GameObject prefab)
    {
        ObjectData objectData = new ObjectData();

        objectData.position.Add(obj.transform.position.x);
        objectData.position.Add(obj.transform.position.y);
        objectData.position.Add(obj.transform.position.z);

        objectData.rotation.Add(obj.transform.eulerAngles.x);
        objectData.rotation.Add(obj.transform.eulerAngles.y);
        objectData.rotation.Add(obj.transform.eulerAngles.z);

        objectData.scale.Add(obj.transform.localScale.x);
        objectData.scale.Add(obj.transform.localScale.y);
        objectData.scale.Add(obj.transform.localScale.z);

        objectData.prefabId = prefab.name;
        objectData.layer = obj.layer;
        objectDataList.Add(objectData);
    }

    public void SaveRoom(string fileName)
    {
        Dictionary<string, object> dataToSave = new Dictionary<string, object>();
        dataToSave.Add("walls", wallDataList);
        dataToSave.Add("objects", objectDataList);
        saveSystem.SaveData(dataToSave, fileName);
    }

    public void SaveWallsData(GameObject obj, Material material)
    {
        WallData wallData = new WallData();

        wallData.position.Add(obj.transform.position.x);
        wallData.position.Add(obj.transform.position.y);
        wallData.position.Add(obj.transform.position.z);

        wallData.rotation.Add(obj.transform.eulerAngles.x);
        wallData.rotation.Add(obj.transform.eulerAngles.y);
        wallData.rotation.Add(obj.transform.eulerAngles.z);

        wallData.scale.Add(obj.transform.localScale.x);
        wallData.scale.Add(obj.transform.localScale.y);
        wallData.scale.Add(obj.transform.localScale.z);

        //wallData.shader = material.shader.name;

        //wallData.texture = material.mainTexture != null ? material.mainTexture.name : null;

        if (obj.CompareTag("Floor")) wallData.materialIndex = menu.floorMaterials.IndexOf(material).ToString();
        else if (obj.CompareTag("Ceiling")) wallData.materialIndex = 4.ToString();
        else wallData.materialIndex = menu.wallMaterials.IndexOf(material).ToString();

        wallData.tag = obj.tag;

        wallData.layer = obj.layer;

        wallDataList.Add(wallData);
    }

    private void SaveGeneralDataInFile(PlayerState playerState, BuildingState buildingState, bool continuousRotation, string controllerName, bool assistedControl, string fileName)
    {
        var dataToSave = new { user = generalData };
        saveSystem.SaveData(dataToSave, fileName);
    }

    private void LoadGeneralData(string fileName)
    {
        var loadedData = saveSystem.LoadData<Dictionary<string, GeneralData>>(fileName);
        if (loadedData != null && loadedData.ContainsKey("general"))
        {
            generalData = loadedData["general"];

            user.state = generalData.playerState;
            menu.buildingState = generalData.buildingState;
            menu.continuousRotation = generalData.continuousRotation;
            user.mainController = GameObject.Find(generalData.mainController);
            menu.assistedControl = generalData.assistedControl;
        }
    }

    private void ClearObjectsAndWalls()
    {
        foreach (GameObject obj in currentObjects)
        {
            Destroy(obj);
        }
        currentObjects.Clear();
        objectDataList.Clear();

        foreach (BuildingWall wall in menu.wallList)
        {
            Destroy(wall.gameObject);
        }
        menu.wallList.Clear();
        wallDataList.Clear();
    }

    public void LoadRoomData(string fileName)
    {
        wallManager.ceilingLight.SetActive(true);
        wallManager.auxiliarLight.SetActive(false);

        viewsCamera.ResetCamera();
        viewsCamera.wallsCenterPoint = Vector3.zero;

        ClearObjectsAndWalls();

        LoadWallsData(fileName);
        LoadObjectsData(fileName);

        if (menu.wallList.Count != 0)
        {
            wallManager.enabled = false;

            Vector3 centerPoint = menu.GetWallsCenterPoint();

            // Set player to center of room
            user.transform.position = new Vector3(centerPoint.x, user.transform.position.y, centerPoint.z);

            menu.mainButtonsList[0].transform.localPosition = new Vector3(-1.6f, 1.2f, 0);
            menu.mainButtonsList[1].transform.localPosition = new Vector3(-0.7f, 1.2f, 0);
            menu.mainButtonsList[2].transform.localPosition = new Vector3(0.2f, 1.2f, 0);
            menu.mainButtonsList[4].transform.localPosition = new Vector3(0, -100f, 0);
            menu.hideWorldMenu();
        }
        else
        {
            wallManager.enabled = true;
            wallManager.ceilingLight.SetActive(false);
            wallManager.auxiliarLight.SetActive(true);

            menu.ceiling.transform.position = new Vector3(0, -100f, 0);
            menu.ceiling.transform.localScale = new Vector3(50f, 0.1f, 50f);

            menu.floor.transform.position = new Vector3(0, 0, 0);
            menu.floor.transform.localScale = new Vector3(100f, 0.1f, 100f);
            menu.floor.GetComponent<Renderer>().material = menu.floorMaterials[4];

            user.transform.position = new Vector3(0, user.transform.position.y, 0);

            menu.mainButtonsList[0].transform.localPosition = new Vector3(0, -100f, 0);
            menu.mainButtonsList[1].transform.localPosition = new Vector3(0, -100f, 0);
            menu.mainButtonsList[2].transform.localPosition = new Vector3(0, -100f, 0);
            menu.mainButtonsList[4].transform.localPosition = new Vector3(-1.6f, 1.2f, 0);
            menu.ShowSecondaryButtons(4);
            menu.hideWorldMenu();
        }
    }

    private void LoadObjectsData(string fileName)
    {
        ObjectData data;
        var loadedData = saveSystem.LoadData<RoomData>(fileName);
        if (loadedData != null)
        {
            objectDataList.Clear();
            currentObjects.Clear();

            int initialNumOfObjects = loadedData.objects.Count;

            for (int i = 0; i < initialNumOfObjects; i++)
            {
                data = loadedData.objects[i];
                GameObject prefab = GetPrefabByName(data.prefabId);
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]), Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2])));
                    obj.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    obj.layer = data.layer;

                    BuildingObject bObj = obj.GetComponent<BuildingObject>();

                    bObj.ChangeObjectLayer();

                    SaveObjectsData(obj, prefab);
                    currentObjects.Add(obj);

                    bObj.dataIndex = currentObjects.IndexOf(obj);
                }
            }
        }
    }

    private void LoadWallsData(string fileName)
    {
        WallData data;
        var loadedData = saveSystem.LoadData<RoomData>(fileName);
        if (loadedData != null)
        {
            wallDataList.Clear();
            menu.wallList.Clear();

            int initialNumOfWalls = loadedData.walls.Count;

            for (int i = 0; i < initialNumOfWalls; i++)
            {

                data = loadedData.walls[i];
                if (data.tag == menu.floor.tag)
                {
                    menu.floor.transform.position = new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]);
                    menu.floor.transform.rotation = Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2]));
                    menu.floor.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    menu.floor.layer = data.layer;

                    Renderer renderer = menu.floor.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        //Material material = renderer.material;
                        //material.shader = Shader.Find(data.shader);
                        //if (!string.IsNullOrEmpty(data.texture))
                        //{
                        //    Texture texture = Resources.Load<Texture>(data.texture);
                        //    Debug.Log(texture);
                        //    material.mainTexture = texture;

                        //    renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.x / 2f, menu.floor.transform.localScale.z / 2f);
                        //}
                        renderer.material = menu.floorMaterials[int.Parse(data.materialIndex)];

                        renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.x / 2f, menu.floor.transform.localScale.z / 2f);
                    }

                    SaveWallsData(menu.floor, menu.floorMaterials[int.Parse(data.materialIndex)]);
                }
                else if (data.tag == menu.ceiling.tag)
                {
                    menu.ceiling.transform.position = new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]);
                    menu.ceiling.transform.rotation = Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2]));
                    menu.ceiling.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    menu.ceiling.layer = data.layer;

                    Renderer renderer = menu.ceiling.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        //Material material = renderer.material;
                        //material.shader = Shader.Find(data.shader);
                        //if (!string.IsNullOrEmpty(data.texture))
                        //{
                        //    Texture texture = Resources.Load<Texture>(data.texture);
                        //    material.mainTexture = texture;

                        //    renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.x / 2f, menu.floor.transform.localScale.z / 2f);
                        //}
                        renderer.material = menu.floorMaterials[int.Parse(data.materialIndex)];
                        renderer.material.mainTextureScale = new Vector2(menu.ceiling.transform.localScale.x / 2f, menu.ceiling.transform.localScale.z / 2f);
                    }

                    SaveWallsData(menu.ceiling, menu.floorMaterials[4]);
                }
                else
                {
                    GameObject obj = Instantiate(menu.wallPrefab, new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]), Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2])));
                    obj.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    obj.layer = data.layer;

                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        //Material material = renderer.material;
                        //material.shader = Shader.Find(data.shader);
                        //if (!string.IsNullOrEmpty(data.texture))
                        //{
                        //    Texture texture = Resources.Load<Texture>(data.texture);
                        //    material.mainTexture = texture;

                        //    renderer.material.mainTextureScale = new Vector2(obj.transform.localScale.z / 2f, obj.transform.localScale.y / 2f);
                        //}

                        renderer.material = menu.wallMaterials[int.Parse(data.materialIndex)];
                        renderer.material.mainTextureScale = new Vector2(obj.transform.localScale.z / 2f, obj.transform.localScale.y / 2f);
                    }

                    obj.layer = LayerMask.NameToLayer("Default");

                    BuildingWall bWall = obj.GetComponent<BuildingWall>();
                    menu.wallList.Add(bWall);
                    bWall.dataIndex = menu.wallList.IndexOf(bWall) + 2;

                    SaveWallsData(obj, menu.wallMaterials[int.Parse(data.materialIndex)]);
                }
            }
        }
    }

    private GameObject GetPrefabByName(string name)
    {
        Debug.Log("BUSCANDO " + name);
        foreach (var prefab in menu.modelsList)
        {
            Debug.Log("checkeando en " + prefab.name);
            if (prefab.name == name)
            {
                return prefab;
            }
        }
        return null;
    }
}

//public static class Extensions
//{
//    public static Vector3 ToVector3(this Vector3Data vectorData)
//    {
//        return new Vector3(vectorData.values[0], vectorData.values[1], vectorData.values[2]);
//    }
//}
