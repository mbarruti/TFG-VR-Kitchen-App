using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallManager : MonoBehaviour
{
    //RaycastHit hit;

    Vector3 _hitPos;

    // Raycast from right controller
    XRRayInteractor rightRay;

    [SerializeField] GameObject ceiling;

    [SerializeField] PlayerManager playerManager;

    [SerializeField] List<BuildingWall> wallList;

    //[SerializeField] List<GameObject> poleList;

    [SerializeField] GameObject originPole;
    [SerializeField] Pole startPole;
    [SerializeField] Pole endPole;
    [SerializeField] GameObject previewPole;
    [SerializeField] List<GameObject> previewPoleList;
    [SerializeField] GameObject wallPrefab;

    private Pole hitPole;
    private GameObject faceHit;

    // -------------------------------------------------

    public RaycastHit hit;

    public BuildingWall wall;

    public RaycastHit wallHit;

    public bool finish = false;

    public bool freePlacement;

    public List<Pole> poleList;

    public GameObject planeHit;

    // Mandos de realidad virtual
    public GameObject rightController;
    public GameObject leftController;

    // Start is called before the first frame update
    void Start()
    {
        rightRay = rightController.GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightRay.TryGetCurrent3DRaycastHit(out hit))
        {
            if (hit.collider.gameObject.CompareTag("Floor")) _hitPos = hit.point;

            // Update position for endPole while finish is true
            if (finish == true)
            {
                // Check if there is a green face that has to be red
                if (hitPole != null)
                {
                    hitPole = null;
                }

                //// Set the Z axis pointing at each other so the wall can be adjusted in that axis
                //startPole.transform.LookAt(endPole.transform.position);
                //endPole.transform.LookAt(startPole.transform.position);
                //var sum = _hitPos + hit.normal * endPole.boxCollider.bounds.extents.y;

                if (freePlacement == false)
                {
                    // If the hit is the floor
                    if (hit.collider.name == "Floor")
                    {
                        Vector3 sum = _hitPos + hit.normal * endPole.boxCollider.bounds.extents.y;

                        wall.SetEndPolePosition(sum, planeHit);

                    }
                    // If the hit is an available pole
                    else if ((endPole.availablePoles.Count > 0 && endPole.availablePoles.Contains(hit.collider.gameObject)) || (previewPoleList.Count > 0 && previewPoleList.Contains(hit.collider.gameObject)))
                    {
                        endPole.transform.position = hit.collider.transform.position;
                    }
                }
                else
                {
                    endPole.transform.position = _hitPos + hit.normal * endPole.boxCollider.bounds.extents.y;
                }

                // Set the Z axis pointing at each other so the wall can be adjusted in that axis
                startPole.transform.LookAt(endPole.transform.position);
                endPole.transform.LookAt(startPole.transform.position);

                // Adjust the width of the wall based on the position of the two poles
                wall.AdjustWall();
            }
            // Get the mid point of the face hit
            else if (hit.collider.gameObject.TryGetComponent(out hitPole))
            {
                if (faceHit != null && faceHit != hitPole.GetClosestFace(hit.point))
                {
                    hitPole.ChangeFaceMaterial(faceHit.GetComponent<MeshRenderer>());
                }

                faceHit = hitPole.GetClosestFace(hit.point);
                if (faceHit.GetComponent<MeshRenderer>().material.color == hitPole.faceMaterials[0].color) hitPole.ChangeFaceMaterial(faceHit.GetComponent<MeshRenderer>());

            }
        }
        // Check if there is a green face that has to be red
        else if (hitPole != null)
        {
            hitPole = null;
        }

        if (faceHit != null && hitPole == null)
        {
            Pole parentPole = faceHit.GetComponentInParent<Pole>();
            if (faceHit.GetComponent<MeshRenderer>().material.color == parentPole.faceMaterials[1].color) parentPole.ChangeFaceMaterial(faceHit.GetComponent<MeshRenderer>());
            faceHit = null;
        }
    }

    //private void FixedUpdate()
    //{

    //    if (rightRay.TryGetCurrent3DRaycastHit(out hit))
    //    {
    //        _hitPos = hit.point;
    //    }
    //}

    public void SetStartPole()
    {
        // If it's the first wall in the world
        if (poleList.Count == 0)
        {
            GameObject auxPole = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            GameObject auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            startPole = auxPole.GetComponent<Pole>();
            endPole = auxPole2.GetComponent<Pole>();

            startPole.transform.position = _hitPos + hit.normal * startPole.boxCollider.bounds.extents.y;
            planeHit.transform.position = _hitPos + hit.normal * startPole.boxCollider.bounds.extents.y;

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            // Instantiate the wall in the world
            //GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            //wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;

            finish = true;
        }
        else if (hit.collider.tag == "Pole")
        {
            wallHit = hit;

            startPole = wallHit.collider.gameObject.GetComponent<Pole>();
            GameObject auxPole2 = Instantiate(originPole, startPole.transform.position, Quaternion.identity);
            endPole = auxPole2.GetComponent<Pole>();

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            planeHit.transform.position = wallHit.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(wallHit.normal, Vector3.up);
            planeHit.transform.rotation = targetRotation;

            endPole.transform.position += Vector3.Scale(planeHit.transform.forward, startPole.transform.localScale);

            // Instantiate the wall in the world
            GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;

            if (poleList.Count >= 3)
            {
                endPole.FilterAvailablePoles(planeHit, startPole);
            }

            finish = true;
        }
    }

    public void SetEndPole()
    {
        finish = false;

        //var aux = Instantiate(startPole, startPole.transform.position, startPole.transform.rotation);
        //var aux2 = Instantiate(endPole, endPole.transform.position, endPole.transform.rotation);

        // Change the layers so the raycast can hit them
        startPole.gameObject.layer = LayerMask.NameToLayer("Default");
        wall.gameObject.layer = LayerMask.NameToLayer("Default");
        //endPole.gameObject.layer = LayerMask.NameToLayer("Default");

        // Destroy every preview pole
        foreach (GameObject pole in previewPoleList)
        {
            Destroy(pole);
        }
        // Clear every element in the preview list
        previewPoleList.Clear();

        //endPole.availablePoles.Clear();

        // If it's the first wall being placed, startPole is added to the list of all poles in the world,
        // else it's already in the list
        if (poleList.Count == 0)
            poleList.Add(startPole);
        //poleList.Add(endPole);

        if (hit.collider.tag == "Pole" && endPole.availablePoles.Count > 0 && endPole.availablePoles.Contains(hit.collider.gameObject))
        {
            startPole.adjacentPoles.Remove(endPole);

            // Destroy the current endPole so it doesn't overlap with the hit pole
            Destroy(endPole.gameObject);

            // New endPole is the hit pole
            endPole = hit.collider.GetComponent<Pole>();
            endPole.adjacentPoles.Add(startPole);

            startPole.adjacentPoles.Add(endPole);
            wall.endPole = endPole;
        }
        else // If it's placed in any free position on the floor
        {
            // Repeat the same process for the endPole
            endPole.gameObject.layer = LayerMask.NameToLayer("Default");
            poleList.Add(endPole);

            // Clear every element in the list
            endPole.availablePoles.Clear();
        }

        wallList.Add(wall);
    }

    public void SetPreviewPole(Vector3 localStartPolePosition, Vector3 localPolePosition, RaycastHit[] hitPoles, GameObject plane)
    {
        bool isPosAvailable;

        Vector3 localPreviewPos = new Vector3(localStartPolePosition.x, localPolePosition.y, localPolePosition.z);
        Vector3 previewPos = plane.transform.TransformPoint(localPreviewPos);

        isPosAvailable = CheckHitPolesPositions(hitPoles, previewPos);
        if (isPosAvailable == true)
        {
            GameObject auxPole = Instantiate(previewPole, previewPos, Quaternion.identity);
            previewPoleList.Add(auxPole);
        }

    }

    /// <summary>
    /// Check if there are any of the Poles hit in the corresponding direction in that position
    /// <summary>
    private bool CheckHitPolesPositions(RaycastHit[] hitPoles, Vector3 position)
    {
        foreach (RaycastHit hitPole in hitPoles)
        {
            if (ApproximateVector(hitPole.transform.position) == ApproximateVector(position))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Cancel the placement of a wall by destroying it and the corresponding poles
    /// <summary>
    public void CancelWallPlacement()
    {
        if (finish == true)
        {
            finish = false;

            if (startPole.adjacentPoles.Count == 1)
            {
                Pole auxStartPole = startPole;
                startPole = null;
                Destroy(auxStartPole.gameObject);
            }
            else
                startPole.adjacentPoles.Remove(endPole);

            Pole auxEndPole = endPole;
            endPole = null;
            Destroy(auxEndPole.gameObject);

            wallList.Remove(wall);

            if (poleList.Count == 0)
            {
                wall.transform.position = new Vector3(0, -20f, 0);
                wall.transform.localScale = new Vector3(0.1f, 4f, 0.1f);
            }
            else
            {
                BuildingWall auxWall = wall;
                wall = null;
                Destroy(auxWall.gameObject);
            }

            // Destroy every preview pole
            foreach (GameObject pole in previewPoleList)
            {
                Destroy(pole);
            }
            // Clear every element in the preview list
            previewPoleList.Clear();
        }
    }

    /// <summary>
    /// Destroy the selected wall
    /// <summary>
    public void DestroyWall(BuildingWall buildingWall)
    {
        if (finish == false)
        {
            GameObject auxStartPole = null;
            GameObject auxEndPole = null;

            // If any of the two poles connected to the selected wall have more than one adjacent pole, it is not destroyed
            if (buildingWall.startPole.adjacentPoles.Count < 2)
            {
                // If it has only one, its reference is saved in a local variable (to destroy it later) so the next pole doesn't get a null/missing in its list
                auxStartPole = buildingWall.startPole.gameObject;

                poleList.Remove(buildingWall.startPole);

                foreach (Pole pole in buildingWall.startPole.adjacentPoles)
                {
                    if (pole != buildingWall.endPole) pole.adjacentPoles.Remove(buildingWall.startPole);
                }

                if (startPole == buildingWall.startPole)
                    startPole = null;
            }
            //else
            //{
            //    // Remove each other from the others adjacent list
            //    buildingWall.startPole.adjacentPoles.Remove(buildingWall.endPole);
            //}

            if (buildingWall.endPole.adjacentPoles.Count < 2)
            {
                auxEndPole = buildingWall.endPole.gameObject;

                poleList.Remove(buildingWall.endPole);

                foreach (Pole pole in buildingWall.endPole.adjacentPoles)
                {
                    pole.adjacentPoles.Remove(buildingWall.endPole);
                }

                if (endPole == buildingWall.endPole)
                    endPole = null;
            }
            else
            {
                // Remove each other from the others adjacent list
                buildingWall.endPole.adjacentPoles.Remove(buildingWall.startPole);
                buildingWall.startPole.adjacentPoles.Remove(buildingWall.endPole);
            }

            //// Remove each other from the others adjacent list
            //buildingWall.startPole.adjacentPoles.Remove(buildingWall.endPole);
            //endPole.adjacentPoles.Remove(buildingWall.startPole);

            // If the condition above is true for any of them, it is destroyed
            if (auxStartPole != null)
            {
                Destroy(auxStartPole);
            }
            if (auxEndPole != null)
            {
                Destroy(auxEndPole);
            }

            // Destroy the wall
            if (wallList.Count > 1)
                Destroy(buildingWall.gameObject);
            else
            {
                buildingWall.startPole = null;
                buildingWall.endPole = null;

                buildingWall.transform.position = new Vector3(0, -20f, 0);
                buildingWall.transform.localScale = new Vector3(0.1f, 4f, 0.1f);
                buildingWall.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                wall = buildingWall;
            }
            wallList.Remove(buildingWall);

            planeHit.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }

    /// <summary>
    /// Sets the height of the ceiling when before finishing building walls
    /// 
    public void SetCeiling()
    {
        ceiling.SetActive(true);
        ceiling.transform.position = new Vector3(0, wall.transform.localScale.y + 1.0f, 0f);
    }

    public void DeleteAllPoles()
    {
        foreach (Pole pole in poleList)
        {
            Destroy(pole.gameObject);
        }

        poleList.Clear();

        foreach (BuildingWall buildingWall in wallList)
        {
            float distance = Vector3.Distance(buildingWall.startPole.transform.position, buildingWall.endPole.transform.position);

            buildingWall.startPole = null;
            buildingWall.endPole = null;

            buildingWall.transform.localScale = new Vector3(buildingWall.transform.localScale.x, buildingWall.transform.localScale.y, distance + 0.1f);
        }
    }

    public Vector3 ApproximateVector(Vector3 vector)
    {
        return new Vector3(Approximate(vector.x), Approximate(vector.y), Approximate(vector.z));
    }

    public float Approximate(float value)
    {
        return Mathf.Round(value * Mathf.Pow(10, 4)) / Mathf.Pow(10, 4);
    }
}
