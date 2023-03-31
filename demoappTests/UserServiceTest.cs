using demoapp.Services;
using demoapp.Models;

using Microsoft.EntityFrameworkCore;


namespace demoappTests;

public class UserServiceTest
{

   
    [Fact]
    public async void TestGetUser()
    {
        var userService = new UserService();
        var user = await userService.GetUser(1);
        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
    }





}
