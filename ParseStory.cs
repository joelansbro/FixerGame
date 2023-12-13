using FixerGame;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System;


public class EventHolder
{
    public string StoryId { get; set; }
    public string StoryName { get; set; }
    public string StoryDescription { get; set; }
    public int DifficultyModifier { get; set; }
    public int MaximumAgents { get; set; }
    public int MinimumAgents { get; set; }
    public RewardList RewardList { get; set; }

    public string startEventId { get; set; }
    public Dictionary<string, StoryEvent> Events { get; set; }

    public string getStartStats()
    {
        return $"Story ID: {StoryId}\n" +
               $"Story Name: {StoryName}\n" +
               $"Story Description: {StoryDescription}\n" +
               $"Difficulty Modifier: {DifficultyModifier}\n" +
               $"Maximum Agents: {MaximumAgents}\n" +
               $"Minimum Agents: {MinimumAgents}\n" +
               $"Reward List: {RewardList}\n" +
               $"Start Event ID: {startEventId}\n";
    }

    public static EventHolder LoadEventHolder(string json)
{
    JObject jObject = JObject.Parse(json);
    JArray eventsArray = jObject[nameof(Events)] as JArray ?? throw new JsonException("Invalid Events array");

    EventHolder eventHolder = new()
    {
        StoryId = jObject["StoryId"]?.Value<string>() ?? throw new JsonException("Invalid StoryId"),
        StoryName = jObject["StoryName"]?.Value<string>() ?? throw new JsonException("Invalid StoryName"),
        StoryDescription = jObject["StoryDescription"]?.Value<string>() ?? throw new JsonException("Invalid StoryDescription"),
        DifficultyModifier = jObject["DifficultyModifier"]?.Value<int>() ?? throw new JsonException("Invalid DifficultyModifier"),
        MaximumAgents = jObject["MaximumAgents"]?.Value<int>() ?? throw new JsonException("Invalid MaximumAgents"),
        MinimumAgents = jObject["MinimumAgents"]?.Value<int>() ?? throw new JsonException("Invalid MinimumAgents"),
        RewardList = jObject["RewardList"]?.ToObject<RewardList>() ?? throw new JsonException("Invalid RewardList"),
        startEventId = jObject["startEventId"]?.Value<string>() ?? throw new JsonException("Invalid startEventId"),
        Events = new Dictionary<string, StoryEvent>()
    };

    foreach (JObject eventObject in eventsArray.Cast<JObject>())
    {
        StoryEvent storyEvent = StoryEvent.LoadStoryEvent(eventObject.ToString());
        if (storyEvent is DecisionEvent decisionEvent)
        {
            Console.WriteLine("Adding decision event");
            Console.WriteLine(decisionEvent.Name);
            Console.WriteLine(decisionEvent.Description);
            Console.WriteLine(decisionEvent.NodeType);
            eventHolder.Events.Add(decisionEvent.Name, decisionEvent);
        } else if (storyEvent is CombatEvent combatEvent)
        {
            Console.WriteLine("Adding combat event");
            Console.WriteLine(combatEvent.Name);
            Console.WriteLine(combatEvent.Description);
            Console.WriteLine(combatEvent.NodeType);
            eventHolder.Events.Add(combatEvent.Name, combatEvent);
        } else if (storyEvent is EndEvent endEvent)
        {
            Console.WriteLine("Adding end event");
            Console.WriteLine(endEvent.Name);
            Console.WriteLine(endEvent.Description);
            Console.WriteLine(endEvent.NodeType);
            eventHolder.Events.Add(endEvent.Name, endEvent);
        } else{
            throw new ArgumentException($"Invalid node type: {storyEvent.NodeType}");
        }
    }
    return eventHolder;
    }
}

abstract public class StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string NodeType { get; set; }

    public string getStats()
    {
        return $"Name: {Name}\n" +
               $"Description: {Description}\n" +
               $"Node Type: {NodeType}\n";
    }

    public static StoryEvent LoadStoryEvent(string json)
    {
    JObject jObject = JObject.Parse(json);

    string nodeType = jObject[nameof(NodeType)]?.Value<string>() ?? throw new JsonException("Invalid node type");

    Console.WriteLine(nodeType);
    switch (nodeType)
    {
        case "Decision":
            DecisionEvent decisionEvent = new DecisionEvent();
            decisionEvent.Name = jObject["Name"]?.Value<string>() ?? throw new JsonException("Invalid Name");
            decisionEvent.Description = jObject["Description"]?.Value<string>() ?? throw new JsonException("Invalid Description");
            decisionEvent.Choices = jObject["Choices"]?.ToObject<Dictionary<string, Choice>>() ?? throw new JsonException("Invalid Choices");
            decisionEvent.NodeType = "Decision";
            return decisionEvent;
        case "Combat":
            CombatEvent combatEvent = new CombatEvent();
            combatEvent.Name = jObject["Name"]?.Value<string>() ?? throw new JsonException("Invalid Name");
            combatEvent.Description = jObject["Description"]?.Value<string>() ?? throw new JsonException("Invalid Description");
            combatEvent.NodeType = "Combat";
            combatEvent.CombatProcess = jObject["CombatProcess"]?.ToObject<CombatProcess>() ?? throw new JsonException("Invalid CombatProcess");
            combatEvent.OnWin = jObject["OnWin"]?.ToObject<OnWin>() ?? throw new JsonException("Invalid OnWin");
            combatEvent.OnLossPointer = jObject["OnLossPointer"]?.Value<string>() ?? throw new JsonException("Invalid OnLossPointer");
            return combatEvent;
        case "End":
            EndEvent endEvent = new EndEvent();
            endEvent.Name = jObject["Name"]?.Value<string>() ?? throw new JsonException("Invalid Name");
            endEvent.Description = jObject["Description"]?.Value<string>() ?? throw new JsonException("Invalid Description");
            endEvent.TeamDeath = jObject["TeamDeath"]?.Value<string>() ?? throw new JsonException("Invalid TeamDeath");
            endEvent.Success = jObject["Success"]?.Value<string>() ?? throw new JsonException("Invalid Success");
            endEvent.NodeType = "End";
            return endEvent;
        default:
            throw new ArgumentException($"Invalid node type: {nodeType}");
    }
    }
}

public class DecisionEvent : StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<String, Choice> Choices { get; set; }
    public string NodeType { get; set; } = "Decision";

    public Choice MakeDecision()
    {
        while (true)
        {
            Console.WriteLine("Enter your decision:");
            string input = Console.ReadLine();
            if (Choices.ContainsKey(input))
            {
                Choice choice = Choices[input];
                // Perform actions based on the chosen choice
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid decision.");
            }
        }
    }
}

public class Choice
{
    // Links within Decision Objects
    public string Name { get; set; }
    public string Description { get; set; }
    public string SuccessPointer { get; set; }
    public string FailurePointer { get; set; }
    public double FailureChance { get; set; }
    public List<string> PosModifiers { get; set; }
    public List<string> NegModifiers { get; set; }
    // Add other properties as needed

    public string ResolveChoice(List<Agent> agents)
    {
        Console.WriteLine("Resolving choice");
        // Calculate the multipliers for the choice, based on the agents' traits. 
        // If the positive multiplier is greater than the negative multiplier, the choice succeeds.
        List<string> agentTraits = new List<string>();
        Console.WriteLine("Agent list:");
        foreach (Agent agent in agents)
        {
            Console.WriteLine(agent.Name);
            if (agent.traitList != null)
            {
                agentTraits.AddRange(agent.traitList.Select(trait => trait.ToString()));
            }
        }

        int posModifierCount = 0;
        int negModifierCount = 0;

        Console.WriteLine("Agent traits:");
        foreach (string trait in agentTraits)
        {
            Console.WriteLine(trait);
        }
        foreach (string trait in agentTraits)
        {
            posModifierCount += PosModifiers.Count(modifier => modifier == trait);
            negModifierCount += NegModifiers.Count(modifier => modifier == trait);
        }

        double posModifierMultiplier = posModifierCount * 0.1;
        double negModifierMultiplier = negModifierCount * 0.1;

        Console.WriteLine($"Pos Modifier Multiplier: {posModifierMultiplier}");
        Console.WriteLine($"Neg Modifier Multiplier: {negModifierMultiplier}");
        // Perform further calculations or actions based on the multipliers
        Random random = new Random();
        bool result = random.NextDouble() < FailureChance + negModifierMultiplier - posModifierMultiplier;
        Console.WriteLine($"Result: {result}");
        if (result)
        {
            return SuccessPointer;
        }
        else
        {
            return FailurePointer;
        }
        // Return the result or perform other operations
    }
}


public class EndEvent: StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TeamDeath { get; set; }
    public string Success { get; set; }
    public string NodeType { get; set; } = "End";

    public void End()
    {
        Console.WriteLine("Thanks for playing!");
    }
}

public class CombatEvent: StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string NodeType { get; set; } = "Combat";
    public CombatProcess CombatProcess { get; set; }
    public OnWin OnWin { get; set; }
    public string OnLossPointer { get; set; }

    public string DebugResolveCombat()
    {
        return OnWin.OnWinPointer;
    }
}

public class OnWin
{
    public string OnWinPointer { get; set; }
    public RewardList RewardList { get; set; }
}

public class CombatProcess
{
    public List<Enemy> Enemies { get; set; }
}

public class Enemy
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public int Level { get; set; }
    public int Body { get; set; }
    public int Cool { get; set; }
    public int Intelligence { get; set; }
    public int Reflex { get; set; }
    public int Tech { get; set; }
    public List<Weapon> weapons { get; set; }
    public int Armor { get; set; }
    public int CritChance { get; set; }
    public int Accuracy { get; set; }
    public int Evasion { get; set; }
}

public class Weapon
{
    public string Name { get; set; }
    public string Category { get; set; }
    public int BaseDamage { get; set; }
    public int CritChance { get; set; }
    public int Accuracy { get; set; }
}

public class RewardList
{
    public int Experience { get; set; }
    public int Gold { get; set; }
    public List<string> Items { get; set; }

}