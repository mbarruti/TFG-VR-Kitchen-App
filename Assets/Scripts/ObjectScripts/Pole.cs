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

    public void FilterAvailablePoles(GameObject plane, Pole startPole)
    {
        RaycastHit[] hitPoles;
        hitPoles = Physics.RaycastAll(startPole.transform.position, plane.transform.forward);

        Vector3 localStartPolePosition = plane.transform.InverseTransformPoint(startPole.transform.position);

        foreach (Pole pole in wallManager.poleList)
        {
            if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 4)
            {
                Vector3 localPolePosition = plane.transform.InverseTransformPoint(pole.transform.position);

                if (wallManager.Approximate(localStartPolePosition.z) < wallManager.Approximate(localPolePosition.z))
                {
                    if (wallManager.Approximate(localStartPolePosition.x) == wallManager.Approximate(localPolePosition.x))
                    {
                        //pole.gameObject.layer = LayerMask.NameToLayer("Default");
                        availablePoles.Add(pole.gameObject);
                    }
                    else wallManager.SetPreviewPole(localStartPolePosition, localPolePosition, hitPoles, plane);
                }
            }
        }
    }
}
