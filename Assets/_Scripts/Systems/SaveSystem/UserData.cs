using DatabaseModels.DataTransferObjets;

public class UserData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public UserDto ToDto()
    {
        return new UserDto { Id = Id, Username = Username };
    }
}