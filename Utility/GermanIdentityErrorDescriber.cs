namespace PasswordSolution.Utility;

public class GermanIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length)
        => new() { Code = nameof(PasswordTooShort), Description = $"Das Passwort muss mindestens {length} Zeichen lang sein." };

    public override IdentityError PasswordRequiresUpper()
        => new() { Code = nameof(PasswordRequiresUpper), Description = "Das Passwort muss mindestens einen Großbuchstaben (A–Z) enthalten." };

    public override IdentityError PasswordRequiresLower()
        => new() { Code = nameof(PasswordRequiresLower), Description = "Das Passwort muss mindestens einen Kleinbuchstaben (a–z) enthalten." };

    public override IdentityError PasswordRequiresDigit()
        => new() { Code = nameof(PasswordRequiresDigit), Description = "Das Passwort muss mindestens eine Zahl (0–9) enthalten." };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Das Passwort muss mindestens ein Sonderzeichen enthalten." };

    public override IdentityError DuplicateUserName(string userName)
        => new() { Code = nameof(DuplicateUserName), Description = $"Der Benutzername '{userName}' ist bereits vergeben." };

    public override IdentityError InvalidUserName(string userName)
        => new() { Code = nameof(InvalidUserName), Description = $"Der Benutzername '{userName}' ist ungültig." };

    public override IdentityError DefaultError()
        => new() { Code = nameof(DefaultError), Description = "Ein unbekannter Fehler ist aufgetreten." };
}
