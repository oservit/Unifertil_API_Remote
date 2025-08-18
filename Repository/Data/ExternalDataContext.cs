using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Settings;
using Infrastructure.Data.MySql;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
    public partial class ExternalDataContext(IOptions<AppSettings> settings) : MySqlDbContext(settings, "Mysql")
    {
    }
}
