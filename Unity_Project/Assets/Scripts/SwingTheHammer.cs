using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTheHammer : MonoBehaviour
{
    public float fireRate;
    
    private Animator anim;
    private bool idleState = true;
    private float nextFire, timeBeforeAnimation, timeAfterAnimation;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextFire && idleState == true)
            {
                timeBeforeAnimation = Time.time;
                nextFire = Time.time + fireRate;                
                anim.SetTrigger("HammerSwing");
                idleState = false;
            }           
        }
        timeAfterAnimation = Time.time;

        if ((timeAfterAnimation - timeBeforeAnimation) > 1.25f && idleState == false)
        {
            anim.SetTrigger("ToIdle");
            idleState = true;
        }
    }

    public Animator getAnimator()
    {
        return anim;
    }
}
