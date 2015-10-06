using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portfolio.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models {
    public class Comment {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        public System.DateTimeOffset Created { get; set; }
        public Nullable<System.DateTimeOffset> Updated { get; set; }
        public string UpdateReason { get; set; }
        public bool IsApproved { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual BlogPost Post { get; set; }
    }
}