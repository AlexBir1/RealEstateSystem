﻿using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IAgreementRepository : IRepository<Agreement>
    {
        Task<IEnumerable<Agreement>> GetAllByAccountIdAsync(string accountId);
    }
}
