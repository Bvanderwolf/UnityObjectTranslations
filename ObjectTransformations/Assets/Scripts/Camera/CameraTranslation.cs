using UnityEngine;

public class CameraTranslation : MonoBehaviour
{
    [Header("SceneReferences")]
    [SerializeField] private Transform observable;
    [SerializeField] private Transform[] startnodes;

    [Header("FollowAttributes")]
    [SerializeField] private float distanceFactor;
    [SerializeField] private float heightFactor;
    [SerializeField] private float followSmoothTime;

    [Header("DynamicFollowAttributes")]
    [SerializeField] private float minimalAngle;
    [SerializeField] private float stepFactor;

    [Header("ObserveAttributes")]
    [SerializeField] private float observeDistanceFactor;
    [SerializeField] private float observeSmoothTime;

    [SerializeField] private float noSwitchTime;
    [SerializeField] private bool followMode;   

    private uint switchCount = 0;
    private const uint MAX_SWITCH_COUNT = 5;
    private float cooldownTime = 0;

    private int currentStartnodeIndex = 0;
    private Vector3 currentObservePosition;

    private Vector3 currentVelocity;

    public bool NoSwitch { get; private set; } = false;
    public bool InRoomConnector { get; private set; } = true;

    private void Awake ()
    {
        if (observable != null)
        {
            ObjectNavigation objectNavigation = observable.GetComponent<ObjectNavigation>();
            objectNavigation.OnLeavingRoom += OnLeavingRoom;
            objectNavigation.OnStartingRoom += OnStartingRoom;

            for (int i = 0; i < startnodes.Length; i++)
            {
                if (startnodes[i].GetInstanceID() == objectNavigation.MoveTarget.GetInstanceID())
                {
                    currentStartnodeIndex = i;
                    currentObservePosition = GetObservePosition();
                    break;
                }
            }
        }
    }

    private void FixedUpdate ()
    {
        if (UIManagement.FadingScreen)
            return;

        Vector3 moveTarget;
        float smoothTime;

        if (followMode)
        {
            //moving the camera towards the back of the plane in a smooth manner
            Vector3 spacingVector = (-observable.forward.normalized * distanceFactor) + Vector3.up * heightFactor;
            moveTarget = observable.position + spacingVector;
            smoothTime = followSmoothTime;
        }
        else
        {
            //moving the camera towards the currentObservePosition
            moveTarget = currentObservePosition;
            smoothTime = observeSmoothTime;
        }
        transform.position = Vector3.SmoothDamp(transform.position, moveTarget, ref currentVelocity, smoothTime);

        float angle = Vector3.Angle(transform.forward, observable.position - transform.position);

        if (angle > minimalAngle)
        {
            float step = Time.deltaTime * angle * stepFactor;
            Vector3 targetDirection = observable.position - transform.position;
            Vector3 steppedDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(steppedDirection);
        }

        if (NoSwitch)
        {
            cooldownTime += Time.deltaTime;
            if (cooldownTime >= noSwitchTime)
            {
                switchCount = 0;
                cooldownTime = 0;
                NoSwitch = false;
            }
        }
        else
        {
            if (switchCount != 0)
            {
                cooldownTime += Time.deltaTime;
                if (cooldownTime >= noSwitchTime)
                {
                    switchCount = 0;
                    cooldownTime = 0;
                }
            }
        }
    }

    private void Update ()
    {
        if (!InRoomConnector && !NoSwitch && Input.GetKeyDown(KeyCode.S))
        {
            SwitchCameraMode();
        }
    }

    public void SwitchCameraMode ()
    {
        if (NoSwitch || InRoomConnector)
            return;

        followMode = !followMode;
        switchCount++;

        if (switchCount > MAX_SWITCH_COUNT)
        {
            NoSwitch = true;
            cooldownTime = 0;
        }
    }

    private void OnLeavingRoom ()
    {
        currentStartnodeIndex++;
        currentObservePosition = GetObservePosition();
        InRoomConnector = true;
    }

    private void OnStartingRoom ()
    {       
        InRoomConnector = false;
    }

    /// <summary>
    /// returns observePosition based on currentStartnodeIndex
    /// </summary>
    /// <returns></returns>
    private Vector3 GetObservePosition ()
    {
        Vector3 startnodePosition = startnodes[currentStartnodeIndex].position;

        Transform room = startnodes[currentStartnodeIndex];
        while (room.parent != null)
        {
            room = room.parent;
        }

        return Vector3.LerpUnclamped(room.position, startnodePosition, observeDistanceFactor);
    }
}
