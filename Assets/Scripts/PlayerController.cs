using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public List<WheelCollider> wheels;
    // Air
    public float thrust;
    private float lift = 7000f;
    private float drag = 100f;
    private float torque = 8000f;
    private float turnSpeed = 0.2f;
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
        isFlying = true;
    }

    private void FixedUpdate()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0.01f;
        }
        if (!isFlying)
        {            
            isFlying = MovingOnGround();
        }
        else
        {
            isFlying = Flying();
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

    bool Flying()
    {
        //float angleOfAttack = Vector3.Angle(transform.forward, rb.velocity) * Mathf.Deg2Rad;
        rb.AddForce(-Physics.gravity * rb.mass);

        if (pitch != 0)
        {
            transform.rotation *= Quaternion.AngleAxis(pitch * turnSpeed, Vector3.right);
        }

        transform.rotation *= Quaternion.AngleAxis(turnSpeed * 10 * transform.rotation.z, transform.InverseTransformVector(Vector3.down));
        if (roll != 0)
        {
            transform.rotation *= Quaternion.AngleAxis(roll * turnSpeed, Vector3.back);
        }

        if (yaw != 0)
        {
            transform.rotation *= Quaternion.AngleAxis(yaw * turnSpeed, Vector3.up);
        }

        rb.AddRelativeForce(Vector3.forward * thrust);

        return true;
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

        if (rb.velocity.magnitude > 20f)
        {
            rb.drag = 0;
            rb.angularDrag = 0.05f;
            rb.AddRelativeForce(Vector3.up * lift * 10);
            return true;
        }
        return false;
    }
}
