using UnityEngine;

public class RotateFunctionNodeAttributes : FunctionNodeAttributes
{
    public override ITranslatable GetFunctionType (ObjectTranslation translation)
    {
        //translation is of type ObjectRotation
        if (translation is ObjectRotation)
        {
            switch (functionName)
            {
                case "LerpRotation": return new LerpRotation(translateSpeed, translateTime, transform.eulerAngles);
                default: return new StandardRotation(translateSpeed, translateTime, transform.eulerAngles);
            }
        }
        else //translation is of type ObjectMovement
        {
            return new StandardMovement(translateSpeed, translateTime, translation.Navigation.LastTarget.position);
        }
    }
}
