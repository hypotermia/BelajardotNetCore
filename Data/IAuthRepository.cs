
using System.Threading.Tasks;


public interface IAuthRepository{
    Task<User> Register(User user,string password);
    Task<User> Login (string username ,string psasword);
    Task<bool> UserExists(string username);
}