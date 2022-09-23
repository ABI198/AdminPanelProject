using ABISoft.BlogApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Entities.Dtos
{
    public class ArticleUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olamaz.")]
        [MinLength(5, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string Title { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MinLength(20, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string Content { get; set; }

        [DisplayName("Thumbnail")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(200, ErrorMessage = "{0} alanı {1} karakterden büyük olamaz.")]
        [MinLength(5, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string Thumbnail { get; set; }

        [DisplayName("Tarih")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] //Suitable for Turkey
        public DateTime Date { get; set; } //Just like CreatedDate but different.

        [DisplayName("Seo Yazar")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(50, ErrorMessage = "{0} alanı {1} karakterden büyük olamaz.")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string SeoAuthor { get; set; }

        [DisplayName("Seo Açıklama")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(50, ErrorMessage = "{0} alanı {1} karakterden büyük olamaz.")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string SeoDescription { get; set; }

        [DisplayName("Seo Etiketler")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(50, ErrorMessage = "{0} alanı {1} karakterden büyük olamaz.")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olamaz.")]
        public string SeoTags { get; set; }

        [DisplayName("Aktif Mi?")] //Draft or not?
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public bool IsActive { get; set; }

        [DisplayName("Silinsin Mi?")] 
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public bool IsDeleted { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
