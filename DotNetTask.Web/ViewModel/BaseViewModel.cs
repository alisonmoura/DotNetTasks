namespace DotNetTask.Web.ViewModel;

public class BaseViewModel
{
    public bool ShowError { get; set; } = false;
    public bool ShowInfo { get; set; } = false;

    public string Message { get; set; } = "";

    public Dictionary<string, List<string>> ErrorFields = [];
}