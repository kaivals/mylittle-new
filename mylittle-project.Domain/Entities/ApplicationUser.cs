using Microsoft.AspNetCore.Identity;
using System;

namespace mylittle_project.Domain.Entities
{
    /// <summary>
    /// Custom Identity user with only essential properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Whether the user is soft-deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Time when the account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last login time of the user.
        /// </summary>
        public DateTime? LastLoginAt { get; set; }
    }
}
