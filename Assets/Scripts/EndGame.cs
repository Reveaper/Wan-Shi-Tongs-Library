using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject EndGameCanvas;

    public Image Darken;


    private LayerMask _playerMask;
    private bool _hasTriggered;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if(_hasTriggered)
        {
            if (Darken.color.a < 1)
                Darken.color += new Color(0, 0, 0, 0.01f);

            if (Darken.color.a >= 1)
            {
                //game officially ends here
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            _hasTriggered = true;
            EndGameCanvas.SetActive(true);
        }
    }

}
