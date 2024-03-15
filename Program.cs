using CommandLine;
using System.Diagnostics;

namespace FixerGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
            {
                if (o.Debug)
                {
                    Logger.SetDebugMode(true);
                }
                
                string story_file = o.StoryInputPath;
                string agent_file = o.AgentInputPath;

                Logger.Log($"Story file: {story_file}");
                Logger.Log($"Agent file: {agent_file}");

                // Load Agents and build agejnt list from file
                string agentJson = File.ReadAllText(agent_file);
                List<Agent> agentList = Agent.LoadAgentList(agentJson);

                // Load the story from the file
                string json = File.ReadAllText(story_file);
                EventHolder story = EventHolder.LoadEventHolder(json);
                string startEventId = story.startEventId;

                Console.WriteLine("Welcome to Fixer Game!");
                Console.WriteLine("Story: " + story.StoryName);
                Console.WriteLine("Description: " + story.StoryDescription);

                Console.WriteLine("Here is who you have hired for this job:");
                foreach (Agent agent in agentList)
                {
                    agent.PrintStats();
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                ProcStory(story, startEventId, agentList);
                Logger.Log("Well that ran...");
            })
            .WithNotParsed<Options>(errors =>
            {
                Console.WriteLine("Error parsing command line options");
            });
            // List<Agent> agentList = new List<Agent>();
            // Agent agent1 = new Agent("V", "Netrunner", 18, 4, 4, 4, 4, 4);
            // agentList.Add(agent1);
        }

        public static void ProcStory(EventHolder story, string startEventId, List<Agent> agentList)
        {
            string currentEventId = startEventId;
            string nextPointer;

            while(currentEventId != null)
            {
                StoryEvent currentEvent = story.Events[currentEventId];
                // TODO: Change this to read the typeOf() and stop messing about with separate string params
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

    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; }

        [Option('s', "story_input", Required = true, HelpText = "Story file path.")]
        public string StoryInputPath { get; set; }

        [Option('a', "agent_input", Required = true, HelpText = "Agent file path.")]
        public string AgentInputPath { get; set; }

        [Option('d', "debug", Required = false, HelpText = "Enable debug mode.")]
        public bool Debug { get; set; }
    }
}