using UnityEngine;

public class FunctionNodeAttributes : NodeAttributes
{
    [SerializeField] private Vector3 functionExcecutionSpace;
    [SerializeField] private uint functionCount;

    [SerializeField] private float translateSpeed;
    [SerializeField] private float translateTime;

    public Vector3 FunctionExcecutionSpace
    {
        get => functionExcecutionSpace;
    }

    public uint FunctionCount
    {
        get => functionCount;
    }

    /// <summary>
    /// gives back a functionType based on RoomName and parameter given
    /// </summary>
    /// <param name="translation">use this keyword to gain a rotationType or a movementType function based on your script</param>
    /// <returns></returns>
    public ITranslatable GetFunctionType (ObjectTranslation translation)
    {
        string parent = transform.parent.parent.name;
        int index = parent.IndexOf('_');
        string function = parent.Substring(index + 1, parent.Length - index - 1);

        //translation is of type ObjectRotation
        if (translation is ObjectRotation)
        {
            switch (function)
            {
                case "LERPROTATION": return new LerpRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
                default: return new StandardRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
            }
        }
        else //translation is of type ObjectMovement
        {
            switch (function)
            {
                case "LERPMOVEMENT": return new LerpMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
                case "BAZIERCURVE": return new BazierCurveMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
                default: return new StandardMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
            }
        }
    }
}
