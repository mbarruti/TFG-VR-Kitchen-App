using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] WallManager wallManager;

    //[SerializeField] List<Pole> availablePoles;

    // ------------------------------------

    //public List<Pole> availablePoles;

    public List<GameObject> availablePoles;

    public List<Pole> adjacentPoles;

    public float activeAxis;

    public BoxCollider boxCollider;

    // TO-DO: lo que esta comentado de antes ignorar, solo se va hacer la lista availablePoles para el Pole que actua como endPole en la direccion que se haya elegido dependiendo de la cara de startPole
    // a lo mejor una funcion que se llama desde SetStartPole para colocar los PreviewPole correspondientes y otra para dibujar la linea en cada frame a dichos PreviewPole o al Pole correspondiente:
    /// <summary>
    /// Filter the available poles in the world to connect to for this pole
    /// <summary>
    //public List<Pole> FilterAvailablePoles(Vector3 direction, Pole startPole)
    public void FilterAvailablePoles(Vector3 direction, Pole startPole)
    {
        RaycastHit[] hitPoles;
        hitPoles = Physics.RaycastAll(wallManager.wallHit.collider.transform.position, wallManager.wallHit.normal);

        float axisPos1;
        float axisPos2;

        float directionAxis;

        directionAxis = Vector3.Dot(direction, Vector3.one);
        //Debug.Log(direction);
        //Debug.Log(directionAxis);

        Vector3 directionAbs = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

        axisPos1 = Vector3.Dot(startPole.transform.position, directionAbs);
        //Debug.Log(axisPos1);

        // Filter the available poles in the world to connect to for this pole
        foreach (Pole pole in wallManager.poleList)
        {
            if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 4)
            {
                axisPos2 = Vector3.Dot(pole.transform.position, directionAbs);

                if (directionAxis > 0 && axisPos1 < axisPos2)
                {
                    //Debug.Log(axisPos2);
                    if (IsSecondAxisAligned(direction, directionAxis, startPole, pole) == true)
                    {
                        //pole.gameObject.layer = LayerMask.NameToLayer("Default");
                        availablePoles.Add(pole.gameObject);
                    }
                    else wallManager.SetPreviewPole(startPole, pole, hitPoles);
                }
                else if (directionAxis < 0 && axisPos1 > axisPos2)
                {
                    //Debug.Log(axisPos2);
                    if (IsSecondAxisAligned(direction, directionAxis, startPole, pole) == true)
                    {
                        //pole.gameObject.layer = LayerMask.NameToLayer("Default");
                        availablePoles.Add(pole.gameObject);
                    }
                    else wallManager.SetPreviewPole(startPole, pole, hitPoles);
                }
            }
        }

        //return availablePoles;
    }

    public void SetActiveAxisSign(BuildingWall wall, Vector3 normal)
    {
        if (wall.axisX == true)
            activeAxis = normal.x;
        else
            activeAxis = normal.z;
    }

    private bool IsSecondAxisAligned(Vector3 direction, float directionAxis, Pole startPole, Pole pole)
    {
        if (direction.x == directionAxis && startPole.transform.position.z == pole.transform.position.z)
            return true;
        if (direction.z == directionAxis && startPole.transform.position.x == pole.transform.position.x)
            return true;
        return false;
    }

    // A lo mejor esta va en WallManager
    /// <summary>
    /// Set the PreviewPoles that align with other poles
    /// <summary>
    //public void SetPreviewPoles()
    //{

    //}
}
