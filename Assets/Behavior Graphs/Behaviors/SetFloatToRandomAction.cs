using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Float to Random", story: "Sets [float] to random between [Min] and [Max]", category: "Action", id: "467c81cd19f2d7f8028768a0d9dbaba8")]
public partial class SetFloatToRandomAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Float;
    [SerializeReference] public BlackboardVariable<float> Min;
    [SerializeReference] public BlackboardVariable<float> Max;

    protected override Status OnStart()
    {
        Float.Value = UnityEngine.Random.Range(Min, Max);
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

