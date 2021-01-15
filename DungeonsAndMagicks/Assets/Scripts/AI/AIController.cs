using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{
    SCOUT,
    DEFENDER,
    TURRET
}

public class AIController : Singleton<AIController>
{
    public uint MutationChance = 0;

    public List<AIAction> MoveActions = new List<AIAction>();
    public List<AIAction> AttackActions = new List<AIAction>();

    public List<AIAgent> AgentTemplates = new List<AIAgent>();

    private void RegisterActions()
    {
        //Add each action, add to approriate List<>
    }
     
    private void GenerateTemplates()
    {
        //Create each sub-class, add to AgentTemplates
    }


    private void Start()
    {
        RegisterActions();
        GenerateTemplates();
    }

    /// <summary>
    /// Gets the AIAgentTemplate from the list, applys a random muation based on MutationChance
    /// </summary>
    /// <param name="aiType">The type to pull from the list</param>
    /// <param name="AttachedTransform">The transform the AIAgent will be controlling</param>
    /// <param name="TargetTransform">The transform the AIAgent will be targeting</param>
    /// <returns>AIAgent of type AIType with apropriate modifiers</returns>
    public AIAgent RequestAgent(AIType aiType, Transform AttachedTransform, Transform TargetTransform)
    {
        //Get correct template from Agenttemplates, apply mutation based on mutchance
        return null;
    }

}
