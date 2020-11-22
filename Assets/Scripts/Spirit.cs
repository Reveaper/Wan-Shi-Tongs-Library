using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    private LayerMask _playerMask;

    public GameObject Sound;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            if(other.TryGetComponent(out PlayerStats playerStats))
            {
                if(playerStats.SpiritsCaptured < 5)
                {
                    Instantiate<GameObject>(Sound, this.transform.position, Quaternion.identity);
                    playerStats.AddSpirit(1);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
