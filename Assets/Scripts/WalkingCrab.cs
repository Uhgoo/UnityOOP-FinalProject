using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// INHERITENCE
public class WalkingCrab : Crabs
{
    private float speed = 3f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
}
