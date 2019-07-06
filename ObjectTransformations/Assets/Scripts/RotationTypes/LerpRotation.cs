using System.Collections.Generic;
using UnityEngine;

public class LerpRotation : LerpableTranslation, ITranslatable
{
    protected Queue<LerpRotateFunction> lerpFunctions;

    public LerpRotation (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from)
    {
        lerpFunctions = GetRotationLerpFunctionQueue();
    }

    public void Update (Transform _myTransform, Transform _targetTransform)
    {
        if (currentTranslateTime < translateTime)
        {
            currentTranslateTime += Time.deltaTime * translateSpeed;
        }

        float t = currentTranslateTime / translateTime;

        //update object based on first lerpFunction in queue
        if (_targetTransform != null && lerpFunctions.Count != 0)
        {
            
        }
    }

    public void Reset (NodeAttributes node, Transform from)
    {
        ResetCurrentTranslateTime();
        fromTranslation = from.eulerAngles;

        /*if node = null it means we reached a node created after reaching a functionnode so we 
         deque to start using the next function in the lerpFunctions queue*/
        if (node == null && lerpFunctions.Count != 0)
        {
            lerpFunctions.Dequeue();
        }
    }

    public void Clean ()
    {
        lerpFunctions.Clear();
        lerpFunctions = null;
    }

    public void Init (ObjectNavigation navigation, NodeAttributes node)
    {

    }

    public string CurrentFunctionName ()
    {
        if (lerpFunctions.Count != 0)
        {
            return lerpFunctions.Peek().Method.Name;
        }
        else
        {
            return "";
        }
    }
}
