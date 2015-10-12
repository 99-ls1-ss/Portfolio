using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portfolio.Models {
    class BlogAccountViewModel {

        BlogPost blogPost = new BlogPost();
        ApplicationUser applicationUser = new ApplicationUser();

        //BlogAccountViewModel viewModel = new BlogAccountViewModel(blogPost, applicationUser);
        //BlogAccountViewModel  account = new AccountModel();
        //BlogAccountViewModel viewModel = new BlogAccountViewModel(blog, account);

        public BlogAccountViewModel(BlogPost blogPost, ApplicationUser applicationUser) {
            // TODO: Complete member initialization
            this.blogPost = blogPost;
            this.applicationUser = applicationUser;

            BlogAccountViewModel viewModel = new BlogAccountViewModel(blogPost, applicationUser);
        }
    }
}
