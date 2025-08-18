using Domain.Base.Enums;
using System.Text.Json.Serialization;

namespace Domain.Settings;

/// <summary>
/// Representa as configurações para Acesso ao banco de dados.
/// </summary>
[Serializable]
public class ConnectionSettings
{
    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    [JsonPropertyName("Sgbd")]
    public required SgbdType Sgbd { get; set; }

    [JsonPropertyName("Host")]
    public required string Host { get; set; }

    [JsonPropertyName("Port")]
    public required int Port { get; set; }

    [JsonPropertyName("Schema")]
    public string? Schema { get; set; }

    [JsonPropertyName("SID")]
    public string? SID { get; set; }

    [JsonPropertyName("Database")]
    public string? Database { get; set; }

    [JsonPropertyName("User")]
    public required string User { get; set; }

    [JsonPropertyName("Password")]
    public required string Password { get; set; }

    [JsonIgnore]
    public string ConnectionString
    {
        get
        {
            return Sgbd switch
            {
                SgbdType.Oracle =>
                    string.Format(
                        "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));Persist Security Info=True;User ID={3};Password={4};Pooling=True;Connection Timeout=60;",
                        Host, Port, SID, User, Password
                    ),

                SgbdType.SqlServer =>
                    $"Server={Host},{Port};Database={Database};User Id={User};Password={Password};TrustServerCertificate=True;",

                SgbdType.PostgreSql =>
                    $"Host={Host};Port={Port};Database={Database};Username={User};Password={Password}",

                SgbdType.MySql =>
                    $"Server={Host};Port={Port};Database={Database};Uid={User};Pwd={Password};",

                SgbdType.Sqlite =>
                    $"Data Source={Database}",

                _ => throw new NotSupportedException($"SGBD '{Sgbd}' não suportado.")
            };
        }
    }

    public ConnectionSettings() { }

    public ConnectionSettings(SgbdType sgbd, string host, int port, string database, string user, string password)
    {
        if (sgbd == SgbdType.Oracle)
            throw new ArgumentException("Use o construtor com SID para Oracle.");

        Sgbd = sgbd;
        Host = host;
        Port = port;
        Database = database;
        User = user;
        Password = password;
    }

    public ConnectionSettings(string host, int port, string sid, string user, string password)
    {
        Sgbd = SgbdType.Oracle;
        Host = host;
        Port = port;
        SID = sid;
        User = user;
        Password = password;
    }
}
