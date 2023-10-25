using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    //[SerializeField] BuildingManager buildingManager;

    private Vector3 _lastPos;
    private Vector3 _lastRot;
    private Vector3 _lastScale;

    // -------------------------------------------

    public BoxCollider boxCollider;

    public List<Collider> detectedColliders;

    private void OnCollisionStay(Collision collision)
    {
        Collider collider = collision.collider;
        if (/*collision.collider != _buildingManager.hit.collider && */!detectedColliders.Contains(collider))
        {
            detectedColliders.Add(collider);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        Debug.Log("sale");
        if (detectedColliders.Contains(collider))
        {
            detectedColliders.Remove(collider);
        }
    }

    //Con el trigger izquierdo se rota el objeto en el eje Y (30 grados)
    public void RotateObject()
    {
        gameObject.transform.Rotate(Vector3.up, 30);
    }

    // Escala según el valor del eje Y del mando derecho
    public void ScaleObject(float value)
    {
        float scaleAmount = value * Time.deltaTime;
        transform.localScale += Vector3.one * scaleAmount;
    }

    //public void SavePreviousTransform()
    //{
    //    _lastPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    //    _lastRot = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    //    _lastScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    //}

    //public void SetPreviousTransform()
    //{
    //    gameObject.transform.position = new Vector3(_lastPos.x, _lastPos.y, _lastPos.z);
    //    gameObject.transform.eulerAngles = new Vector3(_lastRot.x, _lastRot.y, _lastRot.z);
    //    gameObject.transform.localScale = new Vector3(_lastScale.x, _lastScale.y, _lastScale.z);
    //}

    public void ResetTransform()
    {
        boxCollider.transform.position = new Vector3(0, -3, 0);
        boxCollider.transform.eulerAngles = new Vector3(0, 0, 0);
        boxCollider.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetScale(BuildingObject selectedObject)
    {
        //boxCollider.transform.position = new Vector3(selectedObject.boxCollider..x, _lastPos.y, _lastPos.z);
        //boxCollider.transform.eulerAngles = new Vector3(selectedObject.x, _lastRot.y, _lastRot.z);
        boxCollider.transform.localScale = new Vector3(selectedObject.boxCollider.transform.localScale.x, selectedObject.boxCollider.transform.localScale.y, selectedObject.boxCollider.transform.localScale.z);
    }
}
