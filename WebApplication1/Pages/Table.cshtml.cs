using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Journal.Pages;

public class TableModel : PageModel
{
    private readonly DbClassJournalContext context;
    public TableModel(DbClassJournalContext context)
        => this.context = context;
    public IList<Data> Participants { get; private set; } = new List<Data>();
    public void OnGet()
    {
        Participants = context.Participants.OrderBy(p => p.DataId).ToList();
    }
}