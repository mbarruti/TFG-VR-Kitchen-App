using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallManager : MonoBehaviour
{
    RaycastHit hit;

    Vector3 _hitPos;

    // Raycast from right controller
    XRRayInteractor rightRay;

    [SerializeField] PlayerManager playerManager;

    //[SerializeField] List<GameObject> poleList;

    [SerializeField] GameObject originPole;
    [SerializeField] Pole startPole;
    [SerializeField] Pole endPole;
    [SerializeField] GameObject previewPole;
    [SerializeField] List<GameObject> previewPoleList;
    [SerializeField] GameObject wallPrefab;

    // -------------------------------------------------

    public BuildingWall wall;

    public RaycastHit wallHit;

    public bool finish = false;

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
                //var sum = _hitPos + hit.normal * endPole.boxCollider.bounds.extents.y;

                // If the hit is the floor
                if (hit.collider.name == "Floor")
                {
                    var sum = _hitPos + hit.normal * endPole.boxCollider.bounds.extents.y;
                    //var sum = _hitPos + hit.normal * endPole.GetComponent<BoxCollider>().bounds.extents.y;

                    wall.SetEndPolePosition(sum);

                }
                // If the hit is an available pole
                else if ((endPole.availablePoles.Count > 0 && endPole.availablePoles.Contains(hit.collider.gameObject)) || (previewPoleList.Count > 0 && previewPoleList.Contains(hit.collider.gameObject)))
                {
                    endPole.transform.position = hit.collider.transform.position;
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
            var auxPole = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            var auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            startPole = auxPole.GetComponent<Pole>();
            endPole = auxPole2.GetComponent<Pole>();

            startPole.transform.position = _hitPos + hit.normal * startPole.boxCollider.bounds.extents.y;

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            //finish = true;

            //// Instantiate the wall in the world
            //GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            //wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;

            finish = true;
        }
        else if (hit.collider.tag == "Wall")
        {
            wallHit = hit;
            //Debug.Log(wallHit.normal);

            startPole = wallHit.collider.gameObject.GetComponent<Pole>();
            var auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
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

        startPole.gameObject.layer = LayerMask.NameToLayer("Default");
        endPole.gameObject.layer = LayerMask.NameToLayer("Default");

        foreach (GameObject pole in previewPoleList)
        {
            Destroy(pole);
        }

        previewPoleList.Clear();
        endPole.availablePoles.Clear();

        if (poleList.Count == 0) poleList.Add(startPole);
        poleList.Add(endPole);

        //startPole.transform.position = new Vector3(0, -20, 0);
        //endPole.transform.position = new Vector3(0, -20, 0);

        //startPole.transform.eulerAngles = new Vector3(0, 0, 0);
        //endPole.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void SetPreviewPole(Pole startPole, Pole pole, RaycastHit[] hitPoles)
    {
        //Debug.Log(wallHit.normal);

        bool isPosAvailable;

        // Threshold to approximate the direction with the small errors caused by the rotation
        float threshold = 0.01f;

        if (Mathf.Abs(wallHit.normal.x - 1f) < threshold || Mathf.Abs(wallHit.normal.x + 1f) < threshold)
        //if (wallHit.normal.x == 1f || wallHit.normal.x == -1f)
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
    /// Check if there is any of the Poles hit in the corresponding direction in that position
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

    private float GetAxis(Vector3 normal, Vector3 vector)
    {
        // Encontrar el eje dominante de la normal
        float maxAxis = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        if (Mathf.Abs(normal.x) == maxAxis)
        {
            return vector.x;
        }
        else if (Mathf.Abs(normal.y) == maxAxis)
        {
            return vector.y;
        }
        else if (Mathf.Abs(normal.z) == maxAxis)
        {
            return vector.z;
        }
        //Debug.Log("ninguna");
        return 0;
    }

    /// <summary>
    /// Approximate the direction
    /// </summary>
    private Vector3 ApproximateNormal(Vector3 normal)
    {
        // Threshold to approximate the direction with the small errors (caused by the rotation, for example)
        float threshold = 0.01f;

        if (Mathf.Abs(wallHit.normal.x - 1f) < threshold)
            return new Vector3(1f, 0f, 0f);
        else if (Mathf.Abs(wallHit.normal.x + 1f) < threshold)
            return new Vector3(-1f, 0f, 0f);
        else if (Mathf.Abs(wallHit.normal.y - 1f) < threshold)
            return new Vector3(0f, 1f, 0f);
        else if (Mathf.Abs(wallHit.normal.y + 1f) < threshold)
            return new Vector3(0f, -1f, 0f);
        else if (Mathf.Abs(wallHit.normal.z - 1f) < threshold)
            return new Vector3(0f, 0f, 1f);
        else if (Mathf.Abs(wallHit.normal.z + 1f) < threshold)
            return new Vector3(0f, 0f, -1f);

        Debug.Log("Direccion aproximada: " + new Vector3(0f, 0f, 0f));
        return new Vector3(0f, 0f, 0f);

    }
}
