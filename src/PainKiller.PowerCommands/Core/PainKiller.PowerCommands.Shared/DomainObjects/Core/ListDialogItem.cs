namespace PainKiller.PowerCommands.Shared.DomainObjects.Core
{
    public class ListDialogItem
    {
        public int DisplayIndex { get; set; }
        public int ItemIndex { get; set; }
        public int RowIndex { get; set; }
        public bool Selected { get; set; }
        public string Caption { get; set; } = string.Empty;
    }
}