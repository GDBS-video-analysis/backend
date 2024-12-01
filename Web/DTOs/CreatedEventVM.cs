using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    /// <summary>
    /// Объект для создания (редактирования) мероприятия
    /// </summary>
    public class CreatedEventVM
    {
        /// <summary>
        /// Название
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Дата и время 
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Дата и время обязательны")]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
