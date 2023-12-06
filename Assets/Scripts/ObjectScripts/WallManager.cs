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
    [SerializeField] GameObject wallPrefab;

    private RaycastHit wallHit;

    // -------------------------------------------------

    public BuildingWall wall;

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
        }

        // Set position for endPole
        if (finish == true)
        {
            if (poleList.Count == 0)
            {
                var sum = _hitPos + hit.normal * endPole.GetComponent<BoxCollider>().bounds.extents.y;
                if (wall.axisX == true)
                {
                    //var sum = _hitPos + GetOffset(hit.normal, endPole.GetComponent<BoxCollider>());
                    endPole.transform.position = new Vector3(sum.x, sum.y, startPole.transform.position.z);
                }
                else
                {
                    //var sum = _hitPos + GetOffset(hit.normal, endPole.GetComponent<BoxCollider>());
                    endPole.transform.position = new Vector3(sum.x, sum.y, startPole.transform.position.z);
                    endPole.transform.position = new Vector3(startPole.transform.position.x, sum.y, sum.z);
                }

            }
            else
            {
                var sum = _hitPos + hit.normal * endPole.GetComponent<BoxCollider>().bounds.extents.y;

                // Check whether is moved in the X or Z axis based on the direction of the hit
                if (GetAxis(wallHit.normal, sum) == sum.x)
                {
                    endPole.transform.position = new Vector3(sum.x, sum.y, startPole.transform.position.z);

                    // If a pole is hit, is not the same pole we started from and the pole hit and the wall we are placing are aligned in the Z axis
                    if (hit.collider.tag == "Wall" && hit.collider.gameObject != wallHit.collider.gameObject && endPole.transform.position.z == hit.collider.transform.position.z)
                    {
                        endPole.transform.position = hit.collider.transform.position;
                    }
                }
                else if (GetAxis(wallHit.normal, sum) == sum.z)
                {
                    endPole.transform.position = new Vector3(startPole.transform.position.x, sum.y, sum.z);

                    // If a pole is hit, is not the same pole we started from and the pole hit and the wall we are placing are aligned in the X axis
                    if (hit.collider.tag == "Wall" && hit.collider.gameObject != wallHit.collider.gameObject && endPole.transform.position.x == hit.collider.transform.position.x)
                    {
                        endPole.transform.position = hit.collider.transform.position;
                    }
                }
            }

            startPole.transform.LookAt(endPole.transform.position);
            endPole.transform.LookAt(startPole.transform.position);

            // Adjust the width of the wall based on the position of the two poles
            wall.AdjustWall();
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

            finish = true;

            //// Instantiate the wall in the world
            //GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            //wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;
        }
        else if (hit.collider.tag == "Wall")
        {
            wallHit = hit;

            startPole = wallHit.collider.gameObject.GetComponent<Pole>();
            var auxPole2 = Instantiate(originPole, originPole.transform.position, originPole.transform.rotation);
            endPole = auxPole2.GetComponent<Pole>();

            startPole.adjacentPoles.Add(endPole);
            endPole.adjacentPoles.Add(startPole);

            //startPole.transform.position = hit.collider.bounds.center;
            //startPole.transform.rotation = hit.collider.gameObject.transform.rotation;

            finish = true;

            // Instantiate the wall in the world
            GameObject auxWall = Instantiate(wallPrefab, startPole.transform.position, Quaternion.identity);
            wall = auxWall.GetComponent<BuildingWall>();
            wall.startPole = startPole;
            wall.endPole = endPole;
        }
    }

    public void SetEndPole()
    {
        finish = false;

        //var aux = Instantiate(startPole, startPole.transform.position, startPole.transform.rotation);
        //var aux2 = Instantiate(endPole, endPole.transform.position, endPole.transform.rotation);

        startPole.gameObject.layer = LayerMask.NameToLayer("Default");
        endPole.gameObject.layer = LayerMask.NameToLayer("Default");

        if (poleList.Count == 0) poleList.Add(startPole);
        poleList.Add(endPole);

        //startPole.transform.position = new Vector3(0, -20, 0);
        //endPole.transform.position = new Vector3(0, -20, 0);

        //startPole.transform.eulerAngles = new Vector3(0, 0, 0);
        //endPole.transform.eulerAngles = new Vector3(0, 0, 0);
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

        return 0;
    }
}
