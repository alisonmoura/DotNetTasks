namespace DotNetTask.Web.ViewModel;

public class BaseViewModel
{
    public bool ShowError { get; set; } = false;

    public string Error { get; set; } = "";

    public Dictionary<string, List<string>> ErrorFields = [];
}