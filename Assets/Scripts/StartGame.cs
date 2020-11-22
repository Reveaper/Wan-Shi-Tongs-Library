using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Image Darken;

    private void Start()
    {
        Darken.color = new Color(0, 0, 0, 1);
    }

    private void Update()
    {
        Darken.color -= new Color(0, 0, 0, 0.05f);

        if(Darken.color.a <= 0)
            this.enabled = false;
    }
}
