using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories;
using Matrip.Web.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matrip.Web.Repositories
{
    public class ma39tripEvaluationRepository : BaseRepository<ma39tripEvaluation>, Ima39tripEvaluationRepository
    {
        public ma39tripEvaluationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
