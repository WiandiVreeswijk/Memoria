using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningVillager : MonoBehaviour
{
    private Animator animator;
    private int randomNumber;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RandomPoint()
    {
        randomNumber = Random.Range(1, 4);
        animator.SetInteger("isPointing", randomNumber);
    }
}
