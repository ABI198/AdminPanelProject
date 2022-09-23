using ABISoft.BlogApp.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Entities.Concrete
{
    public class Article : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Date { get; set; } //Just like CreatedDate but different.
        public int ViewsCount { get; set; } = 0; 
        public int CommentCount { get; set; } = 0; 
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public Category Category { get; set; }   //NP
        public int CategoryId { get; set; }  //FK
        public User User { get; set; }  //NP
        public int UserId { get; set; } //FK
        public ICollection<Comment> Comments { get; set; } //NP
    }
}
