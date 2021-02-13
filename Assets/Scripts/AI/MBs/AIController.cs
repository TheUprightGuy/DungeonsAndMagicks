using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIType
{
    SCOUT,
    DEFENDER,
    TURRET
}

public class AIController : MonoBehaviour
{

    public static AIController Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Callback Handler Exists!");
            Destroy(this.gameObject);
        }
        Instance = this;

        RegisterActions();
        GenerateTemplates();
        barHandler = transform.GetComponentInChildren<AIBarHandler>();

    }


    [HideInInspector]
    public float MutationChance = 0.0f;

    [HideInInspector]
    public List<AIAction> MoveActions = new List<AIAction>();
    [HideInInspector]
    public List<AIAction> AttackActions = new List<AIAction>();
    [HideInInspector]
    public List<AIAgent> AgentTemplates = new List<AIAgent>();
    [HideInInspector]
    public List<Transform> AgentTransforms = new List<Transform>();
    private void RegisterActions()
    {
        //Add each action, add to approriate List<>
        //MoveActions.Add(new BaseMove());
        AttackActions.Add(new HomingAttack());
    }
     
    private void GenerateTemplates()
    {
        ScoutAI scoutAI = new ScoutAI();
        scoutAI.Randomise(1);
        AgentTemplates.Add(scoutAI);

        //Create each sub-class, add to AgentTemplates
    }


    private void Update()
    {
        foreach (var item in AgentTransforms)
        {
            if (item == null)
            {
                AgentTransforms.Remove(item);
            }
        }
    }


    AIBarHandler barHandler;
    /// <summary>
    /// Gets the AIAgentTemplate from the list, applys a random muation based on MutationChance
    /// </summary>
    /// <param name="aiType">The type to pull from the list</param>
    /// <param name="_AttachedTransform">The transform the AIAgent will be controlling</param>
    /// <param name="_TargetTransform">The transform the AIAgent will be targeting</param>
    /// <returns>AIAgent of type AIType with apropriate modifiers</returns>
    public AIAgent RequestAgent(AIType aiType, Transform _AttachedTransform, Transform _TargetTransform)
    {
        //Get correct template from Agenttemplates, apply mutation based on mutchance
        foreach (AIAgent item in AgentTemplates)
        {

            if (item.thisAIType == aiType)
            {
                AIAgent CopiedAgent = item.ShallowCopy();
                CopiedAgent.TargetTransform = _TargetTransform;
                CopiedAgent.RootTransform = _AttachedTransform;
                CopiedAgent.AINavAgent = _AttachedTransform.GetComponent<NavMeshAgent>();

                AgentTransforms.Add(_AttachedTransform);

                float randChance = UnityEngine.Random.Range(0.0f, 1.0f);
                if (randChance > MutationChance) //Check if mutation present
                {
                    CopiedAgent.Randomise(1);
                }

                return CopiedAgent;
            }
        }

        return null;
    }

}
