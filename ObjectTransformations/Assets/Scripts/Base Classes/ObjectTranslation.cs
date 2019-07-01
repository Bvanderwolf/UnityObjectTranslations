using UnityEngine;

public class ObjectTranslation : MonoBehaviour
{
    protected ObjectNavigation navigation = null;    
    protected ITranslatable currentTranslateType = null;

    public ObjectNavigation Navigation
    {
        get => navigation;
    }

    protected virtual void OnNodeReached(NodeAttributes node)
    {
        currentTranslateType.Reset(node, navigation.LastTarget);

        if (node != null)
        {
            //if we reached a functionNode we get its Function Type, else we do nothing
            if (node is FunctionNodeAttributes)
            {
                FunctionNodeAttributes functionNode = node as FunctionNodeAttributes;
                if (functionNode.FunctionCount != 0)
                {
                    currentTranslateType = functionNode.GetFunctionType(this);                    
                }
            }
        }
        else
        {
            /*if node = null it means we reached a node created after reaching a functionnode and if there are no nodes left we clean up
             else we do nothing since TranslateTypes that aren't standard have queues to switch functionality*/
            if (navigation.currentFunctionCount == 0)
            {
                currentTranslateType.Clean();
                if (this is ObjectMovement)
                {
                    currentTranslateType = new StandardMovement(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.position);
                }
                else
                {
                    currentTranslateType = new StandardRotation(navigation.ObjectSpeedModifyer, navigation.NodeToNodeTime, navigation.LastTarget.eulerAngles);
                }
            }
        }

        currentTranslateType.Init(navigation);
    }
}
