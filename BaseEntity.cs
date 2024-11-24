using System;
using System.Collections.Generic;
 using System.Linq;
namespace LibraryManagementSystem
{
    public abstract class BaseEntity
    {
        // Unique identifier for the entity
        public Guid Id { get; } = Guid.NewGuid();
        public string Title{ get; }
        
        // Date when the entity was created
        public DateTime CreatedDate { get; set; }

        // Constructor to initialize the CreatedDate property
        protected BaseEntity(string title, DateTime? createdDate = null)
        {
           Title = title;
           CreatedDate = createdDate ?? DateTime.UtcNow;
        }
    }
}
