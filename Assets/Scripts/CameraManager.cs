using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CameraManager : CameraMovementManager
{
    public static CameraManager Instance;

    public GameObject FollowEntity;
    [NonSerialized] public Transform CameraPivot;
    [NonSerialized] public Camera Cam;
    private float heightOffset;

    private float targetFOV;
    private float fovVelocity;

    void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        Cam = gameObject.GetComponent<Camera>();
        CameraPivot = gameObject.transform.parent.GetComponent<Transform>();
        heightOffset = CameraPivot.transform.position.y;
        targetFOV = Config.CAMERA_DEFAULT_FOV;
    }

    void Update()
    {
        Cam.fieldOfView = Mathf.SmoothDamp(Cam.fieldOfView, targetFOV, ref fovVelocity, Config.CAMERA_SMOOTH_TIME);
    }

    public override void SetFollowEntity(GameObject? entity, float? newZoom)
    {
        this.FollowEntity = entity;
        if(newZoom.HasValue)
        {
            SetMaxZoom(newZoom.Value);
            PanTo(newZoom.Value);
        }
    }

    public override void UpdateCamera()
    {
        // Update zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * Config.CAMERA_ZOOM_MULTIPLIER;
        zoom = Mathf.Clamp(zoom, Config.CAMERA_MIN_ZOOM, maxZoom);
        Cam.orthographicSize = Mathf.SmoothDamp(Cam.orthographicSize, zoom, ref Config.CAMERA_VELOCITY, Config.CAMERA_SMOOTH_TIME);

        // Update location
        CameraPivot.transform.position = new Vector3(FollowEntity.transform.position.x,
                                             FollowEntity.transform.position.y + heightOffset,
                                             FollowEntity.transform.position.z);

        // Rotate 90 degrees around the mouse
        // TODO: test with controller
        float rightStickX = Input.GetAxis("HorizontalRightJoystick"); // from -1 to 1
        if (rightStickX > 0.8f)
        {
            StartCoroutine(RotateAroundTarget(90f));
        }
        else if ( rightStickX < -0.8f)
        {
            StartCoroutine(RotateAroundTarget(-90f));
        }

        if (Input.GetKeyDown(KeyCode.O))
            StartCoroutine(RotateAroundTarget(-90f));
        else if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(RotateAroundTarget(90f));
    }

    private IEnumerator RotateAroundTarget(float angle)
    {   

        float rotatedDegrees = 0f;
        float direction = Mathf.Sign(angle);
        float totalDegrees= Mathf.Abs(angle);

        while (rotatedDegrees < totalDegrees)
        {
            float step = 180f * Time.deltaTime; // rotate 180 degrees per second
            transform.RotateAround(FollowEntity.transform.position, Vector3.up, step * direction);
            rotatedDegrees += step;
            yield return null;
        }

        // snap to exact 90 degree angle on the grid
        float finalY = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;
        Vector3 currentEuler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(currentEuler.x, finalY, currentEuler.z);
    }

    public override void PanTo(float zoomSize) => zoom = zoomSize;

    public override void SetMaxZoom(float max) => maxZoom = max;

    public void SetCameraFOV(float newFOV) => targetFOV = newFOV;
}
