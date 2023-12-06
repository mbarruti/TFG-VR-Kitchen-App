using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] WallManager wallManager;

    [SerializeField] List<Pole> availablePoles;

    // ------------------------------------

    public List<Pole> adjacentPoles;

    //public bool isStartPole = false;
    //public bool isEndPole = false;

    public BoxCollider boxCollider;

    private void Awake()
    {
        //if (wallManager.poleList.Count != 0)
        //{
        //    // Filter the available poles in the world to connect to for this pole
        //    foreach (Pole pole in wallManager.poleList)
        //    {
        //        if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 2) availablePoles.Add(pole);
        //    }
        //}
    }

    //public void UpdateAvailablePoles()
    //{
    //    // TO-DO: descomentar cuando permita que se eliminen paredes, quiza no va en esta funcion y va en una que haga para actualizar despues de eliminar paredes
    //    //availablePoles.RemoveAll(pole => pole == null);
    //    //adjacentPoles.RemoveAll(pole => pole == null);

    //    foreach (Pole pole in wallManager.poleList)
    //    {
    //        if (!availablePoles.Contains(pole))
    //        {
    //            if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 2) availablePoles.Add(pole);
    //        }
    //        // TO-DO: si lo contiene, mirar si hay que quitarlo de availablePoles
    //        //else
    //        //{

    //        //}
    //    }
    //}

    // TO-DO: lo que esta comentado de antes ignorar, solo se va hacer la lista availablePoles para el Pole que actua como endPole en la direccion que se haya elegido dependiendo de la cara de startPole
    // a lo mejor una funcion que se llama desde SetStartPole para colocar los PreviewPole correspondientes y otra para dibujar la linea en cada frame a dichos PreviewPole o al Pole correspondiente:
    /// <summary>
    /// Filter the available poles in the world to connect to for this pole
    /// <summary>
    private void FilterAvailablePoles()
    {
        if (wallManager.poleList.Count != 0)
        {
            // Filter the available poles in the world to connect to for this pole
            foreach (Pole pole in wallManager.poleList)
            {
                if (!adjacentPoles.Contains(pole) && pole.adjacentPoles.Count < 4) availablePoles.Add(pole);
            }
        }
    }

    // A lo mejor esta va en WallManager
    /// <summary>
    /// Set the PreviewPoles that align with other poles
    /// <summary>
    public void SetPreviewPoles()
    {

    }
}
