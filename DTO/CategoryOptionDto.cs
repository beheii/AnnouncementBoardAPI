namespace NoticeBoard.DTO;

public class CategoryOptionDto
{
    public string Name { get; set; } = string.Empty;
    public List<string> SubCategories { get; set; } = [];
}

