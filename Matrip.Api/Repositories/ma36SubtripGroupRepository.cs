using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma36SubtripGroupRepository : BaseRepository<ma36SubtripGroup>, Ima36SubtripGroupRepository
    {
        public ma36SubtripGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public List<ma36SubtripGroup> GetSubtripGroupList(ma16subtripschedule ma16Subtripschedule, DateTime Date)
        {
            var query = _DbContext.ma36SubtripGroup.Where(e => e.FK3614idsubtrip == ma16Subtripschedule.FK1614idsubtrip)
                .Include(e => e.ma14subtrip);
            if (query.Any())
            {
                return query.ToList();
            }
            return null;
        }
    }
}
