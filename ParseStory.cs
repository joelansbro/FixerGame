using FixerGame;
using Newtonsoft.Json;
using System.Collections.Generic;
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
    public List<String> RewardList { get; set; }
    public int RewardMoney { get; set; }
    public int RewardExp { get; set; }

    public string startEventId { get; set; }
    public Dictionary<string, StoryEvent> Events { get; set; }
}


abstract public class StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string NodeType { get; set; }
}

public class StoryEventIntermediate
{
    public string NodeType { get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
}

public class StoryEventDeserializer {
    public static StoryEvent DeserializeStoryEvent(string json)
    {
        var intermediate = JsonConvert.DeserializeObject<StoryEventIntermediate>(json);
        var storyEvent = StoryEventFactory.CreateStoryEvent(intermediate.NodeType);
        JsonConvert.PopulateObject(json, storyEvent);

        return storyEvent;
    }
}

public static class StoryEventFactory
{
    public static StoryEvent CreateStoryEvent(string nodeType)
    {
        switch (nodeType)
        {
            case "Decision":
                return new DecisionEvent();
            case "Combat":
                return new CombatEvent();
            case "End":
                return new EndEvent();
            default:
                throw new ArgumentException($"Invalid node type: {nodeType}");
        }
    }
}


public class DecisionEvent : StoryEvent
{
    // 
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Choice> Choices { get; set; }
    public string NodeType = "Decision";

    public Choice MakeDecision()
    {
        while (true)
        {
            Console.WriteLine("Enter your decision (as an integer):");
            string input = Console.ReadLine();
            int decision;
            if (int.TryParse(input, out decision))
            {
                if (decision >= 0 && decision < Choices.Count)
                {
                    Choice choice = Choices[decision];
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
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
        // Calculate the multipliers for the choice, based on the agents' traits. 
        // If the positive multiplier is greater than the negative multiplier, the choice succeeds.
        List<string> agentTraits = new List<string>();
        foreach (Agent agent in agents)
        {
            agentTraits.AddRange(agent.traitList.Select(trait => trait.ToString()));
        }

        int posModifierCount = 0;
        int negModifierCount = 0;

        foreach (string trait in agentTraits)
        {
            posModifierCount += PosModifiers.Count(modifier => modifier == trait);
            negModifierCount += NegModifiers.Count(modifier => modifier == trait);
        }

        double posModifierMultiplier = posModifierCount * 0.1;
        double negModifierMultiplier = negModifierCount * 0.1;

        // Perform further calculations or actions based on the multipliers
        Random random = new Random();
        bool result = random.NextDouble() < FailureChance + negModifierMultiplier - posModifierMultiplier;
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

public class CombatEvent: StoryEvent
{
    public string OnWinPointer { get; set; }
    public List<String> RewardList { get; set; }
    public int RewardMoney { get; set; }
    public int RewardExp { get; set; }
    public string OnLosePointer { get; set; }
    public string NodeType = "Combat";

    public string DebugResolveCombat()
    {
        return OnWinPointer;
    }
}

public class EndEvent: StoryEvent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TeamDeath { get; set; }
    public string Success { get; set; }
    public string NodeType = "End";
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


public static class ParseStory
{
    public static EventHolder ParseStoryFile(string path)
    {
        string json = File.ReadAllText(path);
        EventHolder story = JsonConvert.DeserializeObject<EventHolder>(json)!;
        return story!;
    }
}