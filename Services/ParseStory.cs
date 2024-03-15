using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FixerGame {
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
            Logger.Log("Adding decision event");
            Logger.Log(decisionEvent.Name);
            Logger.Log(decisionEvent.Description);
            Logger.Log(decisionEvent.NodeType);
            eventHolder.Events.Add(decisionEvent.Name, decisionEvent);
        } else if (storyEvent is CombatEvent combatEvent)
        {
            Logger.Log("Adding combat event");
            Logger.Log(combatEvent.Name);
            Logger.Log(combatEvent.Description);
            Logger.Log(combatEvent.NodeType);
            eventHolder.Events.Add(combatEvent.Name, combatEvent);
        } else if (storyEvent is EndEvent endEvent)
        {
            Logger.Log("Adding end event");
            Logger.Log(endEvent.Name);
            Logger.Log(endEvent.Description);
            Logger.Log(endEvent.NodeType);
            eventHolder.Events.Add(endEvent.Name, endEvent);
        } else{
            throw new ArgumentException($"Invalid node type: {storyEvent.NodeType}");
        }
    }
    return eventHolder;
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
                if (agent.TraitList != null)
                {
                    agentTraits.AddRange(agent.TraitList.Select(trait => trait.ToString()));
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


    public class OnWin
    {
        public string OnWinPointer { get; set; }
        public RewardList RewardList { get; set; }
    }

    public class RewardList
    {
        public int Experience { get; set; }
        public int Gold { get; set; }
        public List<string> Items { get; set; }
    }

}