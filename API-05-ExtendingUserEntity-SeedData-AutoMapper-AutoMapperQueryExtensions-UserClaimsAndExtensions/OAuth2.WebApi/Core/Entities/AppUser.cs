﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuth2.WebApi.Core.Entities;

public class AppUser
{
    /// <summary>
    /// Due to conventions don't need to put [Key] on it since the property name is convention based. 
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Auto generation like this is not happening. So check the Core/DB/DataContext.cs for more details
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid Guid { get; set; } = Guid.NewGuid();

    [Required]
    public string UserName { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }
}
