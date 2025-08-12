using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Update Vector3", story: "[GameObject] position sets [Vector3]", category: "Action", id: "71bb226f493abec1f4bb1fa23fb0566d")]
public partial class UpdateVector3Action : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> gameObject;
    [SerializeReference] public BlackboardVariable<Vector3> vector3;

    protected override Status OnStart()
    {
        vector3.Value = gameObject.Value.transform.position;
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

