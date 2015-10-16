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
        public DateTimeOffset CreatedDate { get; set; }
        public Nullable<DateTimeOffset> RemovedDate { get; set; }
        public Nullable<DateTimeOffset> UpdatedDate { get; set; }
        public string UpdateReason { get; set; }
        public string RemoveReason { get; set; }
        public bool IsRemoved { get; set; }
        public string RemovedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Slug { get; set; }

        public virtual ApplicationUser Author { get; set; }
        //public virtual IList<ApplicationUser> UserInfo { get; set; }
        public virtual BlogPost Post { get; set; }
    }
}