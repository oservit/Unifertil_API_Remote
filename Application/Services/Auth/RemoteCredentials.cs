namespace Application.Services.Auth
{
    /// <summary>
    /// Atributos de autenticação (Credenciais).
    /// </summary>
    public class RemoteCredentials
    {
        /// <summary>
        /// Usuário para autenticação na API remota.
        /// </summary>
        public string User { get; set; } = null!;

        /// <summary>
        /// Senha Criptografada.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Url Base.
        /// </summary>
        public string BaseUrl { get; set; }

        public RemoteCredentials(string user, string password, string url)
        {
            User = user;
            Password = password;
            BaseUrl = url;
        }
    }
}
