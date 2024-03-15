namespace FixerGame
{
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
                foreach (KeyValuePair<string, Choice> choice in Choices)
                {
                    Console.WriteLine($"{choice.Key}: {choice.Value.Description}");
                }
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
}