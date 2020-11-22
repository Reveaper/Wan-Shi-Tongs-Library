using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Paper : MonoBehaviour
{
    public GameObject UICanvas;

    public List<GameObject> Hints;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            UICanvas.SetActive(!UICanvas.activeSelf);
    }

    public void ClearHints()
    {
        foreach(var hint in Hints)
            hint.SetActive(false);
    }
}
