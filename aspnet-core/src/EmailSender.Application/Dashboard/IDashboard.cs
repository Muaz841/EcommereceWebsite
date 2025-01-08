﻿using EmailSender.Dashboard.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Dashboard
{
    public interface IDashboardService
    {
      
        Task<DashboardRevenueDto> DashboardRevenue();
        
        Task<List<StatsGraphDto>> GraphStats();
    }
}
