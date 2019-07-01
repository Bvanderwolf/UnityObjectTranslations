public class ObjectRotation : ObjectTranslation
{
    private void Start ()
    {
        navigation = GetComponent<ObjectNavigation>();
        navigation.OnNodeReached += OnNodeReached;

        currentTranslateType = new StandardRotation(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.eulerAngles);
        navigation.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName());
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
        navigation.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName());
    }
}
