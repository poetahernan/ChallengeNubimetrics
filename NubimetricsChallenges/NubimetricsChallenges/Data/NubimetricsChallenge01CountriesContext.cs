using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NubimetricsChallenge01Countries.Models
{
    public class NubimetricsChallenge01CountriesContext : DbContext
    {
        public NubimetricsChallenge01CountriesContext (DbContextOptions<NubimetricsChallenge01CountriesContext> options)
            : base(options)
        {
        }

        public DbSet<NubimetricsChallenge01Countries.Models.User> User { get; set; }
    }
}
