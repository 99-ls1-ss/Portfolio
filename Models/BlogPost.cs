using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portfolio.Models {

    public class BlogPost {
        public BlogPost() {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTimeOffset Created { get; set; }
        //public DateTime Created { get { return DateTime; } }
        [Display(Name = "Date Updated")]
        public Nullable<System.DateTimeOffset> Updated { get; set; }
        //public Nullable<System.DateTimeOffset> Updated { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Body")]
        public string Body { get; set; }
        [Display(Name = "Media URL")]
        public string MediaURL { get; set; }
        [Display(Name = "Make Public")]
        public bool IsPublished { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}