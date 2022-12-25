namespace Journal.Pages;

[BindProperties]
public class FormModel : PageModel
{
    private readonly DbClassJournalContext _context;

    public FormModel(DbClassJournalContext context)
        => _context = context;

    public Data Data { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        _context.Participants.Add(Data);

        await _context.SaveChangesAsync();
        return RedirectToPage("./Thanks", Data);
    }
}