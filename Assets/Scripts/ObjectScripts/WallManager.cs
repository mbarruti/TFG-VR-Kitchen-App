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

    [SerializeField] PlayerManager playerManager;

    [SerializeField] List<BuildingWall> wallList;

    //[SerializeField] List<GameObject> poleList;

    [SerializeField] GameObject originPole;
    [SerializeField] Pole startPole;
    [SerializeField] Pole endPole;
    [SerializeField] GameObject previewPole;
    [SerializeField] List<GameObject> previewPoleList;
    [SerializeField] GameObject wallPrefab;

    // -------------------------------------------------

    public RaycastHit hit;

    public BuildingWall wall;

    public RaycastHit wallHit;

    public bool finish = false;

    public bool freePlacement;

    public List<Pole> poleList;

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
            _hitPos = hit.point;

            // Update position for endPole while finish is true
            if (finish == true)
            {
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

                        wall.SetEndPolePosition(sum);

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
        if (poleList.Count == 0)
        {
            GameObject auxPole = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            GameObject auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            startPole = auxPole.GetComponent<Pole>();
            endPole = auxPole2.GetComponent<Pole>();

            startPole.transform.position = _hitPos + hit.normal * startPole.boxCollider.bounds.extents.y;

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            //finish = true;

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
            //Debug.Log(wallHit.normal);

            startPole = wallHit.collider.gameObject.GetComponent<Pole>();
            GameObject auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            endPole = auxPole2.GetComponent<Pole>();

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            //startPole.transform.position = hit.collider.bounds.center;
            //startPole.transform.rotation = hit.collider.gameObject.transform.rotation;

            // Instantiate the wall in the world
            GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;

            wall.SetActiveAxis(ApproximateNormal(wallHit.normal));

            if (poleList.Count >= 3)
            {
                //List<Pole> currentPoleList = endPole.FilterAvailablePoles(ApproximateNormal(wallHit.normal), startPole);

                // Set preview poles
                //foreach (Pole pole in currentPoleList)
                //{
                //    SetPreviewPole(startPole, pole, hitPoles);
                //}

                endPole.FilterAvailablePoles(ApproximateNormal(wallHit.normal), startPole);
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

    public void SetPreviewPole(Pole startPole, Pole pole, RaycastHit[] hitPoles)
    {
        //Debug.Log(wallHit.normal);

        bool isPosAvailable;

        Vector3 normal = ApproximateNormal(wallHit.normal);
        //// Threshold to approximate the direction with the small errors caused by the rotation
        //float threshold = 0.01f;

        //if (Mathf.Abs(wallHit.normal.x - 1f) < threshold || Mathf.Abs(wallHit.normal.x + 1f) < threshold)
        if (normal.x == 1f || normal.x == -1f)
        {
            //Debug.Log("Dir X");
            var previewPos = new Vector3(pole.transform.position.x, pole.transform.position.y, startPole.transform.position.z);

            isPosAvailable = CheckHitPolesPositions(hitPoles, previewPos);
            if (isPosAvailable == true)
            {
                GameObject auxPole = Instantiate(previewPole, previewPos, Quaternion.identity);
                previewPoleList.Add(auxPole);
            }
        }
        else
        {
            //Debug.Log(wallHit.normal.x);
            //Debug.Log("Dir Z");
            //Debug.Log(endPole.transform.position.x);
            var previewPos = new Vector3(startPole.transform.position.x, pole.transform.position.y, pole.transform.position.z);

            isPosAvailable = CheckHitPolesPositions(hitPoles, previewPos);
            if (isPosAvailable == true)
            {
                GameObject auxPole = Instantiate(previewPole, previewPos, Quaternion.identity);
                previewPoleList.Add(auxPole);
            }
        }
    }

    /// <summary>
    /// Check if there are any of the Poles hit in the corresponding direction in that position
    /// <summary>
    private bool CheckHitPolesPositions(RaycastHit[] hitPoles, Vector3 position)
    {
        foreach (RaycastHit hitPole in hitPoles)
        {
            if (hitPole.transform.position == position)
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
                // If it has only one, its reference is saved in a local variable (to destroy it later) so the next pole doesn't get a null in its list
                auxStartPole = buildingWall.startPole.gameObject;

                poleList.Remove(buildingWall.startPole);

                foreach (Pole pole in buildingWall.startPole.adjacentPoles)
                {
                    pole.adjacentPoles.Remove(buildingWall.startPole);
                }

                if (startPole == buildingWall.startPole)
                    startPole = null;
            }
            else
            {
                // Remove each other from the others adjacent list
                buildingWall.startPole.adjacentPoles.Remove(buildingWall.endPole);
            }

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

            wallList.Remove(wall);

            // Destroy the wall
            if (poleList.Count > 2)
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
        }
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

    //private float GetAxis(Vector3 normal, Vector3 vector)
    //{
    //    // Encontrar el eje dominante de la normal
    //    float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

    //    if (Mathf.Abs(normal.x) == maxAxis)
    //    {
    //        return vector.x;
    //    }
    //    else if (Mathf.Abs(normal.y) == maxAxis)
    //    {
    //        return vector.y;
    //    }
    //    else if (Mathf.Abs(normal.z) == maxAxis)
    //    {
    //        return vector.z;
    //    }
    //    //Debug.Log("ninguna");
    //    return 0;
    //}

    /// <summary>
    /// Approximate the direction
    /// </summary>
    private Vector3 ApproximateNormal(Vector3 normal)
    {
        // Threshold to approximate the direction with the small errors (caused by the rotation, for example)
        float threshold = 0.01f;

        if (Mathf.Abs(normal.x - 1f) < threshold)
            return new Vector3(1f, 0f, 0f);
        else if (Mathf.Abs(normal.x + 1f) < threshold)
            return new Vector3(-1f, 0f, 0f);
        else if (Mathf.Abs(normal.y - 1f) < threshold)
            return new Vector3(0f, 1f, 0f);
        else if (Mathf.Abs(normal.y + 1f) < threshold)
            return new Vector3(0f, -1f, 0f);
        else if (Mathf.Abs(normal.z - 1f) < threshold)
            return new Vector3(0f, 0f, 1f);
        else if (Mathf.Abs(normal.z + 1f) < threshold)
            return new Vector3(0f, 0f, -1f);

        Debug.Log("Direccion aproximada: " + new Vector3(0f, 0f, 0f));
        return new Vector3(0f, 0f, 0f);

    }
}
