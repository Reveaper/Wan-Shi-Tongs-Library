using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public List<Transform> WayPoints;
    public Transform LookTarget;

    private int _currentWayPoint = 0;

    [SerializeField]
    private float _speed = 2f;

    public Camera PlayerCamera;

    public Transform Target;

    void Update()
    {
        Vector3 delta = WayPoints[_currentWayPoint].position - this.transform.position;

        if(delta.magnitude <= 0.2f)
        {
            _currentWayPoint = (_currentWayPoint + 1);

            delta = delta.normalized * _speed * Time.deltaTime;

            this.transform.position += delta;

            if (_currentWayPoint >= WayPoints.Count-1)
            {
                PlayerCamera.gameObject.SetActive(true);
                Destroy(Target.gameObject);
                Destroy(this.gameObject);
            }
        }
        else
        {
            delta = delta.normalized * _speed * Time.deltaTime;
            this.transform.position += delta;
        }

        this.transform.LookAt(LookTarget);
    }
}
