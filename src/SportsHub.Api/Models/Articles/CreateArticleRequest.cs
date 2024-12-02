using System.ComponentModel.DataAnnotations;

namespace SportsHub.Api.Models.Articles;

public class CreateArticleRequest
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string ShortDescription { get; set; }

    [Required]
    public string Description { get; set; }
}