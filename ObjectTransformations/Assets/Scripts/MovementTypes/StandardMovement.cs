using UnityEngine;

public class StandardMovement : LerpableTranslation, ITranslatable
{
    bool nonStandardRotation;

    public StandardMovement (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from){}

    public void Update (Transform _myTransform, Transform _targetTransform)
    {        
        if (currentTranslateTime < translateTime)
        {
            currentTranslateTime += Time.deltaTime * translateSpeed;
        }               

        float t = currentTranslateTime / translateTime;       

        if (_targetTransform != null)
        {
            _myTransform.position = SmootherStepLerp(fromTranslation, _targetTransform.position, t);
        }        
    }

    public void Reset (NodeAttributes node, Transform from)
    {
        ResetCurrentTranslateTime();
        fromTranslation = from.position;
    }

    public void Clean()
    {
    }

    public void Init (ObjectNavigation navigation, NodeAttributes node)
    {
        /*the standard movement script automatically behaves differently when instantiated after reaching a function node, because this means that
        we are using a nonstandard version of rotation. A function must be either a movement function or rotation function*/
        if (node is FunctionNodeAttributes)
        {
            nonStandardRotation = (node as FunctionNodeAttributes).FunctionCount != 0;
            Debug.Log((node as FunctionNodeAttributes).FunctionCount);
        }
        else
        {
            nonStandardRotation = false;
        }        
    }

    public string CurrentFunctionName ()
    {
        return "Default Movement";
    }
}
