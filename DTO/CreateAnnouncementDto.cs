using System.ComponentModel.DataAnnotations;

namespace NoticeBoard.DTO;

public class CreateAnnouncementDto
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;

    public bool Status { get; set; } = true;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty; // Stored procedure resolves this name to CategoryId.

    [StringLength(100)]
    public string? SubCategory { get; set; } // Optional name resolved to SubCategoryId.
}
