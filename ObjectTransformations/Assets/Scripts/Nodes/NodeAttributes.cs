using UnityEngine;

public class NodeAttributes : MonoBehaviour
{
    [SerializeField] protected Transform nextNode;

    public Transform NextNode
    {
        get => nextNode;
    }
}
