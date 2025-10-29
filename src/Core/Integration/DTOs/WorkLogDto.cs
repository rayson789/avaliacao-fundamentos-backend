namespace Core.Integration.DTOs;
public class WorkLogDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
