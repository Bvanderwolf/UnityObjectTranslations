using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectNavigation : MonoBehaviour
{
    [SerializeField, Tooltip("Next node to move towards")]
    private Transform moveTarget;

    [SerializeField, Tooltip("Influences the speed with which the object travels")]
    private float objectSpeedModifyer;

    [SerializeField, Tooltip("Influences the time it takes for the object to reach a node")]
    private float nodeToNodeTime;

    [SerializeField, Tooltip("Decides on use of rotation based on move direction or based on target node")]
    private bool moveDirectionBasedRotation;

    [SerializeField]
    private GameObject nodePrefab;

    [SerializeField]
    private TextMeshProUGUI functionTextMesh;

    [SerializeField] private Transform[] objectLights;
    [SerializeField] private float lightLerpSpeed;

    private bool lightsFlickering;

    public delegate void NodeReached (NodeAttributes node);
    public event NodeReached OnNodeReached;

    public delegate void LeavingRoom ();
    public event LeavingRoom OnLeavingRoom;

    public delegate void StartingRoom ();
    public event StartingRoom OnStartingRoom;

    private Vector3 functionExcecutionSpace;
    private Transform lastFunctionNode = null;

    private Transform lastTarget = null;
    private Vector3 lastPosition;

    private const float LASTPOSITIONCHECK = 0.6f;
    private float currentCheckTimer = 0;

    public uint currentFunctionCount { get; private set; } = 0;

    public float ObjectSpeedModifyer
    {
        get => objectSpeedModifyer;
    }

    public float NodeToNodeTime
    {
        get => nodeToNodeTime;
    }

    public bool MoveDirectionBasedRotation
    {
        get => moveDirectionBasedRotation;
    }

    public bool LightsFlickering
    {
        get => lightsFlickering;
    }

    public Transform MoveTarget
    {
        get => moveTarget;
    }

    public Transform LastTarget
    {
        get => lastTarget;
    }

    public Vector3 LastPosition
    {
        get => lastPosition;
    }

    public bool IsLookingAtTarget
    {
        get => Vector3.Angle(transform.forward, MoveTarget.position - transform.position) < 0.5f;
    }

    private void Awake ()
    {
        lastTarget = transform;
    }

    private void FixedUpdate ()
    {
        if (IsNodeReached())
        {
            NodeAttributes nodeReached = moveTarget.GetComponent<NodeAttributes>();
            SetNewTargetBasedOnReachedNode(nodeReached);
            functionTextMesh.text = "";
            moveDirectionBasedRotation = false;
            OnNodeReached(nodeReached);

            if (!IsLookingAtTarget && !lightsFlickering)
            {
                StartCoroutine(FlickerLights());
            }
        }       
    }

    private IEnumerator FlickerLights ()
    {
        lightsFlickering = true;

        Renderer[] lightRends = new Renderer[objectLights.Length];
        Color[] lightColors = new Color[objectLights.Length];

        for (int i = 0; i < lightRends.Length; i++)
        {
            lightRends[i] = objectLights[i].GetComponent<Renderer>();
            lightColors[i] = lightRends[i].material.color;
        }

        while (!IsLookingAtTarget)
        {
            float t = Mathf.PingPong(Time.time * lightLerpSpeed, 1f);
            for (int i = 0; i < lightRends.Length; i++)
            {
                Color lerpCol = Color.Lerp(lightColors[i], Color.red, t);
                lightRends[i].material.SetColor("_EmissionColor", lerpCol);
            }
            yield return new WaitForFixedUpdate();
        }

        for(int i = 0; i < lightRends.Length; i++)
        {
            lightRends[i].material.SetColor("_EmissionColor", lightColors[i]);
        }
        lightsFlickering = false;
    }

    /// <summary>
    /// Add name of current function to excecute to text mesh on canvas
    /// </summary>
    /// <param name="name">name of the function</param>
    public void AddFunctionNameToTextMesh(string name)
    {
        functionTextMesh.text += name;
    }

    /// <summary>
    /// Sets the moveDirectionBasedRation boolean to true
    /// </summary>
    public void SetRotationBasedOnMoveDirection ()
    {
        moveDirectionBasedRotation = true;
    }

    public void SetLastPosition(ObjectMovement move, Vector3 lastPos)
    {
        if (move != null)
        {
            if (currentCheckTimer < LASTPOSITIONCHECK)
            {
                currentCheckTimer += Time.deltaTime;
            }
            else
            {
                currentCheckTimer = 0;
                lastPosition = lastPos;
            }            
        }
    }
    
    /// <summary>
    /// returns whether we reached the target by checking the magnitude (length) of the vector pointing at the target
    /// </summary>
    /// <returns></returns>
    public bool IsNodeReached ()
    {
        return (moveTarget.position - transform.position).magnitude < 0.01f;
    }

    /// <summary>
    /// sets new target based on the next node variable hold by the target his NodeAttributes script
    /// </summary>
    public void SetNewTargetBasedOnReachedNode (NodeAttributes _nodeReached)
    {
        lastTarget = MoveTarget;

        if (_nodeReached != null)
        {
            //we check if we reached a functionNode or not
            if (_nodeReached is FunctionNodeAttributes)
            {
                OnStartingRoom();
                FunctionNodeAttributes node = _nodeReached as FunctionNodeAttributes;

                /*if the node does not contain any functions we just move to the next node (in the case of base room for example)
                 otherwise we set our functionExcecutionSpace, currentFunctionCount, instantatiate our new target and store the node we want
                 to travel to after we are finished with the functions*/
                if (node.FunctionCount == 0)
                {
                    moveTarget = node.NextNode;
                }
                else
                {
                    functionExcecutionSpace = node.FunctionExcecutionSpace;
                    lastFunctionNode = node.transform;
                    currentFunctionCount = node.FunctionCount;

                    Vector3 targetPos = SetNewRandomFunctionSpaceTarget(_nodeReached.transform.position);
                    moveTarget = Instantiate(nodePrefab, targetPos, Quaternion.identity).GetComponent<Transform>();
                }
            }
            else
            {
                //if a node has no NextNode reference it means we are at the last node else we move towards its nextnode reference
                if (_nodeReached.NextNode != null)
                {
                    moveTarget = _nodeReached.NextNode;

                    //if the node reached has the tag: EndNode, we fire the event that tells other scripts we are leaving this room
                    if (_nodeReached.tag == "EndNode")
                    {
                        OnLeavingRoom();
                    }                                   
                }
                else
                {
                    Debug.Log("last Node");
                }
            }
        }
        else
        {
            /*if _nodereached = null it means we reached a node created after reaching a functionnode 
            which means we destroy this one and check if we need to move towards a new one based on the currentFunctionCount*/
            Destroy(moveTarget.gameObject);
            currentFunctionCount--;

            if (currentFunctionCount == 0)
            {
                moveTarget = lastFunctionNode.GetComponent<NodeAttributes>().NextNode;
            }
            else
            {
                Vector3 targetPos = SetNewRandomFunctionSpaceTarget(lastFunctionNode.position);
                moveTarget = Instantiate(nodePrefab, targetPos, Quaternion.identity).GetComponent<Transform>();
            }
        }
    }

    /// <summary>
    /// returns a random new target pos relative to the node position in the world
    /// </summary>
    /// <param name="_nodePos"></param>
    /// <returns></returns>
    private Vector3 SetNewRandomFunctionSpaceTarget (Vector3 _nodePos)
    {
        float rdmX = Random.Range(-functionExcecutionSpace.x, functionExcecutionSpace.x) + _nodePos.x;
        float rdmY = Random.Range(-functionExcecutionSpace.y, functionExcecutionSpace.y) + _nodePos.y;
        float rdmZ = Random.Range(-functionExcecutionSpace.z, functionExcecutionSpace.z) + _nodePos.z;
        return new Vector3(rdmX, rdmY, rdmZ);
    }

    //private void OnDrawGizmos ()
    //{
    //    Gizmos.color = Color.red;        
    //    Gizmos.DrawLine(transform.position, Vector3.LerpUnclamped(lastPosition, transform.position, 1.5f));
    //}
}
