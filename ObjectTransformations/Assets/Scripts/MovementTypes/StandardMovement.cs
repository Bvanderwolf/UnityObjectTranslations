using UnityEngine;

public class StandardMovement : LerpableTranslation, ITranslatable
{
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

    public void Init (ObjectNavigation navigation)
    {

    }

    public string CurrentFunctionName ()
    {
        return "Default Movement";
    }
}
