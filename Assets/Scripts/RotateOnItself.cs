using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnItself : MonoBehaviour
{
    private float speed = 100f;

    void Update()
    {
        transform.Rotate(Time.deltaTime * speed, Time.deltaTime * speed, Time.deltaTime * speed);
    }
}
