namespace WebEF.Repository.IRequest
{
    public interface IPetition
    {
        Task<string> ClientAsync(string tipoPeticion, string peticion, HttpContent? body = null);
    }
}
