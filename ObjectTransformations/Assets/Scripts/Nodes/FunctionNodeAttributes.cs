using UnityEngine;

public class FunctionNodeAttributes : NodeAttributes
{
    [SerializeField] private Vector3 functionExcecutionSpace;
    [SerializeField] private uint functionCount;

    [SerializeField] private float translateSpeed;
    [SerializeField] private float translateTime;

    [SerializeField] private string functionName;

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
        //translation is of type ObjectRotation
        if (translation is ObjectRotation)
        {
            switch (functionName)
            {
                case "LerpRotation": return new LerpRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
                default: return new StandardRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
            }
        }
        else //translation is of type ObjectMovement
        {
            switch (functionName)
            {
                case "LerpMovement": return new LerpMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
                case "BazierCurveMovement": return new BazierCurveMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
                default: return new StandardMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
            }
        }
    }
}
