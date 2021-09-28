using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAirplane : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody[] wingParts;

    private void OnCollisionEnter(Collision collision)
    {
        foreach(Rigidbody rb in wingParts)
        {
            rb.isKinematic = false;
        }
    }
}
