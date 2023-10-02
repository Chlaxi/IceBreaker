using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody rg;
    private InputReader InputReader;
    //private CharacterController characterController;

    [SerializeField]
    private float baseSpeed = 3f;
    [SerializeField]
    private float rotationAngle = 45f;
    [SerializeField]
    private float rotationSmooth = 45f;
    [SerializeField]
    private float maxForce = 5f;


    // Start is called before the first frame update
    void Start()
    {
        // characterController = GetComponent<CharacterController>();
        rg = GetComponent<Rigidbody>();
        InputReader = GetComponent<InputReader>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Move();
        if (rg.position.y < -4)
        {
            GameController.instance.StopGame();
        }
    }

    void Move()
    {
        Vector2 inputDir = InputReader.MovementDir;
        inputDir.Normalize();
        FaceMoveDir(inputDir);
        float speed = baseSpeed; // Add modifiers
        Vector3 direction = transform.forward * inputDir.y;
        Vector3 currentVelocity = rg.velocity;

        Vector3 deltaVelocity = direction * speed - currentVelocity;
        deltaVelocity.y = 0;

        Vector3.ClampMagnitude(deltaVelocity, maxForce);
        rg.AddForce(deltaVelocity, ForceMode.Acceleration);
        
    }

    private void FaceMoveDir(Vector3 dir)
    {
        Quaternion direction = transform.rotation * Quaternion.Euler(0, dir.x * rotationAngle, 0);
        transform.rotation = Quaternion.Lerp(
        transform.rotation,
        direction,
        rotationSmooth * Time.deltaTime);
    }
}
