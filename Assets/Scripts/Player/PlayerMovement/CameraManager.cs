using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CameraManager : CameraMovementManager
{
    GameObject FollowEntity;
    public GameObject CameraPivot;
    public Camera Cam;
    private float heightOffset;

    public void Start()
    {
        heightOffset = CameraPivot.transform.position.y;
    }

    public override void SetFollowEntity(GameObject entity, float? newZoom)
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
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Cam.orthographicSize = Mathf.SmoothDamp(Cam.orthographicSize, zoom, ref velocity, smoothTime);

        // Update location
        CameraPivot.transform.position = new Vector3(FollowEntity.transform.position.x,
                                             FollowEntity.transform.position.y + heightOffset,
                                             FollowEntity.transform.position.z);
    }

    public override void PanTo(float zoomSize) => zoom = zoomSize;

    public override void SetMaxZoom(float max) => maxZoom = max;
}
