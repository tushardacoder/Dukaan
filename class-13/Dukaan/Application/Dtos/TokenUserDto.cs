namespace Dukaan.Application.Dtos
{
    public class TokenUserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid TenantId { get; set; }
    }



}
