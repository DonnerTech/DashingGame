using System;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private int health;

    public BehaviorGraphAgent behaviorAgent;

    public void Awake()
    {
        behaviorAgent.SetVariableValue("health", health);
    }
}
