namespace PainKiller.PowerCommands.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PowerCommandsToolbarAttribute : Attribute
    {
        /// <summary>
        /// Set parameters for the toolbar
        /// </summary>
        /// <param name="labels">Labels separated with | character</param>
        /// <param name="postCommandLabels"></param>
        /// <param name="timer">Milliseconds before the toolbar is shown, possible min (and default) is 500 and max is 5000</param>
        /// <param name="description">Description of the toolbar</param>
        /// <param name="colors">Colors, could be null to use default colors</param>
        public PowerCommandsToolbarAttribute(string[] labels, string[]? postCommandLabels = null, int timer = 500, string description = "", ConsoleColor[]? colors = null)
        {
            Labels = labels;
            PostCommandLabels = postCommandLabels;
            Timer = timer;
            Description = description;
            Colors = colors;
        }
        public string[] Labels { get; }
        public string[]? PostCommandLabels { get; }
        public string Description { get; }
        public int Timer { get; }
        public ConsoleColor[]? Colors { get; }
    }
}