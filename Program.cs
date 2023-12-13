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

            string json = File.ReadAllText("data/No_Comments.json");
            EventHolder story = EventHolder.LoadEventHolder(json);
            string startEventId = story.startEventId;

            Console.WriteLine("Welcome to Fixer Game!");
            ProcStory(story, startEventId, agentList);
            Console.WriteLine("Well that ran...");
        }

        public static void ProcStory(EventHolder story, string startEventId, List<Agent> agentList)
        {
            string currentEventId = startEventId;
            string nextPointer;

            while(currentEventId != null)
            {
                StoryEvent currentEvent = story.Events[currentEventId];
                if (currentEvent is DecisionEvent decis){
                    DecisionEvent decisionEvent = (DecisionEvent)currentEvent;
                    Choice choice = decisionEvent.MakeDecision();
                    nextPointer = choice.ResolveChoice(agentList);
                    currentEventId = nextPointer;
                } else if (currentEvent is CombatEvent comb)
                {
                    CombatEvent combatEvent = (CombatEvent)currentEvent;
                    nextPointer = combatEvent.DebugResolveCombat();
                    currentEventId = nextPointer;
                } else if (currentEvent is EndEvent end){
                    EndEvent endEvent = (EndEvent)currentEvent;
                    endEvent.End();
                    
                    currentEventId = null;
                } else
                {
                    throw new ArgumentException($"Invalid node type: {currentEvent.NodeType}");
                }
            }
        }
    }
}