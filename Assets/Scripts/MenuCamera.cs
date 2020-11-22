using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCamera : MonoBehaviour
{
    public float Speed = 0.15f;

    int currentView = 0;

    public Image DarkenImg;

    public List<Transform> View1;
    public Transform View1Target;
    public List<GameObject> View1Objects;
    public List<GameObject> View1ObjectsOff;

    public List<Transform> View2;
    public Transform View2Target;
    public List<GameObject> View2Objects;
    public List<GameObject> View2ObjectsOff;

    public List<Transform> View3;
    public Transform View3Target;
    public List<GameObject> View3Objects;
    public List<GameObject> View3ObjectsOff;

    public List<Transform> View4;
    public Transform View4Target;
    public List<GameObject> View4Objects;
    public List<GameObject> View4ObjectsOff;

    public List<Transform> View5;
    public Transform View5Target;
    public List<GameObject> View5Objects;
    public List<GameObject> View5ObjectsOff;

    public List<Transform> View6;
    public Transform View6Target;
    public List<GameObject> View6Objects;
    public List<GameObject> View6ObjectsOff;

    private bool _isDarkening;
    private bool _isGoingOutOfDarkening;

    private void Start()
    {
        PickNewView(UnityEngine.Random.Range(0, 6));
    }

    private void Update()
    {
        if (currentView == 0)
            MoveCamera(View1[1].position, View1Target);
        if (currentView == 1)
            MoveCamera(View2[1].position, View2Target);
        if (currentView == 2)
            MoveCamera(View3[1].position, View3Target);
        if (currentView == 3)
            MoveCamera(View4[1].position, View4Target);
        if (currentView == 4)
            MoveCamera(View5[1].position, View5Target);
        if (currentView == 5)
            MoveCamera(View6[1].position, View6Target);

        if (_isGoingOutOfDarkening)
        {
            DarkenImg.color -= new Color(0, 0, 0, 0.05f);
            if (DarkenImg.color.a <= 0)
            {
                _isGoingOutOfDarkening = false;
                DarkenImg.gameObject.SetActive(false);
            }
        }
    }

    private void MoveCamera(Vector3 targetPosition, Transform target)
    {
        Vector3 delta = (targetPosition - this.transform.position).normalized;
        this.transform.position += delta * Speed * Time.deltaTime;
        this.transform.LookAt(target);

        float distanceToEnd = Vector3.Distance(this.transform.position, targetPosition);

        if (distanceToEnd < 0.5f && !_isDarkening)
        {
            _isDarkening = true;
            DarkenImg.gameObject.SetActive(true);
            DarkenImg.color = new Color(0, 0, 0, 0);
        }

        if (distanceToEnd < 0.5f)
            Darken();
    }

    private void Darken()
    {
        DarkenImg.color += new Color(0, 0, 0, 0.05f);

        if (DarkenImg.color.a >= 1)
        {
            PickNewView(UnityEngine.Random.Range(0, 6));
            _isDarkening = false;
            _isGoingOutOfDarkening = true;
        }
    }

    private void PickNewView(int view)
    {
        while(view == currentView)
        {
            view = UnityEngine.Random.Range(0, 6);
        }

        if (view == 0)
            HandleTransition(View1[0].position, View1Target, View1ObjectsOff, View1Objects);
        if (view == 1)
            HandleTransition(View2[0].position, View2Target, View2ObjectsOff, View2Objects);
        if (view == 2)
            HandleTransition(View3[0].position, View3Target, View3ObjectsOff, View3Objects);
        if (view == 3)
            HandleTransition(View4[0].position, View4Target, View4ObjectsOff, View4Objects);
        if (view == 4)
            HandleTransition(View5[0].position, View5Target, View5ObjectsOff, View5Objects);
        if (view == 5)
            HandleTransition(View6[0].position, View6Target, View6ObjectsOff, View6Objects);

        currentView = view;
    }

    private void HandleTransition(Vector3 spawnPos, Transform target, List<GameObject> turnObjectsOff, List<GameObject> turnObjectsOn)
    {
        this.transform.position = spawnPos;
        this.transform.LookAt(target);

        foreach (var obj in turnObjectsOff)
            obj.SetActive(false);

        foreach (var obj in turnObjectsOn)
            obj.SetActive(true);
    }
}
