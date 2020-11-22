using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float Speed = 3f;

    private void Update()
    {
        this.transform.Rotate(Vector3.up, Speed * Time.deltaTime, Space.World);
    }
}
