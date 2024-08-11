using LibraryManagerApp.Data.Enum;
using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerApp.Data.Dto
{
    public class BookCreateModel
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; }

        [MaxLength(100)]
        public string Publisher { get; set; }

        [Range(1,int.MaxValue)]
        public int? PublishedYear { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Range(1,int.MaxValue)]
        public int TotalPages {  get; set; }

        [MaxLength(512)]
        public string ImageUrl { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public Author Author { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public BookShelf ShelfName { get; set; }
    }
}
