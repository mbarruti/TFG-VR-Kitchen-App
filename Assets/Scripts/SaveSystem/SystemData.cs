using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public List<ObjectData> objectDataList;
    public List<WallData> wallDataList;

    public RoomData(List<ObjectData> objectDataList, List<WallData> wallDataList)
    {
        this.objectDataList = objectDataList;
        this.wallDataList = wallDataList;
    }
}

[System.Serializable]
public class ObjectData
{
    public TransformData transform;
    //public MaterialData material;
    public string prefabId;
    public int layer;

    public ObjectData(Transform transform, string prefabId, int layer)
    {
        this.transform = new TransformData(transform);
        //this.material = new MaterialData(material);
        this.prefabId = prefabId;
        this.layer = layer;
    }
}

public class WallData
{
    public TransformData transform;
    public MaterialData material;
    //public string prefabId;
    public int layer;

    public WallData(Transform transform, Material material, /*string prefabId,*/ int layer)
    {
        this.transform = new TransformData(transform);
        this.material = new MaterialData(material);
        //this.prefabId = prefabId;
        this.layer = layer;
    }
}

[System.Serializable]
public class TransformData
{
    public Vector3Data position;
    public Vector3Data rotation;
    public Vector3Data scale;

    public TransformData(Transform transform)
    {
        position = new Vector3Data(transform.position);
        rotation = new Vector3Data(transform.eulerAngles);
        scale = new Vector3Data(transform.localScale);
    }
}

[System.Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;

    public Vector3Data(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
}

[System.Serializable]
public class MaterialData
{
    public string shader;
    public ColorData color;
    public string texture;

    public MaterialData(Material material)
    {
        shader = material.shader.name;
        color = new ColorData(material.color);
        texture = material.mainTexture != null ? material.mainTexture.name : null;
    }
}

[System.Serializable]
public class ColorData
{
    public float r;
    public float g;
    public float b;
    public float a;

    public ColorData(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }
}

[System.Serializable]
public class GeneralData
{
    public PlayerState playerState;
    public BuildingState buildingState;
    public string mainController;
    public bool continuousRotation;
    public bool assistedControl;

    public GeneralData(PlayerState playerState, BuildingState buildingState, string mainController, bool continuousRotation, bool assistedControl)
    {
        this.playerState = playerState;
        this.buildingState = buildingState;
        this.mainController = mainController;
        this.continuousRotation = continuousRotation;
        this.assistedControl = assistedControl;
    }
}
