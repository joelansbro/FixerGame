
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FixerGame
{
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

    Logger.Log(nodeType);
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
}