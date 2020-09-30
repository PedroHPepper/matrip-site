using Matrip.Domain.Models.Entities;
using System;
using System.Collections.Generic;

namespace Matrip.Web.Repositories.Contracts
{
    public interface Ima36SubtripGroupRepository : IBaseRepository<ma36SubtripGroup>
    {
        List<ma36SubtripGroup> GetSubtripGroupList(ma16subtripschedule ma16Subtripschedule, DateTime Date);
    }
}
