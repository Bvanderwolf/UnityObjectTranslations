public class ObjectMovement : ObjectTranslation
{
    private void Start ()
    {
        navigation = GetComponent<ObjectNavigation>();
        navigation.OnNodeReached += OnNodeReached;

        feedback = GetComponent<ObjectUserFeedback>();

        currentTranslateType = new StandardMovement(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.position);
        feedback.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName() + "\n");
    }

    private void FixedUpdate ()
    {
        if (feedback.LightsFlickering)
            return;

        if (navigation.MoveDirectionBasedRotation)
            navigation.SetLastPosition(this, transform.position);

        currentTranslateType.Update(transform, navigation.MoveTarget);
    }

    protected override void OnNodeReached (NodeAttributes node)
    {
        base.OnNodeReached(node);
        feedback.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName() + "\n");
    }
}
