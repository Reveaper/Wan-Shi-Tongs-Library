using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private LayerMask _playerMask;

    public GameObject BooksFloorEnable;
    public List<GameObject> BooksFloorDisable;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _playerMask)
        {
            Debug.Log("Entered");

            foreach (GameObject go in BooksFloorDisable)
                go.SetActive(false);

            BooksFloorEnable.SetActive(true);
        }
    }
}
