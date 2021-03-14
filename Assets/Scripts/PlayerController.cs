using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public List<WheelCollider> wheels;
    // Air
    public float thrust;
    // Ground
    public float acceleration;

    [Range(-1, 1)]
    public float pitch;
    [Range(-1, 1)]
    public float roll;
    [Range(0, 1)]
    public float brake;
    [Range(0, 1)]
    public float flap;
    [Range(1, 1)]
    public float yaw;

    public bool isFlying;

    // Start is called before the first frame update
    void Start()
    {
        isFlying = false;
    }

    private void FixedUpdate()
    {
        if (!isFlying)
        {
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0.01f;
            }
            isFlying = MovingOnGround();
        }
    }

    // Update is called once per frame
    void Update()
    {
        pitch = Input.GetAxis("Vertical");
        roll = Input.GetAxis("Horizontal");
        yaw = Input.GetAxis("Yaw");
        brake = Input.GetAxis("Jump");
    }

    bool MovingOnGround()
    {
        if (pitch != 0)
        {
            rb.AddRelativeForce(Vector3.forward * thrust * pitch * (Input.GetKey(KeyCode.U) ? acceleration : 1)); // U is acceleration
            rb.AddRelativeTorque(Vector3.up * thrust  * roll * (rb.velocity.magnitude > 2 && !Input.GetKey(KeyCode.U) ? 1 : 0));
        }

        if (brake != 0)
        {
            rb.drag = rb.velocity.magnitude < 0.7 ? 1 : 0.7f;
        }
        else rb.drag = 0.3f;

        if (rb.velocity.magnitude > 30f)
        {
            rb.drag = 0;
            rb.angularDrag = 0.05f;
            return true;
        }
        return false;
    }
}
