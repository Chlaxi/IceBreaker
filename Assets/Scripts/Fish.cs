using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody rg;
    Vector3 target;
    [SerializeField, Range(1, 5)]
    private float jumpForce;
    private float _jumpTime = 5;
    private void Start()
    {
        rg = GetComponent<Rigidbody>(); 
    }

    public void SetTarget(Vector3 target)
    {
        target.y = 14f;
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (transform.position.y > 15f)
        {
            MoveTowardsTarget();
        }

        _jumpTime -= Time.deltaTime;
        if (_jumpTime > 0)
            return;

        Jump();
        _jumpTime = Random.Range(.5f, 1.5f);
        if (rg.position.y < -5)
            Deactivate();
    }

    private void MoveTowardsTarget()
    {
        rg.velocity = Vector3.zero;
        float mass = Random.Range(1f, 3f);
        rg.AddForce(Vector3.down * mass);
        rg.useGravity = true;
        transform.position = target;
    }

    private void Jump()
    {
        Vector3 jumpDir = new Vector3(Random.Range(-1f,1f), 1, Random.Range(-1f,1f));
        jumpDir += jumpDir + Vector3.up;
        jumpDir.Normalize();
        float force = Random.Range(1, jumpForce);
        rg.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
    }

    public void Deactivate(){
        transform.position = new Vector3(-10, -10, -10);
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
        GameController.instance.AddFishToPool(gameObject);
    }
}
