namespace PasswordSolution;

public static class SecureHasher
{
    public static string HashPassword(string password, byte[] saltSha, byte[] saltArgon)
    {
        string mixedSha = InterleaveUtility.InterleaveSalt(password, saltSha);
        byte[] shaBytes = SHA512.HashData(Encoding.UTF8.GetBytes(mixedSha));
        string shaBase64 = Convert.ToBase64String(shaBytes);

        string mixedArgon = InterleaveUtility.InterleaveSalt(shaBase64, saltArgon);
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(mixedArgon))
        {
            Salt = saltArgon,
            DegreeOfParallelism = Environment.ProcessorCount,
            MemorySize = 262144,
            Iterations = 6
        };

        byte[] finalHash = argon2.GetBytes(64);
        return Convert.ToBase64String(finalHash);
    }
    
    public static bool VerifyPassword(string password, byte[] saltSha, byte[] saltArgon, string storedHash)
    {
        string computed = HashPassword(password, saltSha, saltArgon);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computed),
            Convert.FromBase64String(storedHash)
        );
    }

    public static (byte[] saltSha, byte[] saltArgon) GeneratePasswordSalts()
    {
        var saltSha = SaltUtility.GenerateShaSalt();
        var saltArgon = SaltUtility.GenerateArgonSalt();
        return (saltSha, saltArgon);
    }
}
