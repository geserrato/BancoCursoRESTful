﻿using Microsoft.AspNetCore.Identity;

namespace Identity.Model;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
}