namespace Dukaan.Application.Dtos
{

    public record LoginRequestDTO(
    string Email,
    string Password
);

    public record AuthResponseDTO(
        string Token,
        DateTime Expiration
    );


}
