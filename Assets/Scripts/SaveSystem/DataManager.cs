using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameObject[] prefabs;

    // -----------------------------------

    private GeneralData generalData;
    private List<ObjectData> objectDataList = new();
    private List<WallData> wallDataList = new();
    public SaveSystem saveSystem;

    [SerializeField] WorldMenuManager menu;
    [SerializeField] PlayerManager user;

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

    //public void InstantiateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    //{
    //    GameObject obj = Instantiate(prefab, position, rotation);
    //    Renderer renderer = obj.GetComponent<Renderer>();
    //    Material material = renderer != null ? renderer.material : null;

    //    ObjectData objectData = new ObjectData(obj.transform, material, prefab.name, obj.layer);
    //    objectDataList.Add(objectData);
    //}

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

        wallData.shader = material.name;
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

    public void LoadObjectsData(string fileName)
    {
        //var loadedData = saveSystem.LoadData<Dictionary<string, List<ObjectData>>>(fileName);
        var loadedData = saveSystem.LoadData<RoomData>(fileName);
        if (loadedData != null)
        {
            objectDataList = loadedData.objects;
            foreach (var data in objectDataList)
            {
                Debug.Log(data.prefabId);
                GameObject prefab = GetPrefabByName(data.prefabId);
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]), Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2])));
                    obj.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    obj.layer = data.layer;
                }
            }
        }
    }

    public void LoadWallsData(string fileName)
    {
        WallData data;
        var loadedData = saveSystem.LoadData<RoomData>(fileName);
        if (loadedData != null)
        {
            menu.wallList.Clear();

            wallDataList = loadedData.walls;
            for (int i = 0; i < wallDataList.Count; i++)
            {
                data = wallDataList[i];
                if (i == 0)
                {
                    menu.floor.transform.position = new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]);
                    menu.floor.transform.rotation = Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2]));
                    menu.floor.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    menu.floor.layer = data.layer;

                    Renderer renderer = menu.floor.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.shader);
                        if (!string.IsNullOrEmpty(data.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.z / 2f, menu.floor.transform.localScale.y / 2f);
                        }
                    }
                }
                else if (i == 1)
                {
                    menu.ceiling.transform.position = new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]);
                    menu.ceiling.transform.rotation = Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2]));
                    menu.ceiling.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    menu.ceiling.layer = data.layer;

                    Renderer renderer = menu.floor.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.shader);
                        if (!string.IsNullOrEmpty(data.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.z / 2f, menu.floor.transform.localScale.y / 2f);
                        }
                    }
                }
                else
                {
                    GameObject obj = Instantiate(menu.wallPrefab, new Vector3((float)data.position[0], (float)data.position[1], (float)data.position[2]), Quaternion.Euler(new Vector3((float)data.rotation[0], (float)data.rotation[1], (float)data.rotation[2])));
                    obj.transform.localScale = new Vector3((float)data.scale[0], (float)data.scale[1], (float)data.scale[2]);
                    obj.layer = data.layer;

                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.shader);
                        if (!string.IsNullOrEmpty(data.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(obj.transform.localScale.z / 2f, obj.transform.localScale.y / 2f);
                        }
                    }

                    menu.wallList.Add(obj.GetComponent<BuildingWall>());
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

public static class Extensions
{
    public static Vector3 ToVector3(this Vector3Data vectorData)
    {
        return new Vector3(vectorData.values[0], vectorData.values[1], vectorData.values[2]);
    }
}
