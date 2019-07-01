using UnityEngine;

public class Translation
{
    protected float translateSpeed;
    protected float translateTime;
    protected float currentTranslateTime;
    protected Vector3 fromTranslation;

    public Translation (float _translateSpeed, float _translateTime, Vector3 from)
    {
        translateSpeed = _translateSpeed;
        translateTime = _translateTime;
        fromTranslation = from;
    }

    protected void ResetCurrentTranslateTime ()
    {
        currentTranslateTime = 0;
    }
}
