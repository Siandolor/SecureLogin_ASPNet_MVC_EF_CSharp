namespace PasswordSolution.Utility;

public static class SaltUtility
{
    public static byte[] GenerateShaSalt(int length = 32)
    {
        byte[] salt = new byte[length];
        RandomNumberGenerator.Fill(salt);

        using var hashAlg = CreateSha3OrFallback();
        var hashed = hashAlg.ComputeHash(salt);

        for (int i = 0; i < hashed.Length / 2; i++)
            hashed[i] ^= hashed[hashed.Length - 1 - i];

        return hashed.Take(length).ToArray();
    }
    
    public static byte[] GenerateArgonSalt(int length = 32)
    {
        byte[] salt = new byte[length];
        RandomNumberGenerator.Fill(salt);

        byte[] extra = new byte[length];
        RandomNumberGenerator.Fill(extra);

        for (int i = 0; i < length; i++)
        {
            salt[i] ^= extra[(i * 7) % length];
            salt[i] = (byte)((salt[i] << (i % 8)) | (salt[i] >> (8 - (i % 8))));
        }

        using var hashAlg = CreateSha3OrFallback();
        return hashAlg.ComputeHash(salt).Take(length).ToArray();
    }
    
    private static HashAlgorithm CreateSha3OrFallback()
    {
        try
        {
            return Sha3.Sha3512();
        }
        catch
        {
            return SHA512.Create();
        }
    }
}
