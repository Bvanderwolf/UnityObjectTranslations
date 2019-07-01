using System.Collections.Generic;
using UnityEngine;

public class BazierCurveTranslation : Translation
{
    protected delegate Vector3 BazierCurve (Vector3 P0, Vector3 P1, float t);

    private const float CURVE_STRENGHT = 2F;

    private const float CUBIC_CURVE_POSITION_1 = -0.25f;
    private const float CUBIC_CURVE_POSITION_2 = 0.75F;

    public BazierCurveTranslation (float _translateSpeed, float _translateTime, Vector3 from) : base(_translateSpeed, _translateTime, from){}

    protected Vector3 LinearBazierCurve(Vector3 P0, Vector3 P1, float t)
    {
        return P0 + (t * (P1 - P0));
    }

    protected Vector3 QuadraticBazierCurve(Vector3 P0, Vector3 P1, float t)
    {
        Vector3 center = P0 + ((P1 - P0) * 0.5f);
        Vector3 direction = Vector3.Cross(center, Vector3.up).normalized * CURVE_STRENGHT;
        Vector3 P2 = center + direction;
        return (Squared((1 - t), 2) * P0) + ((2 * (1 - t)) * (t * P2)) + (Squared(t, 2) * P1);
    }

    protected Vector3 CubicBazierCurve(Vector3 P0, Vector3 P1, float t)
    {
        Vector3 P2Center = P0 + ((P1 - P0) * -CUBIC_CURVE_POSITION_1);
        Vector3 P2Dir = Vector3.Cross(P2Center, Vector3.up).normalized * CURVE_STRENGHT;

        Vector3 P3Center = P0 + ((P1 - P0) * CUBIC_CURVE_POSITION_2);
        Vector3 P3Dir = Vector3.Cross(P3Center, Vector3.up).normalized * CURVE_STRENGHT;

        return (Squared((1 - t), 3) * P0) + (3 * Squared((1 - t), 2) * t * (P2Center + P2Dir)) + (3 * (1 - t) * Squared(t, 2) * (P3Center + P3Dir)) + (Squared(t, 3) * P1);
    }

    private float Squared(float value, int times)
    {
        float squared = value;

        for (int i = 0; i < times - 1; i++)
        {
            squared *= value;
        }
        return squared;
    }

    protected Queue<BazierCurve> GetBazierCurveFunctionQueue ()
    {
        Queue<BazierCurve> queue = new Queue<BazierCurve>();
        queue.Enqueue(LinearBazierCurve);
        queue.Enqueue(QuadraticBazierCurve);
        queue.Enqueue(CubicBazierCurve);

        return queue;
    } 
}
