using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{

    private float _speed = 0;

    private float _timer;

    private void Update()
    {
        _speed += 0.5f * Time.deltaTime;
        _speed = Mathf.Clamp(_speed, 0, 2f);

        this.transform.position += Vector3.down * _speed * Time.deltaTime;
        /*
        _timer += Time.deltaTime;
        if(_timer >= 8f)
        {
            Time.timeScale = Mathf.Max(Time.timeScale - 0.1f * Time.deltaTime, 0);
        }*/
    }
}
