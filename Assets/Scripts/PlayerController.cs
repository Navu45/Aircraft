using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public List<WheelCollider> wheels;
    public GameObject canvasEnd;
    public Text endText;
    public Text score;
    public GameObject continueButton;
    public bool isFuelOver;

    // Air
    public float thrust;
    public float lift = 0;
    private float torque = 20000f;
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
    void Start()
    {
        isFlying = false;
        rb.centerOfMass = new Vector3(0, 1.7f, -1.2f);
        rb.angularDrag = 0.8f;
        lift = 0;
        isFuelOver = false;
    }

    private void FixedUpdate()
    {
        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0.01f;
        }
        if (!isFlying)
        {
            thrust = 20000f;
            isFlying = MovingOnGround();
        }
        else
        {
            rb.drag = 0.3f;
            thrust += thrust <= 40000 ? 4000 * Time.deltaTime : 0;
            isFlying = Flying();
        }
    }
    void Update()
    {
        pitch = Input.GetAxis("Vertical");
        roll = Input.GetAxis("Horizontal");
        yaw = Input.GetAxis("Yaw");
        brake = Input.GetAxis("Jump");

        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            endText.text = "Do you want to exit?";
            score.text = "";
            continueButton.SetActive(true);
            canvasEnd.SetActive(true);
        }
    }

    bool Flying()
    {  
        float gas = Input.GetKey(KeyCode.U) ? 2 : (Input.GetKey(KeyCode.J) && rb.velocity.magnitude >= 25 ? 0.8f : (
            rb.velocity.magnitude < 25 ? 0.5f : 1));
        if (!isFuelOver)
            rb.AddRelativeForce(Vector3.forward * thrust * gas);
        else
            rb.AddRelativeForce(Vector3.forward * rb.velocity.magnitude * 1000);
        if (rb.velocity.magnitude >= 25)
        {
            if (lift != 0)
                lift += lift < 1 ? Time.deltaTime * 0.05f : 0;
            else lift = 1;
        }        

        if (1 - Mathf.Abs(transform.rotation.x) < 0.2f)
            rb.AddForce(-Physics.gravity * rb.mass * lift * (1 + transform.rotation.x));
        else if (0.5f - Mathf.Abs(transform.rotation.x) < 0.08f)
            rb.AddForce(-Physics.gravity * rb.mass * lift * (0.3f + transform.rotation.x));
        else
            rb.AddForce(-Physics.gravity * rb.mass * lift);

        if (lift < 1 && (wheels[0].isGrounded || wheels[1].isGrounded || wheels[2].isGrounded))
        {
            rb.drag = 0;
            return false;
        }

        lift -= lift > 0 && gas == 0.8f || isFuelOver ? Time.deltaTime * 0.3f : 0;

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
            rb.AddRelativeTorque(Vector3.up * yaw * torque * 2);
        }
        
        return true;
    }

    bool MovingOnGround()
    {
        if (rb.velocity.magnitude >= 25f && lift == 0)
        {
            rb.AddForce(Vector3.up * thrust * 2);
            rb.AddRelativeTorque(Vector3.right * torque);
            return true;
        }

        int gas = Input.GetKey(KeyCode.U) ? 1 : 0;

        if (gas != 0)
        {
            if (!isFuelOver)
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
        else if (lift < 0)
            lift = 0;

        return false;
    }
}
