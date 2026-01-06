namespace WebApp.ViewModels;

public class ModalDropdownViewModel
{
    public string Label { get; set; } = "";
    
    public string Name { get; set; } = "";

    public string Placeholder { get; set; } = "";
    
    public string? Value { get; set; }
    
    public IEnumerable<string> Items { get; set; } = [];
}