using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody rg;
    Vector3 target;

    private void Start()
    {
        rg = GetComponent<Rigidbody>(); 
    }

    public void SetTarget(Vector3 target)
    {
        target.y = 14f;
        Debug.Log("Setting target to " + target);
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (transform.position.y > 15f)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        rg.velocity = Vector3.zero;
        rg.useGravity = true;
        transform.position = target;
    }

}
