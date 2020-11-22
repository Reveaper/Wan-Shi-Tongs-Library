using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicIntroTarget : MonoBehaviour
{
    public Transform Target;

    public float Speed = 0.15f;

    private Vector3 _movement;

    private void Start()
    {
        _movement = (Target.position - this.transform.position).normalized;
    }

    private void Update()
    {
        if(Vector3.Distance(this.transform.position, Target.position) < 0.02f)
        {
            this.transform.position = Target.position;
        }
        else
        {
            this.transform.position += _movement * Speed;
        }
    }
}
