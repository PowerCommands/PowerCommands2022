#nullable disable

namespace PainKiller.PowerCommands.DrontlogCommands.Entities
{
    public class WriterInput
    {
        public Guid WriterInputID { get; set; }
        public Guid ProviderServiceID { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string InputStatus { get; set; }
        public string UrlToLogo { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
    }
}