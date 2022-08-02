using Dragons_Breath.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dragons_Breath.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [Display(Name = "Select Attachment")]
        [NotMapped]
        [AllowedExtensions(new string[] { ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".pdf", ".txt" })]
        [MaxFileSize((1024 * 1024 * 5))]
        [MinFileSize(1024)]
        public IFormFile FormFile { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public string ContentType { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }

        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public string UserId { get; set; }

        public virtual BTUser User { get; set; }
    }
}
