using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portfolio.Models {
    public class ContactModel {
        public string contactName { get; set; }
        public string contactEmail { get; set; }
        public string contactSubject { get; set; }
        public string contactMessage { get; set; }
    }
}