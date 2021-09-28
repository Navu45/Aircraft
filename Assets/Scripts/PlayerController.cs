﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public List<WheelCollider> wheels;
    // Air
    public float thrust;
    public float lift = 0;
    private float drag = 10f;
    private float torque = 20000f;
    private float turnSpeed = 0.2f;
    // Ground
    public float acceleration;


    private float MAX_TURN_ANGLE = 0.3f;
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
        rb.centerOfMass = new Vector3(0, 1.7f, -1.2f);
    }

    private void FixedUpdate()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0.01f;
        }
        if (!isFlying)
        {
            rb.angularDrag = 0.8f;
            thrust = 30000f;
            isFlying = MovingOnGround();
        }
        else
        {
            rb.drag = 0.3f;
            thrust = 30000f;
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
        float gas = Input.GetKey(KeyCode.U) ? 2 : (Input.GetKey(KeyCode.J) && rb.velocity.magnitude >= 16 ? 0.8f : (
            rb.velocity.magnitude < 16 ? 0.5f : 1));
        rb.AddRelativeForce(Vector3.forward * thrust * gas);
        if (rb.velocity.magnitude >= 16)
        {
            if (lift != 0)
                lift += lift < 1 ? Time.deltaTime * 0.05f : 0;
            else lift = 1;
        }

        lift -= lift > 0 && gas == 0.8f ? Time.deltaTime * 0.3f : 0;
        
        rb.AddForce(-Physics.gravity * rb.mass * lift * (1 - Mathf.Abs(transform.rotation.x)));

        if (transform.position.y < 1 && lift < 1)
        {
            rb.drag = 0;
            return false;
        }

        if (pitch != 0)
        {
            rb.AddRelativeTorque(Vector3.right * torque * pitch);
        }

        if (Mathf.Abs(transform.rotation.z) < 0.4f)
            rb.AddRelativeTorque(Physics.gravity.normalized * torque * 3 * Mathf.Abs(transform.rotation.z));
        if (roll != 0)
        {
            rb.AddRelativeTorque(Vector3.back * torque * roll);
        }

        if (yaw != 0)
        {
            rb.AddRelativeTorque(Vector3.up * yaw * torque);
        }

        
        return true;
    }

    bool MovingOnGround()
    {
        if (rb.velocity.magnitude > 16f && lift == 0)
        {
            rb.AddForce(Vector3.up * thrust * 2);
            rb.AddRelativeTorque(Vector3.right * torque);
            return true;
        }

        int gas = Input.GetKey(KeyCode.U) ? 1 : 0;

        if (gas != 0)
        {
            rb.AddRelativeForce(Vector3.forward * thrust * gas);
            rb.AddRelativeTorque(Vector3.up * thrust * gas * roll * (rb.velocity.magnitude > 2 ? 1 : 0));
        }

        if (brake != 0)
        {
            rb.drag = rb.velocity.magnitude < 0.7 ? 1 : 0.3f;
        }
        else rb.drag = 0.05f;

        if (lift > 0)
            lift -= Time.deltaTime;

        return false;
    }
}
