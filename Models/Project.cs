using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
public class Project
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column(TypeName = "jsonb")]
    public string Title { get; set; } = "{}";
    
    [Required]
    [Column(TypeName = "jsonb")]
    public string Description { get; set; } = "{}";
    public string? Url { get; set; }
}