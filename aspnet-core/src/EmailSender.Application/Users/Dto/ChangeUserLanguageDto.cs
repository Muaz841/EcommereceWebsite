using System.ComponentModel.DataAnnotations;

namespace EmailSender.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}