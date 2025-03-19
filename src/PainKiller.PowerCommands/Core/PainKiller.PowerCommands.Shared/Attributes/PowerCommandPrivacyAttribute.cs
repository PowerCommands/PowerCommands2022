namespace PainKiller.PowerCommands.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PowerCommandPrivacyAttribute : Attribute
    {
        /// <summary>
        /// If true sensitive input will not be logged.
        /// </summary>
        public bool Confidential { get; set; } = true;
    }
}