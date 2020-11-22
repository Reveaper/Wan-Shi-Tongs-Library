using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SingleButtonDestroy : MonoBehaviour
{
    public KeyCode Key;

    private void Update()
    {
        if (Input.GetKeyDown(Key))
            Destroy(this.gameObject);
    }
}
