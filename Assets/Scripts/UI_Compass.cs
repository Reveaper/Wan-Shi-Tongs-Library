using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Compass : MonoBehaviour
{
    public Camera Camera;

    private void Update()
    {
        Vector3 viewRotationProjected = Vector3.ProjectOnPlane(Camera.transform.localEulerAngles, Vector3.up);
        this.transform.localEulerAngles = new Vector3(0, 0, Camera.transform.localEulerAngles.y);
    }

}
