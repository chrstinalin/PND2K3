using System;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    [NonSerialized] public static PlayerMarker Instance;
    [NonSerialized] public GameObject Target;

    public event Action<GameObject> OnTargetChanged;

    private Joystick _Input = Constant.JOY_LEFT;
    private bool isActive = false;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetChildrenActive(false);
    }

    void Update()
    {
        if (!isActive)
        {
            gameObject.transform.position = PlayerMouse.Instance.getActivePlayer().transform.position;
        }
        else
        {
            float h = Input.GetAxis(_Input.Horizontal);
            float v = Input.GetAxis(_Input.Vertical);

            Vector3 inputXZ = new Vector3(h, 0f, v);
            Vector3 intended = transform.position + inputXZ.normalized * Config.PLAYER_MARKER_MOVE_SPEED * Time.deltaTime;

            if (TryGetHighestGroundY(intended, out float groundY))
            {
                intended.y = groundY + Config.PLAYER_MARKER_GROUND_SNAP_OFFSET;
                transform.position = intended;
            }
            else
            {
                Vector3 fallback = transform.position;
                fallback.x = intended.x;
                fallback.z = intended.z;
                transform.position = fallback;
            }
        }
    }

    public void setActive(bool val)
    {
        CameraManager.Instance.SetFollowEntity(gameObject, null);
        SetChildrenActive(val);
        isActive = val;
    }

    private void SetChildrenActive(bool val)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                child.gameObject.SetActive(val);
            }
        }
    }

    private bool TryGetHighestGroundY(Vector3 positionXZ, out float highestY)
    {
        highestY = float.MinValue;
        float originY = positionXZ.y + Config.PLAYER_MARKER_GROUND_RAY_HEIGHT;
        Vector3 origin = new Vector3(positionXZ.x, originY, positionXZ.z);
        float rayDistance = Config.PLAYER_MARKER_GROUND_RAY_HEIGHT * 2f;

        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, rayDistance, Config.PLAYER_MARKER_GROUND_LAYERS.value);

        if (hits == null || hits.Length == 0) return false;

        foreach (var hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                if (hit.collider.CompareTag("Environment"))
                {
                    if (hit.point.y > highestY) highestY = hit.point.y;
                }
            }
        }

        return highestY != float.MinValue;
    }

    void OnTriggerEnter(Collider other)
    {
        var selectable = other.GetComponent<LockOnSelectable>();
        if (selectable != null) selectable.OnHover(true);
    }

    void OnTriggerExit(Collider other)
    {
        var selectable = other.GetComponent<LockOnSelectable>();
        if (selectable != null) selectable.OnHover(false);
    }

    public void SetTarget(GameObject target)
    {
        if (Target == target) return;
        Target = target;
        OnTargetChanged?.Invoke(Target);
    }

}
