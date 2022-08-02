using Dragons_Breath.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Models
{
    public class BTUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name ="Full Name")]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; }  }

        [Display(Name = "Avatar")]
        [NotMapped]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize((1024 * 1024 * 1))]
        [MinFileSize(1024)]
        public IFormFile FormFile { get; set; }

        public string ImagePath { get; set; }

        public byte[] ImageData { get; set; }

        public DateTimeOffset LastNotificationCheck { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }
    }
}
