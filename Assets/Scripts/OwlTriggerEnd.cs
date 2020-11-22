using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlTriggerEnd : MonoBehaviour
{
    public GameObject Owl;
    public ParticleSystem OwlParticles;

    private LayerMask _playerMask;

    public AudioSource Sound;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            if(Owl.activeSelf)
            {
                OwlParticles.Play();
                Owl.transform.rotation = this.transform.rotation;
                Owl.gameObject.SetActive(false);
                Sound.Play();
            }
        }
    }
}
