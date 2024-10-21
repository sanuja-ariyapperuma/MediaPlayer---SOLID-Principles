
namespace MediaPlayer.Service.DTO.UserDTO
{
    public class ReadUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string UserCategory { get; set; } = String.Empty;
    }
}
