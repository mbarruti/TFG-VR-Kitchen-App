using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{ 

    [SerializeField]
    private BuildingManager _buildingManager;

    // ---------------------

    public bool canPlace;

    public MeshRenderer meshRenderer;

    //Con la X se rota el objeto (30 grados)
    public void RotateObject()
    {
        gameObject.transform.Rotate(Vector3.up, 30);
    }

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

    public void assignMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
