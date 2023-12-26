namespace PainKiller.PowerCommands.Core.Services;
public static class ListService
{
    private const int TopMargin = 5;
    private static readonly List<ListDialogItem> SelectedItems = new();
    public static Dictionary<int, string> ListDialog(string header, List<string> items, bool multiSelect = false, bool autoSelectIfOnlyOneItem = true, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Blue, bool clearConsole = true)
    {
        SelectedItems.Clear();
        if (items.Count == 0) return new Dictionary<int, string>();
        
        var listItems = items.ToListDialogItems();
        if (listItems.Count - TopMargin > Console.WindowHeight)
        {
            clearConsole = true;
            var currentPage = 0;
            var pageSize = (Console.WindowHeight < TopMargin ? TopMargin*2 : Console.WindowHeight) - TopMargin;
            var shouldContinue = true;
            var totalPages = (listItems.Count/pageSize)+1;
            while (shouldContinue)
            {
                var pageItems = listItems.Skip(currentPage * pageSize).Take(pageSize).ToList();
                var lastPageFound = pageItems.Max(p => p.DisplayIndex) == listItems.Max(l => l.DisplayIndex);
                var response = ListDialogPage(header, pageItems, multiSelect, autoSelectIfOnlyOneItem, foregroundColor, backgroundColor, clearConsole, currentPage, totalPages, items.Count);

                if (response.Count == 0) return new();
                if (response.First().Caption == "a" && multiSelect) return listItems.ToDictionary();
                if (response.Count > 1) return response.ToDictionary();

                if (response.First().ItemIndex < 0)
                {
                    if (response.First().Caption == "n" && !lastPageFound)
                    {
                        currentPage++;
                    }
                    else if (currentPage > 0 && response.First().Caption == "p") currentPage--;
                }
                else
                {
                    ToolbarService.ClearToolbar();
                    return response.ToDictionary(); 
                }
            }
        }
        ToolbarService.ClearToolbar();
        var retVal = ListDialogPage(header, listItems, multiSelect, autoSelectIfOnlyOneItem, foregroundColor, backgroundColor, clearConsole);
        if (retVal.Count > 0 && retVal.First().Caption == "a" && multiSelect) return listItems.ToDictionary();
        return retVal.ToDictionary();
    }
    private static List<ListDialogItem> ListDialogPage(string header, List<ListDialogItem> items, bool multiSelect = false, bool autoSelectIfOnlyOneItem = true, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Blue, bool clearConsole = true, int currentPage = -1, int totalPages = 1, int totalItems=0)
    {
        if (clearConsole) Console.Clear();
        WriteLabel($"{header}\n");
        var startRow = Console.CursorTop;
        
        var listCaption =  (currentPage > -1) ? multiSelect ? $"Page {currentPage+1} of {totalPages} ({totalItems} items).  Enter (n) for next page (p) for previous, (a) select all." : $"Page ({currentPage+1}) Enter (n) for next page (p) for previous" : multiSelect ? "Use (a) to select all" : "";
        var footer = multiSelect ? "Enter number(s) and use enter to select them. (blank to quit)" : "Enter a number and hit enter to select. (blank to quit)";
        
        RenderList(items, startRow, listCaption, footer, foregroundColor, backgroundColor);

        if (items.Count == 1 && autoSelectIfOnlyOneItem)
        {
            RenderList(items, startRow, "Autoselected", "", foregroundColor, backgroundColor);
            SelectedItems.Add(items.First());
            return SelectedItems;
        }

        var quit = false;
        while (!quit)
        {
            var input = $"{Console.ReadLine()}".Trim().ToLower();
            if(input == "") break;
            
            if (multiSelect && input == "a")
            {
                RenderList(items.Select(i => new ListDialogItem{Caption = i.Caption,DisplayIndex = i.DisplayIndex,ItemIndex = i.ItemIndex,RowIndex = i.RowIndex,Selected = true}).ToList(), startRow, listCaption, footer, foregroundColor, backgroundColor);
                return [new() { ItemIndex = -1, Caption = input }];
            }

            if (input is "n" or "p") return [new() { ItemIndex = -1, Caption = input }];

            var selectedIndex = (int.TryParse(input, out var index) ? index : 1);
            if (selectedIndex > items.Max(i => i.DisplayIndex)) selectedIndex = items.Max(i => i.DisplayIndex);
            if (selectedIndex < items.Min(i => i.DisplayIndex)) selectedIndex = items.Min(i => i.DisplayIndex);
            var selectedItem = items.First(i => i.DisplayIndex == selectedIndex);
            SelectedItems.Remove(selectedItem);
            selectedItem.Selected = !selectedItem.Selected;
            if(selectedItem.Selected) SelectedItems.Add(selectedItem);
            RenderList(items, startRow, listCaption, footer, foregroundColor, backgroundColor);
            if (selectedIndex > 0 && !multiSelect) quit = true;
        }
        return SelectedItems;
    }
    private static void RenderList(List<ListDialogItem> items, int startRow, string header, string footer, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Console.CursorLeft = 0;
        Console.CursorTop = startRow;
        ConsoleService.Service.ClearRow(startRow);
        
        WriteHeader($"{header}\n");
        var padLength = items.Max(i => i.DisplayIndex) < 100 ? 3 : 4;
        foreach (var item in items)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{item.DisplayIndex}.".PadRight(padLength));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(item.Caption);
            Console.ForegroundColor = originalColor;
        }
        foreach (var selectedItem in items.Where(i => i.Selected))
        {
            ConsoleService.Service.WriteRowWithColor((startRow) + selectedItem.RowIndex, foregroundColor, backgroundColor, $"{selectedItem.DisplayIndex}.".PadRight(padLength) + $"{selectedItem.Caption}");
        }
        ConsoleService.Service.ClearRow(Console.CursorTop);
        WriteHeader($"{footer} >");
    }
    private static List<ListDialogItem> ToListDialogItems(this List<string> items)
    {
        var pageSize = Console.WindowHeight - TopMargin;
        var retVal = new List<ListDialogItem>();
        var index = 0;
        foreach (var item in items)
        {
            var dialogItem = new ListDialogItem{ItemIndex = index};
            var currentPage = index / (pageSize==0 ? 1 : pageSize);
            dialogItem.DisplayIndex = index+1;
            dialogItem.Caption = item;
            dialogItem.RowIndex = dialogItem.DisplayIndex - (currentPage * pageSize);
            retVal.Add(dialogItem);
            index++;
        }
        return retVal;
    }
    private static Dictionary<int, string> ToDictionary(this List<ListDialogItem> items)
    {
        var dictionary = new Dictionary<int, string>();
        foreach (var item in items) dictionary[item.ItemIndex] = item.Caption;
        return dictionary;
    }
    private static void WriteHeader(string text)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }
    private static void WriteLabel(string label)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{label}");
        Console.ForegroundColor = originalColor;
    }
}