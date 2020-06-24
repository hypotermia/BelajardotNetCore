using System.ComponentModel.DataAnnotations;

public class UserForRegisDto{
    
    [Required]
    public string Username { get; set; }
    [Required]
    [StringLength(8,MinimumLength = 4,ErrorMessage = "You Must Specify password between 4 and 8 characters")]
    public string Password { get; set; }
}