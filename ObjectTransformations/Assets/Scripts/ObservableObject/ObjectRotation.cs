public class ObjectRotation : ObjectTranslation
{
    private void Start ()
    {
        navigation = GetComponent<ObjectNavigation>();
        navigation.OnNodeReached += OnNodeReached;

        feedback = GetComponent<ObjectUserFeedback>();

        currentTranslateType = new StandardRotation(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.eulerAngles);
        feedback.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName());
    }

    private void FixedUpdate ()
    {
        if (currentTranslateType is StandardRotation && navigation.IsLookingAtTarget)
            return;

        currentTranslateType.Update(transform, navigation.MoveTarget);
    }

    protected override void OnNodeReached (NodeAttributes node)
    {
        base.OnNodeReached(node);
        feedback.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName());
    }
}
