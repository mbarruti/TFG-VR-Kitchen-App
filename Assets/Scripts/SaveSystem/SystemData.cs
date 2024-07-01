using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public List<ObjectData> objects;
    public List<WallData> walls;

    public RoomData(List<ObjectData> objectDataList, List<WallData> wallDataList)
    {
        this.objects = objectDataList;
        this.walls = wallDataList;
    }
}

[System.Serializable]
public class ObjectData
{
    public List<double> position { get; set; }
    public List<double> rotation { get; set; }
    public List<double> scale { get; set; }
    public string prefabId { get; set; }
    public int layer { get; set; }
}

[System.Serializable]
public class WallData
{
    public List<double> position { get; set; }
    public List<double> rotation { get; set; }
    public List<double> scale { get; set; }
    public string shader { get; set; }
    public string texture { get; set; }
    public int layer { get; set; }

}

[System.Serializable]
public class TransformData
{
    public float[] position = new float[3];
    public float[] rotation = new float[3];
    public float[] scale = new float[3];

    public TransformData(Transform transform)
    {
        position[0] = transform.position.x;
        position[1] = transform.position.y;
        position[2] = transform.position.z;

        rotation[0] = transform.eulerAngles.x;
        rotation[1] = transform.eulerAngles.y;
        rotation[2] = transform.eulerAngles.z;

        scale[0] = transform.localScale.x;
        scale[1] = transform.localScale.y;
        scale[2] = transform.localScale.z;
    }
}

[System.Serializable]
public class Vector3Data
{
    public float[] values = new float[3];

    public Vector3Data(Vector3 vector)
    {
        values[0] = vector.x;
        values[1] = vector.y;
        values[2] = vector.z;
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
