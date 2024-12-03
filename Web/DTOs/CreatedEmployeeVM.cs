using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.DTOs
{
    /// <summary>
    /// Объект для создания сотрудников
    /// </summary>
    public class CreatedEmployeeVM
    {
        /// <summary>
        /// Ссылка на должность
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должность обязательна")]
        public short PostID { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия обязательна")]
        public string LastName { get; set; } = null!;
        /// <summary>
        /// Отчество
        /// </summary>
        public string? Patronymic { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Номер телефона обязателен")]
        public string Phone { get; set; } = null!;
    }
}
