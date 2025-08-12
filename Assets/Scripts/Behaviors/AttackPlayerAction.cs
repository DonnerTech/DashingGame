using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack Player", story: "The [Agent] attacks the [Target]", category: "Action/Animation", id: "d5689b12ea6f11de03afc0e4d7680a52")]
public partial class AttackPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> SecondsToTurn;
    [CreateProperty] private float m_Timer = 0.0f;

    [CreateProperty] private Quaternion originalRotation;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        originalRotation = Agent.Value.transform.rotation;

        m_Timer = SecondsToTurn;
        if (m_Timer <= 0.0f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        Vector3 direction = Target.Value.transform.position - Agent.Value.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Agent.Value.transform.rotation = Quaternion.Lerp(originalRotation,lookRotation,1 - m_Timer/SecondsToTurn);

        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            //Attack Code
            
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Debug.Log("Attack Success");
    }
}

