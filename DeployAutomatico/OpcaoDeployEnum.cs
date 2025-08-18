namespace DeployAutomatico
{
    public enum  OpcaoDeployEnum
    {
        /// <summary>
        /// Realiza o deploy somente do módulo backend.
        /// </summary>
        Backend = 1,
        /// <summary>
        /// Realiza o deploy somente do módulo frontend.
        /// </summary>
        Frontend = 2,
        /// <summary>
        /// Realiza o deploy completo, backend e frontend.
        /// </summary>
        Completo = 3
    }
}
