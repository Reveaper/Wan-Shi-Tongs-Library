using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invector;

public class vThirdPersonCamera : MonoBehaviour
{
    private static vThirdPersonCamera _instance;
    public static vThirdPersonCamera instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<vThirdPersonCamera>();

                //Tell unity not to destroy this object when loading a new scene!
                //DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private float _defaultRightOffset;
    private float _defaultHeight;
    private Vector2 _cameraShakeDistance = new Vector2(-0.04f, 0.04f);
    private Vector2 _currentCameraShake;
    private bool _isShaking;
    private float _shakeTimer;
    private float _shakeTimerMax = 2;
    private float _shakeModifier;

    #region inspector properties    

    public Transform target;
    [Tooltip("Lerp speed between Camera States")]
    public float smoothCameraRotation = 12f;    
    [Tooltip("What layer will be culled")]
    public LayerMask cullingLayer = 1 << 0;                
    [Tooltip("Debug purposes, lock the camera behind the character for better align the states")]
    public bool lockCamera;
    
    public float rightOffset = 0f;
    public float defaultDistance = 2.5f;
    public float height = 1.4f;
    public float smoothFollow = 10f;
    public float xMouseSensitivity = 3f;
    public float yMouseSensitivity = 3f;
    public float yMinLimit = -40f;
    public float yMaxLimit = 80f; 
    #endregion

    #region hide properties    

    [HideInInspector]
    public int indexList, indexLookPoint;
    [HideInInspector]
    public float offSetPlayerPivot;
    [HideInInspector]
    public string currentStateName;
    [HideInInspector]
    public Transform currentTarget;
    [HideInInspector]
    public Vector2 movementSpeed;
   
    private Transform targetLookAt;
    private Vector3 currentTargetPos;
    private Vector3 lookPoint;
    private Vector3 current_cPos;
    private Vector3 desired_cPos;
    private Camera _camera;
    private float distance = 5f;
    private float mouseY = 0f;
    private float mouseX = 0f;
    private float currentHeight;
    private float cullingDistance;
    private float checkHeightRadius = 0.4f;
    private float clipPlaneMargin = 0f;
    private float forward = -1f;
    private float xMinLimit = -360f;
    private float xMaxLimit = 360f;
    private float cullingHeight = 0.2f;
    private float cullingMinDist = 0.1f;

    #endregion

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (target == null)
            return;

        _camera = GetComponent<Camera>();
        currentTarget = target;
        currentTargetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);

        targetLookAt = new GameObject("targetLookAt").transform;
        targetLookAt.position = currentTarget.position;
        targetLookAt.hideFlags = HideFlags.HideInHierarchy;
        targetLookAt.rotation = currentTarget.rotation;     

        mouseY = currentTarget.eulerAngles.x;
        mouseX = currentTarget.eulerAngles.y;

        distance = defaultDistance;
        currentHeight = height;

        _defaultRightOffset = rightOffset;
        _defaultHeight = height;
    }

    public void BeginShake(float duration)
    {
        _isShaking = true;
        _shakeTimer = 0;
        _shakeTimerMax = duration;
        _shakeModifier = 1 - (_shakeTimer / _shakeTimerMax);
    }

    private void Update()
    {
        if (_currentCameraShake.x < 0.001f)
            _currentCameraShake.x = 0;
        if (_currentCameraShake.y < 0.001f)
            _currentCameraShake.y = 0;

        if (_isShaking)
        {
            _shakeTimer += Time.deltaTime;
            _shakeModifier = 1 - (_shakeTimer / _shakeTimerMax);

            do
            {
                _currentCameraShake.x = UnityEngine.Random.Range(_cameraShakeDistance.x, _cameraShakeDistance.y);
            }
            while (_currentCameraShake.x >= -0.001f && _currentCameraShake.x <= 0.001f);

            do
            {
                _currentCameraShake.y = UnityEngine.Random.Range(_cameraShakeDistance.x, _cameraShakeDistance.y);
            }
            while (_currentCameraShake.y >= -0.001f && _currentCameraShake.y <= 0.001f);

            _currentCameraShake *= _shakeModifier;
        }
        else
        {
            _currentCameraShake = Vector2.zero;
        }

        if(_shakeTimer >= _shakeTimerMax)
        {
            _isShaking = false;
            _shakeTimer = 0;
            _shakeModifier = 0;
            _currentCameraShake = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (target == null || targetLookAt == null) return;

        CameraMovement();
    }


    /// <summary>
    /// Set the target for the camera
    /// </summary>
    /// <param name="New cursorObject"></param>
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget ? newTarget : target;
    }

    public void SetMainTarget(Transform newTarget)
    {
        target = newTarget;
        currentTarget = newTarget;
        mouseY = currentTarget.rotation.eulerAngles.x;
        mouseX = currentTarget.rotation.eulerAngles.y;
        Init();
    }

    /// <summary>    
    /// Convert a point in the screen in a Ray for the world
    /// </summary>
    /// <param name="Point"></param>
    /// <returns></returns>
    public Ray ScreenPointToRay(Vector3 Point)
    {
        return this.GetComponent<Camera>().ScreenPointToRay(Point);
    }

    /// <summary>
    /// Camera Rotation behaviour
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RotateCamera(float x, float y)
    {
        // free rotation 
        mouseX += x * xMouseSensitivity;
        mouseY -= y * yMouseSensitivity;

        movementSpeed.x = x;
        movementSpeed.y = -y;
        if (!lockCamera)
        {
            mouseY = vExtensions.ClampAngle(mouseY, yMinLimit, yMaxLimit);
            mouseX = vExtensions.ClampAngle(mouseX, xMinLimit, xMaxLimit);
        }
        else
        {
            mouseY = currentTarget.root.localEulerAngles.x;
            mouseX = currentTarget.root.localEulerAngles.y;
        }
    }

    /// <summary>
    /// Camera behaviour
    /// </summary>    
    void CameraMovement()
    {
        if (currentTarget == null)
            return;

        distance = Mathf.Lerp(distance, defaultDistance, smoothFollow * Time.deltaTime);
        //_camera.fieldOfView = fov;
        cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
        var camDir = (forward * targetLookAt.forward) + (rightOffset * targetLookAt.right);

        camDir = camDir.normalized;

        var targetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);

        rightOffset = _defaultRightOffset + _currentCameraShake.x;
        height = _defaultHeight + _currentCameraShake.y;


        currentTargetPos = targetPos;
        desired_cPos = targetPos + new Vector3(0, height, 0);
        current_cPos = currentTargetPos + new Vector3(0, currentHeight, 0);
        RaycastHit hitInfo;

        ClipPlanePoints planePoints = _camera.NearClipPlanePoints(current_cPos + (camDir * (distance)), clipPlaneMargin);
        ClipPlanePoints oldPoints = _camera.NearClipPlanePoints(desired_cPos + (camDir * distance), clipPlaneMargin);

        //Check if Height is not blocked 
        if (Physics.SphereCast(targetPos, checkHeightRadius, Vector3.up, out hitInfo, cullingHeight + 0.2f, cullingLayer))
        {
            var t = hitInfo.distance - 0.2f;
            t -= height;
            t /= (cullingHeight - height);
            cullingHeight = Mathf.Lerp(height, cullingHeight, Mathf.Clamp(t, 0.0f, 1.0f));
        }

        //Check if desired target position is not blocked       
        if (CullingRayCast(desired_cPos, oldPoints, out hitInfo, distance + 0.2f, cullingLayer, Color.blue))
        {
            distance = hitInfo.distance - 0.2f;
            if (distance < defaultDistance)
            {
                var t = hitInfo.distance;
                t -= cullingMinDist;
                t /= cullingMinDist;
                currentHeight = Mathf.Lerp(cullingHeight, height, Mathf.Clamp(t, 0.0f, 1.0f));
                current_cPos = currentTargetPos + new Vector3(0, currentHeight, 0);
            }
        }
        else
        {
            currentHeight = height;
        }
        //Check if target position with culling height applied is not blocked
        if (CullingRayCast(current_cPos, planePoints, out hitInfo, distance, cullingLayer, Color.cyan)) distance = Mathf.Clamp(cullingDistance, 0.0f, defaultDistance);
        var lookPoint = current_cPos + targetLookAt.forward * 2f;
        lookPoint += (targetLookAt.right * Vector3.Dot(camDir * (distance), targetLookAt.right));
        targetLookAt.position = current_cPos;

        Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
        targetLookAt.rotation = Quaternion.Slerp(targetLookAt.rotation, newRot, smoothCameraRotation * Time.deltaTime);
        transform.position = current_cPos + (camDir * (distance));
        var rotation = Quaternion.LookRotation((lookPoint) - transform.position);

        //lookTargetOffSet = Vector3.Lerp(lookTargetOffSet, Vector3.zero, 1 * Time.fixedDeltaTime);

        //rotation.eulerAngles += rotationOffSet + lookTargetOffSet;
        transform.rotation = rotation;
        movementSpeed = Vector2.zero;
    }


    /// <summary>
    /// Custom Raycast using NearClipPlanesPoints
    /// </summary>
    /// <param name="_to"></param>
    /// <param name="from"></param>
    /// <param name="hitInfo"></param>
    /// <param name="distance"></param>
    /// <param name="cullingLayer"></param>
    /// <returns></returns>
    bool CullingRayCast(Vector3 from, ClipPlanePoints _to, out RaycastHit hitInfo, float distance, LayerMask cullingLayer, Color color)
    {
        bool value = false;

        if (Physics.Raycast(from, _to.LowerLeft - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.LowerRight - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.UpperLeft - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.UpperRight - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        return value;
    }
}
