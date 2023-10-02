using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class IcebergControls : MonoBehaviour
{
    public float stabilisation, stabilisationfactor, damping;
    private struct icebergValues
    {
        public icebergValues(Transform t, float mass)
        {
            this.t = t;
            this.mass = mass;
        }
        public Transform t;
        public float mass;
    }

    icebergValues startValues;
    public bool isSmelting = false;
    Rigidbody rg;
    Vector3 rotationClamp = Vector3.zero;


    [SerializeField, Range(0f, 1f)]
    private float shrinkSpeed = 0.5f;
    public Vector3 shrinkgoal;
    [SerializeField]
    private float shrinkTime = 5; 
    private float shrinkElapsedTime = 0; 

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.AddTorque(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
       // rg.constraints = RigidbodyConstraints.FreezePosition;
        startValues = new icebergValues(transform, rg.mass);
        shrinkgoal = transform.localScale;

        
    }

    private void FixedUpdate()
    {
        float targetMass = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        rg.mass = targetMass;
        stabilisation = rg.mass * stabilisationfactor;
        Balance();
        if (!isSmelting)
            return;
        Shrink();
    }

    public void ResetValues()
    {
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;
        transform.position = startValues.t.position;
        transform.rotation = startValues.t.rotation;
        transform.localScale = startValues.t.localScale;
        rg.mass = startValues.mass;
    }

    private void Shrink()
    {
        Debug.Log("Smelting!");
        if(rg.mass>1)
            rg.mass -= 0.01f;
        if (transform.localScale.x < 1.5)
            return;
        float interpolationRatio = (shrinkElapsedTime / shrinkTime) * Time.deltaTime * shrinkSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale,
                                            shrinkgoal,
                                            interpolationRatio);
        shrinkElapsedTime += Time.deltaTime;
        if (transform.localScale.x <= shrinkgoal.x)
        {
            isSmelting = false;
        }
    }

    public void StartShrink()
    {
        shrinkElapsedTime = 0;
        shrinkgoal -= new Vector3(.25f, 0, .25f);
        isSmelting = true;
    }

    private void Balance()
    {
        Vector3 torque = stabilisation * Vector3.Cross(transform.up, Vector3.up) - damping * rg.angularVelocity;
        rg.AddTorque(torque);
    }
}
