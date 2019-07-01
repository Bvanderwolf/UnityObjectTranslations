using UnityEngine;

public class ObjectMovement : ObjectTranslation
{  
    private void Start ()
    {
        navigation = GetComponent<ObjectNavigation>();
        navigation.OnNodeReached += OnNodeReached;

        currentTranslateType = new StandardMovement(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.position);
        navigation.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName() + "\n");
    }

    private void FixedUpdate ()
    {
        if (navigation.LightsFlickering)
            return;

        navigation.SetLastPosition(this, transform.position);
        currentTranslateType.Update(transform, navigation.MoveTarget);
    }
    
    protected override void OnNodeReached (NodeAttributes node)
    {
        base.OnNodeReached(node);
        navigation.AddFunctionNameToTextMesh(currentTranslateType.CurrentFunctionName() + "\n");
    }
}
