using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingObject : MonoBehaviour
{

    [SerializeField] BuildingManager _buildingManager;

    [SerializeField] Outline outline;

    // ---------------------

    public bool canPlace;

    public MeshRenderer meshRenderer;



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



    public void enableOutline()
    {
        outline.enabled = true;
    }
    public void disableOutline()
    {
        outline.enabled = false;
    }
}
