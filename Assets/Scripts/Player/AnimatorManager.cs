using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void PlayerMovement(float speed){
        animator.SetFloat("Speed", speed);
    }
}
