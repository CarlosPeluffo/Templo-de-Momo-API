using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Templo_de_Momo.Models
{
    public abstract class Base
    {
        protected readonly IConfiguration configuration; 
        protected readonly string connectionString;
        protected Base(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}