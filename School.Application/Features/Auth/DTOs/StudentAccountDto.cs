using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Features.Auth.DTOs
{
    public class StudentAccountDto
    {
        public string UserId { get; set; } = default!;
        public int StudentId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string TemporaryPassword { get; set; } = default!;
        public int? ClassId { get; set; }
        public string CreatedBy { get; set; } = default!;
    }
}
