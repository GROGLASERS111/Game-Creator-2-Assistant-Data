using UnityEngine;

public class RippleObject : MonoBehaviour
{
    public ParticleSystem ripple;
    private RaycastHit isGround;
    private Vector3 lastPosition;
    private bool inWater;
    public float minVelocityThreshold = 0.1f; 

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float velocity = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (inWater && velocity > minVelocityThreshold)
        {
            TriggerRippleEffect();
        }

        CheckWaterCollision();
    }

    void TriggerRippleEffect()
    {
        ripple.transform.position = transform.position;
        ripple.Emit(1);
    }

    void CheckWaterCollision()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        inWater = Physics.Raycast(transform.position + Vector3.up * height, Vector3.down, height * 2, LayerMask.GetMask("Water"));

        ripple.gameObject.SetActive(inWater);

        Physics.Raycast(transform.position, Vector3.down, out isGround, 2.7f, LayerMask.GetMask("Ground"));
    }
}
