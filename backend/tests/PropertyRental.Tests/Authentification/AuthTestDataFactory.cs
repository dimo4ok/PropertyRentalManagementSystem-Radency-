using PropertyRental.Application.Authentification.Commands.SignIn;
using PropertyRental.Application.Authentification.Commands.SignUp;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Tests.Authentification;

public static class AuthTestDataFactory
{
    public static SignUpCommand GetSignUpCommand(UserRole userRole = UserRole.Applicant)
    {
        var model = new SignUpModel(
            UserName: "testuser",
            Email: "test@example.com",
            FirstName: "Test",
            LastName: "User",
            Password: "Test123!@#",
            PhoneNumber: "+40712345678",
            UserRole: userRole);

        return new SignUpCommand(model);
    }


    public static SignInCommand GetSignInCommand()
    {
        var model = new SignInModel(
            UserName: "testuser",
            Password: "Test123!@#");

        return new SignInCommand(model);
    }
}