using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushAirplane : MonoBehaviour
{
    public Rigidbody rb;
    public GameControllerScript gameController;

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        print("i");
        if (rb.velocity.magnitude > 20)
            gameController.isCrushed = true;
    }
}
