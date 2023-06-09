using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{
    [SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    // Aqui se guarda la transformacion previa a la edicion del objeto seleccionado
    //private Transform lastTransform;
    private Vector3 _lastPos;

    // Material original del objeto
    //private Material material;

    // ---------------------

    //private void OnEnable()
    //{
    //    material = meshRenderer.material;
    //}

    // Indica si se puede colocar el objeto o no
    public bool canPlace;

    public MeshRenderer meshRenderer;

    public BoxCollider boxCollider;

    // Si el objeto colisiona con otros objetos, no se puede colocar
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            //Debug.Log("No se puede");
            canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            canPlace = true;
        }
    }

    //Con la X se rota el objeto (30 grados)
    public void RotateObject()
    {
        gameObject.transform.Rotate(Vector3.up, 30);
    }

    public void assignMaterial(Material material)
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
    }

    public void SetPreviousTransform()
    {
        gameObject.transform.position = new Vector3(_lastPos.x, _lastPos.y, _lastPos.z);
        //gameObject.transform.rotation = lastTransform.rotation;
        //gameObject.transform.localScale = lastTransform.localScale;

        //lastTransform = null;
    }
}
