using System;

namespace FixerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Agent> agentList = new List<Agent>();
            Agent agent1 = new Agent("V", "Netrunner", 18, 4, 4, 4, 4, 4);
            agentList.Add(agent1);

            string json = File.ReadAllText("data/Basic_Example.json");
            // there will be a problem here with the return type of DeserializeStoryEvent, this needs to bean event holder
            
            EventHolder story = ParseStory.ParseStoryFile("data/Basic_Example.json");
            string startEventId = story.startEventId;

            ProcessStory(story, startEventId, agentList);
        }

        public static void ProcessStory(EventHolder story, string startEventId, List<Agent> agentList)
        {
            string currentEventId = startEventId;
            string nextPointer;

            while(currentEventId != null)
            {
                StoryEvent currentEvent = story.Events[currentEventId];
                switch (currentEvent.NodeType)
                {
                    case "Decision":
                        DecisionEvent decisionEvent = (DecisionEvent)currentEvent;
                        Choice choice = decisionEvent.MakeDecision();
                        nextPointer = choice.ResolveChoice(agentList);
                        currentEventId = nextPointer;
                        break;
                    case "Combat":
                        CombatEvent combatEvent = (CombatEvent)currentEvent;
                        nextPointer = combatEvent.DebugResolveCombat();
                        currentEventId = nextPointer;
                        break;
                    case "End":
                        EndEvent endEvent = (EndEvent)currentEvent;
                        // endEvent.End();
                        
                        currentEventId = null;
                        break;
                    default:
                        throw new ArgumentException($"Invalid node type: {currentEvent.NodeType}");
                }
            }
        }
    }
}