using Domain.Features.Clients;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class ExternalDataContext
    {
        public required DbSet<Client> Clients { get; set; }
    }
}
