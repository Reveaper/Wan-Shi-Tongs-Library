using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : MonoBehaviour
{
    public List<GameObject> TurnStuffOn;
    public List<GameObject> TurnStuffOff;

    public Image Darken;

    private bool hasPressed;

    private void Update()
    {
        if(hasPressed)
        {
            Darken.color += new Color(0, 0, 0, 0.05f);

            if (Darken.color.a >= 1f)
            {
                foreach (var obj in TurnStuffOff)
                    obj.SetActive(false);

                foreach (var obj in TurnStuffOn)
                    obj.SetActive(true);

                this.gameObject.SetActive(false);


            }
        }
    }

    public void StartGame()
    {
        Darken.color = new Color(0, 0, 0, 0);
        Darken.gameObject.SetActive(true);
        hasPressed = true;
    }
}
