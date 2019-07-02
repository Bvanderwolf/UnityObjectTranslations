using UnityEngine;

public class StandardRotation : Translation, ITranslatable
{
    private const float ROTATESPEED = 1.5f;
    private const float TURN_SPEED_DAMP = 0.015f;

    private bool dynamicRotation = false;

    private ObjectUserFeedback feedback;
    private ObjectNavigation navigation;

    public StandardRotation (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from) { }

    public void Update (Transform _myTransform, Transform _targetTransform)
    {        
        Vector3 targetDirection;
        float step = 0;

        if (_targetTransform != null)
        {
            if (feedback == null && navigation == null)
            {
                feedback = _myTransform.GetComponent<ObjectUserFeedback>();
                navigation = _myTransform.GetComponent<ObjectNavigation>();
            }          

            if (dynamicRotation && !feedback.LightsFlickering)
            {
                Vector3 predictedNewPosition = Vector3.LerpUnclamped(navigation.LastPosition, _myTransform.position, 1.5f);
                targetDirection = predictedNewPosition - _myTransform.position;
                float angle = Vector3.Angle(_myTransform.forward, targetDirection);
                step = angle * Time.deltaTime * TURN_SPEED_DAMP;
            }
            else
            {
                targetDirection = _targetTransform.position - _myTransform.position;
                step = ROTATESPEED * Time.deltaTime;
            }
            
            Vector3 newDirection = Vector3.RotateTowards(_myTransform.forward, targetDirection, step, 0.0f);
            _myTransform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public void Reset (NodeAttributes node, Transform from)
    {
        ResetCurrentTranslateTime();
    }

    public void Clean ()
    {
        feedback = null;
        navigation = null;
    }

    public void Init(ObjectNavigation navigation)
    {
        dynamicRotation = navigation.MoveDirectionBasedRotation;
    }

    public string CurrentFunctionName ()
    {
        return "Default Rotation";
    }   
}
