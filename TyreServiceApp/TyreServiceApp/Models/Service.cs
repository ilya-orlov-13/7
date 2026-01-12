using System.ComponentModel.DataAnnotations;

namespace TyreServiceApp.Models
{
    /// <summary>
    /// Представляет услугу, предоставляемую шиномонтажной мастерской.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Уникальный идентификатор услуги.
        /// </summary>
        [Key]
        [Display(Name = "Код услуги")] 
        public int ServiceCode { get; set; }

        /// <summary>
        /// Название услуги.
        /// </summary>
        [Required(ErrorMessage = "Название услуги обязательно")]
        [Display(Name = "Название услуги")] 
        [StringLength(100, ErrorMessage = "Название услуги не должно превышать 100 символов")]
        public string ServiceName { get; set; } = string.Empty;
        
        /// <summary>
        /// Стоимость услуги в рублях.
        /// </summary>
        [Required(ErrorMessage = "Стоимость услуги обязательна")]
        [Display(Name = "Стоимость услуги")] 
        [Range(0, 1000000, ErrorMessage = "Стоимость должна быть от 0 до 1 000 000 рублей")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal ServiceCost { get; set; }

        /// <summary>
        /// Навигационное свойство для выполненных работ с этой услугой.
        /// </summary>
        public ICollection<CompletedWork> CompletedWorks { get; set; } = new List<CompletedWork>();

        /// <summary>
        /// Форматированное отображение услуги с ценой.
        /// </summary>
        public override string ToString()
        {
            return $"{ServiceName} - {ServiceCost:C2}";
        }
    }
}