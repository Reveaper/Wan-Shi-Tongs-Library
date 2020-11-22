using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHowToPlay : MonoBehaviour
{
    public GameObject Image;
    
    public void OnPressed()
    {
        Image.SetActive(!Image.activeSelf);
    }


}
