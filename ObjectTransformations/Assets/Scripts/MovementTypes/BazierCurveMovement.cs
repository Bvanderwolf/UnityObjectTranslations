using System.Collections.Generic;
using UnityEngine;

public class BazierCurveMovement : BazierCurveTranslation, ITranslatable
{
    private Queue<BazierCurve> bazierCurveFunctions;

    public BazierCurveMovement (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from)
    {
        bazierCurveFunctions = GetBazierCurveFunctionQueue();
    }

    public void Update (Transform _myTransform, Transform _targetTransform)
    {
        if (currentTranslateTime < translateTime)
        {
            currentTranslateTime += Time.deltaTime * translateSpeed;
        }

        float t = currentTranslateTime / translateTime;

        //update object based on first lerpFunction in queue
        if (_targetTransform != null && bazierCurveFunctions.Count != 0)
        {
            _myTransform.position = bazierCurveFunctions.Peek().Invoke(fromTranslation, _targetTransform.position, t);
        }
    }

    public void Clean ()
    {
        bazierCurveFunctions.Clear();
        bazierCurveFunctions = null;
    }

    public void Reset (NodeAttributes node, Transform from)
    {
        ResetCurrentTranslateTime();
        fromTranslation = from.position;

        /*if node = null it means we reached a node created after reaching a functionnode so we 
         deque to start using the next function in the lerpFunctions queue*/
        if (node == null && bazierCurveFunctions.Count != 0)
        {
            bazierCurveFunctions.Dequeue();
        }
    }

    public void Init (ObjectNavigation navigation)
    {
        navigation.SetRotationBasedOnMoveDirection();
    }

    public string CurrentFunctionName ()
    {
        if (bazierCurveFunctions.Count != 0)
        {
            return bazierCurveFunctions.Peek().Method.Name + "\n";
        }
        else
        {
            return "";
        }
    }
}
