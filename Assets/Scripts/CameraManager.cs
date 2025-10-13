using System;
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
    }

    public override void PanTo(float zoomSize) => zoom = zoomSize;

    public override void SetMaxZoom(float max) => maxZoom = max;

    public void SetCameraFOV(float newFOV) => targetFOV = newFOV;
}
