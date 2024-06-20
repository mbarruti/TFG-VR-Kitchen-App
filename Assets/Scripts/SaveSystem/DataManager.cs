using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameObject[] prefabs;

    // -----------------------------------

    private GeneralData generalData;
    private RoomData roomData;
    private List<ObjectData> objectDataList = new List<ObjectData>();
    private List<WallData> wallDataList = new List<WallData>();
    private SaveSystem saveSystem;

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
        ObjectData objectData = new ObjectData(obj.transform, prefab.name, obj.layer);
        objectDataList.Add(objectData);
    }

    private void SaveObjectsDataInFile()
    {
        var dataToSave = new { objects = objectDataList };
        saveSystem.SaveData(dataToSave);
    }

    public void SaveWallsData(GameObject obj, Material material)
    {
        WallData wallData = new WallData(obj.transform, material, obj.layer);
        wallDataList.Add(wallData);
    }

    private void SaveWallsDataInFile()
    {
        var dataToSave = new { walls = wallDataList };
        saveSystem.SaveData(dataToSave);
    }

    public void SaveRoomData()
    {

    }

    private void SaveGeneralDataInFile(PlayerState playerState, BuildingState buildingState, bool continuousRotation, string controllerName, bool assistedControl)
    {
        var dataToSave = new { user = generalData };
        saveSystem.SaveData(dataToSave);
    }

    private void LoadGeneralData()
    {
        var loadedData = saveSystem.LoadData<Dictionary<string, GeneralData>>();
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

    private void LoadObjectsData()
    {
        var loadedData = saveSystem.LoadData<Dictionary<string, List<ObjectData>>>();
        if (loadedData != null && loadedData.ContainsKey("objects"))
        {
            objectDataList = loadedData["objects"];
            foreach (var data in objectDataList)
            {
                GameObject prefab = GetPrefabByName(data.prefabId);
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, data.transform.position.ToVector3(), Quaternion.Euler(data.transform.rotation.ToVector3()));
                    obj.transform.localScale = data.transform.scale.ToVector3();
                    obj.layer = data.layer;
                }
            }
        }
    }

    private void LoadWallsData()
    {
        WallData data;
        var loadedData = saveSystem.LoadData<Dictionary<string, List<WallData>>>();
        if (loadedData != null && loadedData.ContainsKey("walls"))
        {
            wallDataList = loadedData["walls"];
            for (int i = 0; i < wallDataList.Count; i++)
            {
                data = wallDataList[i];

                if (i == 0)
                {
                    menu.floor.transform.position = data.transform.position.ToVector3();
                    menu.floor.transform.rotation = Quaternion.Euler(data.transform.rotation.ToVector3());
                    menu.floor.transform.localScale = data.transform.scale.ToVector3();
                    menu.floor.layer = data.layer;

                    Renderer renderer = menu.floor.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.material.shader);
                        material.color = new Color(data.material.color.r, data.material.color.g, data.material.color.b, data.material.color.a);
                        if (!string.IsNullOrEmpty(data.material.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.material.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.z / 2f, menu.floor.transform.localScale.y / 2f);
                        }
                    }
                }
                else if (i == 1)
                {
                    menu.ceiling.transform.position = data.transform.position.ToVector3();
                    menu.ceiling.transform.rotation = Quaternion.Euler(data.transform.rotation.ToVector3());
                    menu.ceiling.transform.localScale = data.transform.scale.ToVector3();
                    menu.ceiling.layer = data.layer;

                    Renderer renderer = menu.floor.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.material.shader);
                        material.color = new Color(data.material.color.r, data.material.color.g, data.material.color.b, data.material.color.a);
                        if (!string.IsNullOrEmpty(data.material.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.material.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(menu.floor.transform.localScale.z / 2f, menu.floor.transform.localScale.y / 2f);
                        }
                    }
                }
                else
                {
                    GameObject obj = Instantiate(menu.wallPrefab, data.transform.position.ToVector3(), Quaternion.Euler(data.transform.rotation.ToVector3()));
                    obj.transform.localScale = data.transform.scale.ToVector3();
                    obj.layer = data.layer;

                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material material = renderer.material;
                        material.shader = Shader.Find(data.material.shader);
                        material.color = new Color(data.material.color.r, data.material.color.g, data.material.color.b, data.material.color.a);
                        if (!string.IsNullOrEmpty(data.material.texture))
                        {
                            Texture texture = Resources.Load<Texture>(data.material.texture);
                            material.mainTexture = texture;

                            renderer.material.mainTextureScale = new Vector2(obj.transform.localScale.z / 2f, obj.transform.localScale.y / 2f);
                        }
                    }
                }
            }
        }
    }

    private GameObject GetPrefabByName(string name)
    {
        foreach (var prefab in prefabs)
        {
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
        return new Vector3(vectorData.x, vectorData.y, vectorData.z);
    }
}
