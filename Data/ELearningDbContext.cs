using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RP_Identity_API.Data
{
    public class ELearningDbContext:IdentityDbContext
    {
        public ELearningDbContext(DbContextOptions<ELearningDbContext> opt):base(opt)
        {
        }
    }
}
