using System;

namespace AspCqrs.Application.Common
{
    public class AuditableDto
    {
        public DateTime Created { get; set; }
        
        public DateTime? LasModified { get; set; }
    }
}