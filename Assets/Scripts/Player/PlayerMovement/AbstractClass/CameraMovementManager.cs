using System;
using UnityEngine;

public abstract class CameraMovementManager : MonoBehaviour
{
    [NonSerialized] public float zoom;
    [NonSerialized] public float maxZoom = Config.CAMERA_MAX_ZOOM;

    public abstract void SetFollowEntity(GameObject entity, float? newZoom);
    public abstract void UpdateCamera();
    public abstract void PanTo(float zoomSize);

    public abstract void SetMaxZoom(float maxZoom);
}
