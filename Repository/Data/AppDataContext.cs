using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Settings;
using Infrastructure.Data.Oracle;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
    public partial class AppDataContext(IOptions<AppSettings> settings) : OracleDbContext(settings, "Oracle")
    {
    }
}
