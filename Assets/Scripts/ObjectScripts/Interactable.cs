using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float limit;

    Vector3 initialPosition;
    Vector3 initialRotation;

    // ------------------------------------------------------

    // True it rotates, false it moves
    public bool rotationInteractable;

    // 0 for X axis, 1 for Y axis, 2 for Z axis
    public float axis;

    // 1 for positive value, -1 for negative value in the specified axis
    public float sign;

    // Saved initial values of the controller that is interacting with the object
    public Vector3 interactingControllerPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }

    public void RotateInteraction(GameObject controller)
    {
        switch (axis)
        {
            // X axis
            case 0:

                // Check the sign
                if (sign > 0)
                {
                    // Get the value for the interaction
                    float newValue = interactingControllerPosition.y - controller.transform.localPosition.y;

                    transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x + newValue * offset, 0f, limit), transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
                else if (sign < 0)
                {
                    // Get the value for the interaction
                    float newValue = interactingControllerPosition.y - controller.transform.localPosition.y;

                    // Make the operations in Quaternions so negative values can't start problems
                    Quaternion q = Quaternion.Euler(transform.localEulerAngles.x + newValue * offset, transform.localEulerAngles.y, transform.localEulerAngles.z);

                    q.x /= q.w;
                    q.y /= q.w;
                    q.z /= q.w;
                    q.w = 1.0f;

                    float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
                    angleX = Mathf.Clamp(angleX, -limit, 0f);
                    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

                    transform.localRotation = q;
                }

                break;

            // Y axis
            case 1:

                // Check the sign
                if (sign > 0)
                {
                    // Get the value for the interaction
                    float newValue = interactingControllerPosition.x - controller.transform.localPosition.x;

                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.Clamp(transform.localEulerAngles.y + newValue * offset, 0f, limit), transform.localEulerAngles.z);
                }
                else if (sign < 0)
                {
                    // Get the value for the interaction
                    float newValue = interactingControllerPosition.x - controller.transform.localPosition.x;

                    // Make the operations in Quaternions so negative values can't start problems
                    Quaternion q = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + newValue * offset, transform.localEulerAngles.z);

                    q.x /= q.w;
                    q.y /= q.w;
                    q.z /= q.w;
                    q.w = 1.0f;

                    float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
                    angleY = Mathf.Clamp(angleY, -limit, 0f);
                    q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

                    transform.localRotation = q;
                }

                break;

            // Z axis
            //case 2:

            //    if (sign > 0)
            //    {

            //    }

            //    break;
        }
    }

    public void MoveInteraction(GameObject controller)
    {
        switch (axis)
        {
            // X axis
            //case 0:

            //    if (sign > 0)
            //        Debug.Log("hola");

            //    break;

            // Y axis
            //case 1:

            //    if (sign > 0)
            //        Debug.Log("hola");

            //    break;

            // Z axis
            case 2:

                // Check the sign
                if (sign > 0)
                {
                    // Get the value for the interaction
                    float newValue = interactingControllerPosition.z - controller.transform.localPosition.z;

                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z + newValue * offset, 0f, limit));
                }
                //else if (sign < 0)
                //{
                //    // Get the value for the interaction
                //    float newValue = interactingControllerPosition.z - controller.transform.localPosition.z;

                //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z + newValue * offset, -limit, 0f));
                //}

                break;
        }
    }
}
