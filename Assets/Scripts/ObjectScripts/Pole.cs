using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] WallManager wallManager;

    //[SerializeField] List<Pole> availablePoles;

    //[SerializeField] Material[] faceMaterials;

    [SerializeField] GameObject[] faces = new GameObject[4];

    private Vector3[] vertices = new Vector3[8];

    // ------------------------------------

    //public List<Pole> availablePoles;

    public Material[] faceMaterials;

    public List<GameObject> availablePoles;

    public List<Pole> adjacentPoles;

    public float activeAxis;

    public BoxCollider boxCollider;

    // TO-DO: lo que esta comentado de antes ignorar, solo se va hacer la lista availablePoles para el Pole que actua como endPole en la direccion que se haya elegido dependiendo de la cara de startPole
    // a lo mejor una funcion que se llama desde SetStartPole para colocar los PreviewPole correspondientes y otra para dibujar la linea en cada frame a dichos PreviewPole o al Pole correspondiente:
    /// <summary>
    /// Filter the available poles in the world to connect to for this pole
    /// <summary>
    //public List<Pole> FilterAvailablePoles(Vector3 direction, Pole startPole)

    private void Start()
    {
        SetBoxVertices();
    }

    // Set the collider vertices
    void SetBoxVertices()
    {
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size * 0.5f; // Divide to get half of the collider

        //Vector3[] vertices = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            float x = ((i & 1) == 0) ? size.x : -size.x;
            float y = ((i & 2) == 0) ? size.y : -size.y;
            float z = ((i & 4) == 0) ? size.z : -size.z;

            vertices[i] = center + new Vector3(x, y, z);
            //Debug.Log(vertices[i]);
        }
    }

    public GameObject GetClosestFace(Vector3 hitPoint)
    {
        GameObject nearestFace = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject face in faces)
        {
            float distance = Vector3.Distance(hitPoint, face.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestFace = face;
            }
        }

        return nearestFace;
    }

    public void ChangeFaceMaterial(MeshRenderer faceRenderer)
    {
        faceRenderer.material = (faceRenderer.material.color == faceMaterials[0].color) ? faceMaterials[1] : faceMaterials[0];
        //if (faceRenderer.material.color == faceMaterials[0].color) faceRenderer.material = faceMaterials[1];
        //else faceRenderer.material = faceMaterials[0];
    }

    //// Get the midpoint of the face hit by the raycast
    //public Vector3 GetFaceMidpoint(Vector3 hitPoint)
    //{
    //    // Encuentra la cara más cercana al punto de impacto
    //    int nearestFace = FindNearestFace(transform.InverseTransformPoint(hitPoint));

    //    // Suma los vértices de la cara para obtener el punto medio
    //    Vector3 faceMidpoint = Vector3.zero;
    //    for (int i = 0; i < 4; i++)
    //    {
    //        faceMidpoint += vertices[(nearestFace * 4) + i];
    //    }
    //    faceMidpoint /= 4.0f;

    //    return transform.TransformPoint(faceMidpoint);
    //}

    //// Encuentra la cara más cercana al punto de impacto
    //int FindNearestFace(Vector3 hitPoint)
    //{
    //    float minDistance = float.MaxValue;
    //    int nearestFace = 0;

    //    for (int i = 0; i < 6; i++)
    //    {
    //        // Calcula el punto medio de la cara actual
    //        Vector3 faceMidpoint = CalculateFaceMidpoint(i);

    //        // Calcula la distancia al punto de impacto
    //        float distance = Vector3.Distance(hitPoint, faceMidpoint);

    //        // Actualiza si la distancia es menor
    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            nearestFace = i;
    //        }
    //    }

    //    return nearestFace;
    //}

    //// Calcula el punto medio de la cara
    //Vector3 CalculateFaceMidpoint(int faceIndex)
    //{
    //    return (vertices[faceIndex * 4] + vertices[faceIndex * 4 + 1] + vertices[faceIndex * 4 + 2] + vertices[faceIndex * 4 + 3]) / 4.0f;
    //}

    public void FilterAvailablePoles(Vector3 direction, Pole startPole)
    {
        RaycastHit[] hitPoles;
        hitPoles = Physics.RaycastAll(wallManager.wallHit.collider.transform.position, wallManager.wallHit.normal);

        float axisPos1;
        float axisPos2;

        //float directionAxis;

        //directionAxis = Vector3.Dot(direction, Vector3.one);
        //Debug.Log(direction);
        //Debug.Log(directionAxis);

        Vector3 directionAbs = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

        axisPos1 = Vector3.Dot(startPole.transform.position, directionAbs);
        //Debug.Log(axisPos1);

        // Filter the available poles in the world to connect to for this pole
        foreach (Pole pole in wallManager.poleList)
        {
            if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 4)
            {
                axisPos2 = Vector3.Dot(pole.transform.position, directionAbs);

                if (activeAxis > 0 && axisPos1 < axisPos2)
                {
                    //Debug.Log(axisPos2);
                    if (IsSecondAxisAligned(direction, activeAxis, startPole, pole) == true)
                    {
                        //pole.gameObject.layer = LayerMask.NameToLayer("Default");
                        availablePoles.Add(pole.gameObject);
                    }
                    else wallManager.SetPreviewPole(startPole, pole, hitPoles);
                }
                else if (activeAxis < 0 && axisPos1 > axisPos2)
                {
                    //Debug.Log(axisPos2);
                    if (IsSecondAxisAligned(direction, activeAxis, startPole, pole) == true)
                    {
                        //pole.gameObject.layer = LayerMask.NameToLayer("Default");
                        availablePoles.Add(pole.gameObject);
                    }
                    else wallManager.SetPreviewPole(startPole, pole, hitPoles);
                }
            }
        }

        //return availablePoles;
    }

    public void SetActiveAxisSign(BuildingWall wall, Vector3 normal)
    {
        if (wall.axisX == true)
            activeAxis = normal.x;
        else
            activeAxis = normal.z;
    }

    private bool IsSecondAxisAligned(Vector3 direction, float directionAxis, Pole startPole, Pole pole)
    {
        if (direction.x == directionAxis && startPole.transform.position.z == pole.transform.position.z)
            return true;
        if (direction.z == directionAxis && startPole.transform.position.x == pole.transform.position.x)
            return true;
        return false;
    }

    // A lo mejor esta va en WallManager
    /// <summary>
    /// Set the PreviewPoles that align with other poles
    /// <summary>
    //public void SetPreviewPoles()
    //{

    //}
}
