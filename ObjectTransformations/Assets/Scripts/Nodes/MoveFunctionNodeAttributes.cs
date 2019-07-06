public class MoveFunctionNodeAttributes : FunctionNodeAttributes
{
    public override ITranslatable GetFunctionType (ObjectTranslation translation)
    {
        //translation is of type ObjectRotation
        if (translation is ObjectRotation)
        {
            return new StandardRotation(translateSpeed, translateTime, translation.Navigation.LastTarget.eulerAngles);
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
