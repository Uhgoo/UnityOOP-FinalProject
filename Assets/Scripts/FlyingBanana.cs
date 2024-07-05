using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBanana : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 center = new Vector3(0, 4, 0);
    private float force = 18f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 power = (center - transform.position).normalized;
        rb.AddForce(power * force, ForceMode.Impulse);
        rb.AddTorque(power * force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -15f)
        {
            Destroy(gameObject);
        }
    }
}
