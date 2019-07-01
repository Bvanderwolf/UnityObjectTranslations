using UnityEngine;

public interface ITranslatable 
{
    void Update (Transform _myTransform, Transform _targetTransform);
    void Reset (NodeAttributes node, Transform from);
    void Clean ();
    void Init (ObjectNavigation navigation);
    string CurrentFunctionName ();
}
