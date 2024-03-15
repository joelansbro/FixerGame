namespace FixerGame
{
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
}