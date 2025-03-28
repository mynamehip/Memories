﻿using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<UserRole> UserRoles { get; set; } = new();

        public User(string name, string password, List<UserRole>? userRoles)
        {
            Id = Guid.NewGuid();
            Username = name;
            Password = password;
            UserRoles = userRoles ?? new();
        }

        public User()
        {
        }
    }
}
