using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimationToggler : MonoBehaviour
{

    //[SerializeField] bool isSelected;

    // ------------------------------------------------------

    public Animator animator;


    //private void OnEnable()
    //{
    //    if (isSelected == true) ActivateIsSelected();
    //    else DeactivateIsSelected();
    //}


    public void ActivateIsSelected()
    {
        //isSelected = true;
        animator.SetBool("isSelected", true);
    }

    public void DeactivateIsSelected()
    {
        //isSelected = false;
        animator.SetBool("isSelected", false);
    }
}
