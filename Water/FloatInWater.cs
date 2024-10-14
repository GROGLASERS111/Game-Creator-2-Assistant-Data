using UnityEngine;

public class FloatInWater : MonoBehaviour
{
    public float waterLevel = 0.0f; 
    public float buoyancyForce = 10f;
    public float linearDamping = 0.5f;
    public float angularDamping = 0.5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {
            Vector3 buoyancy = new Vector3(0, buoyancyForce, 0);
            rb.AddForce(buoyancy, ForceMode.Acceleration);

            Vector3 velocityDamping = -rb.velocity * linearDamping;
            rb.AddForce(velocityDamping, ForceMode.Acceleration);

            Vector3 angularVelocityDamping = -rb.angularVelocity * angularDamping;
            rb.AddTorque(angularVelocityDamping, ForceMode.Acceleration);
        }
    }
}
