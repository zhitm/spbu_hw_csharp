using System.ComponentModel.DataAnnotations;

namespace Journal;

public class Data
{
    public int DataId { get; set; }

    [Required(ErrorMessage = "Введите имя")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Введите название предмета")]
    public string Subject { get; set; } = "";

    [Required(ErrorMessage = "Введите оценку")]
    [RegularExpression("[0-9]|[10]", ErrorMessage = "Оценка - число от 0 до 10")]
    public int Grade { get; set; } = 0;
}