using System.Collections.Generic;
using UnityEngine;

public class LerpableTranslation : Translation
{
    protected delegate Vector3 LerpMoveFunction(Vector3 P1, Vector3 P2, float t);
    protected delegate Quaternion LerpRotateFunction(Quaternion O1, Quaternion O2, float t);

    public LerpableTranslation (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from){}

    /// <summary>
    /// use this to use normal lerp movement
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 NormalMoveLerp (Vector3 P1, Vector3 P2, float t)
    {      
        return Vector3.Lerp(P1, P2, t);
    }

    protected Quaternion NormalRotateLerp(Quaternion O1, Quaternion O2, float t)
    {
        return Quaternion.Lerp(O1, O2, t);
    }

    /// <summary>
    /// use this to ease out you lerp movement
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 SineLerp (Vector3 P1, Vector3 P2, float t)
    {
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(P1, P2, t);
    }

    /// <summary>
    /// use this to ease in your lerp movement
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 CosineLerp(Vector3 P1, Vector3 P2, float t)
    {
        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(P1, P2, t);
    }

    /// <summary>
    /// use this to use exponential movement on your lerp
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 QuadraticLerp(Vector3 P1, Vector3 P2, float t)
    {
        t = t * t;
        return Vector3.Lerp(P1, P2, t); 
    }

    /// <summary>
    /// use this to ease in and also ease out
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 SmoothStepLerp (Vector3 P1, Vector3 P2, float t)
    {
        t = t * t * (3f - 2f * t);
        return Vector3.Lerp(P1, P2, t);
    }

    /// <summary>
    /// use this to use an even smoother smoothstep
    /// </summary>
    /// <param name="t">your travel percentage</param>
    /// <returns></returns>
    protected Vector3 SmootherStepLerp(Vector3 P1, Vector3 P2, float t)
    {
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        return Vector3.Lerp(P1, P2, t);
    }

    /// <summary>
    /// Returns a Queue of all the lerp functions related to movement
    /// </summary>
    /// <returns></returns>
    protected Queue<LerpMoveFunction> GetMoveLerpFunctionQueue ()
    {
        Queue<LerpMoveFunction> queue = new Queue<LerpMoveFunction>();
        queue.Enqueue(NormalMoveLerp);
        queue.Enqueue(SineLerp);
        queue.Enqueue(CosineLerp);
        queue.Enqueue(QuadraticLerp);
        queue.Enqueue(SmoothStepLerp);
        queue.Enqueue(SmootherStepLerp);
        return queue;
    }

    /// <summary>
    /// returns a Queue with lerp functions related to rotation
    /// </summary>
    /// <returns></returns>
    protected Queue<LerpRotateFunction> GetRotationLerpFunctionQueue ()
    {
        Queue<LerpRotateFunction> queue = new Queue<LerpRotateFunction>();
        queue.Enqueue(NormalRotateLerp);
        return queue;
    }
}
