using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Journal.Pages;

public class ThanksModel : PageModel
{
    public Data Participant { get; set; } = new();
    public void OnGet(Data participant)
    {
        Participant = participant;
    }
}