using UnityEngine;

public class FunctionNodeAttributes : NodeAttributes
{
    [SerializeField] protected Vector3 functionExcecutionSpace;
    [SerializeField] protected uint functionCount;

    [SerializeField] protected float translateSpeed;
    [SerializeField] protected float translateTime;

    [SerializeField] protected string functionName;

    private Translation translationHolding;

    public Vector3 FunctionExcecutionSpace
    {
        get => functionExcecutionSpace;
    }

    public uint FunctionCount
    {
        get => functionCount;
    }

    private void Awake ()
    {
        
    }

    /// <summary>
    /// gives back a functionType based on RoomName and parameter given
    /// </summary>
    /// <param name="translation">use this keyword to gain a rotationType or a movementType function based on your script</param>
    /// <returns></returns>
    public virtual ITranslatable GetFunctionType (ObjectTranslation translation)
    {
        //translation is of type ObjectRotation
        if (translation is ObjectRotation)
        {
            return new StandardRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
        }
        else //translation is of type ObjectMovement
        {
            return new StandardMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
        }
    }
}
