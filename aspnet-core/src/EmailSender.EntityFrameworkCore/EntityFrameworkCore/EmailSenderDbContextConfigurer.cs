using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.EntityFrameworkCore
{
    public static class EmailSenderDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EmailSenderDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EmailSenderDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
