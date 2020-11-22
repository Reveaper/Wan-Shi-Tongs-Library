using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLight : MonoBehaviour
{

    private LayerMask _playerMask;

    private bool _isTriggered;


    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if(_isTriggered)
            this.transform.position += Vector3.up * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            _isTriggered = true;
        }
    }
}
