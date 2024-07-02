using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{
    //[SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    [SerializeField] Material[] collisionMaterials;

    [SerializeField] bool twoAxisScalling;

    // Aqui se guardan las transformaciones previas a la edicion del objeto seleccionado
    //private Transform lastTransform;
    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    //private Vector3 localMovement;
    private float touchpadValueX;
    private float touchpadValueY;

    // ------------------------------------------------

    public GameObject surfaceObject;
    public Vector3 surfaceNormal;

    public bool rotationLocked;

    public Vector3[] vertices = new Vector3[8]; // List of vertices of the box collider
    public Vector3[] faces = new Vector3[6];

    public BoxCollider[] interactables;

    public BuildingManager _buildingManager;

    // Indica si se puede posicionar donde este el CollisionManager
    public bool canPlace;

    // If it can be placed in the world (por ahora solo con withTrigger)
    public bool canBePlaced;

    public MeshRenderer meshRenderer;

    public BoxCollider boxCollider;

    public List<BoxCollider> colliderList = new List<BoxCollider>();

    public Rigidbody objectRigidbody;

    public bool isInteractable;

    public bool isPlaced;

    public bool isInLimit;

    public int dataIndex;

    //public Vector3 offset;

    public List<Collider> detectedColliders;

    private void Start()
    {
        //Debug.Log("hitPos del Update:" + offset);
        //transform.position = _buildingManager._hitPos;
        //transform.position = new Vector3(0, 0, 0);

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

    /// <summary>
    /// Rotate the objects forward vector so its direction is the same as the perpendicular vector of the surface
    /// </summary>
    public void SetForwardAxisDirection()
    {
        if (surfaceObject.CompareTag("Wall")) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(surfaceNormal, Vector3.up);
            transform.rotation = targetRotation;

            rotationLocked = true;
        }
        else if (surfaceObject.CompareTag("Floor") || surfaceObject.CompareTag("Ceiling"))
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);

            rotationLocked = false;
        }
    }

    public void SetTouchpadValues(float valueX, float valueY)
    {
        touchpadValueX = valueX;
        touchpadValueY = valueY;
    }

    /// <summary>
    /// Move the physics based object with the touchpad
    /// </summary>
    public void MoveWithTouchpad()
    {
        Vector3 worldMovement = Vector3.zero;

        if (surfaceObject.CompareTag("Wall"))
        {
            //Vector3 localMovement = Vector3.zero;

            Vector3 localMovement = new Vector3(-touchpadValueX, touchpadValueY, 0f);

            // Convertir el desplazamiento local a coordenadas globales
            worldMovement = transform.TransformDirection(localMovement);

            // Calcular la nueva posición sumando el desplazamiento a la posición actual
            // nextPosition = 5f * Time.deltaTime * worldMovement + transform.position;

            // Mover el objeto utilizando Rigidbody.MovePosition
            //objectRigidbody.MovePosition(nextPosition);
        }
        else if (surfaceObject.CompareTag("Floor") || surfaceObject.CompareTag("Ceiling"))
            worldMovement = new Vector3(touchpadValueX, 0f, touchpadValueY);

        Vector3 nextPosition = 5f * Time.deltaTime * worldMovement + transform.position;

        // Mover el objeto utilizando el rigidbody
        objectRigidbody.MovePosition(nextPosition);
    }

    /// <summary>
    /// Rotate the object in the Y axis
    /// </summary>
    public void RotateObject(float value)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + value, transform.eulerAngles.z);
        _buildingManager.parentObject.RotateObject(value);
    }

    // Scale object
    public void ScaleObject(Vector2 value, bool leftTriggerPressed)
    {
        //float scaleAmountY = 0f;
        //float scaleAmountX = 0f;

        Vector3 scaleAmount = Vector3.zero;

        if (twoAxisScalling == true && leftTriggerPressed)
        {
            // Y axis scaling depends on Y value of the touchpad
            if (value.y >= 0.8f || value.y <= -0.8f)
            {
                //if (transform.localScale.y - scaleAmount.y >= 0.15f) scaleAmount.y = value.y * Time.deltaTime;
                //else scaleAmount.y += 0.15f - transform.localScale.y;
                scaleAmount.y = value.y * Time.deltaTime;
            }
            // X axis scaling depends on X value of the touchpad
            if (value.x >= 0.8f || value.x <= -0.8f)
            {
                //if (transform.localScale.x - scaleAmount.x >= 0.15f) scaleAmount.x = value.x * Time.deltaTime;
                //else scaleAmount.x += 0.15f - transform.localScale.x;
                scaleAmount.x = value.x * Time.deltaTime;
            }
        }
        else // Every axis is scaled depending on the Y value of the touchpad
        {
            //if (transform.localScale.x - scaleAmount.x >= 0.15f) scaleAmount.x = value.y * Time.deltaTime;
            //else scaleAmount.x += 0.15f - transform.localScale.x;

            //if (transform.localScale.y - scaleAmount.y >= 0.15f) scaleAmount.y = value.y * Time.deltaTime;
            //else scaleAmount.y += 0.15f - transform.localScale.y;

            //if (transform.localScale.z - scaleAmount.z >= 0.15f) scaleAmount.z = value.y * Time.deltaTime;
            //else scaleAmount.z += 0.15f - transform.localScale.z;

            scaleAmount.x = value.y * Time.deltaTime;
            scaleAmount.y = value.y * Time.deltaTime;
            scaleAmount.z = value.y * Time.deltaTime;
        }

        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + scaleAmount.x, 0.15f, 2f), Mathf.Clamp(transform.localScale.y + scaleAmount.y, 0.15f, 2f), Mathf.Clamp(transform.localScale.z + scaleAmount.z, 0.15f, 2f));
    }

    public void AssignMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public void switchOutline(bool enabled)
    {
        outline.enabled = enabled;
    }

    public void SavePreviousTransform()
    {
        _lastPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        _lastRot = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        _lastScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    public void SetPreviousTransform()
    {
        gameObject.transform.position = new Vector3(_lastPos.x, _lastPos.y, _lastPos.z);
        gameObject.transform.eulerAngles = new Vector3(_lastRot.x, _lastRot.y, _lastRot.z);
        gameObject.transform.localScale = new Vector3(_lastScale.x, _lastScale.y, _lastScale.z);
    }

    public void DisableColliders()
    {
        foreach(BoxCollider subCollider in colliderList)
        {
            subCollider.enabled = false;
        }
    }

    public void EnableColliders()
    {
        foreach (BoxCollider subCollider in colliderList)
        {
            subCollider.enabled = true;
        }
    }

    public void ChangeObjectLayer()
    {
        if (colliderList.Count > 0)
        {
            boxCollider.enabled = false;

            foreach (BoxCollider boxColl in colliderList)
            {
                boxColl.enabled = true;
                boxColl.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        foreach (BoxCollider inter in interactables)
        {
            inter.gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }
}
