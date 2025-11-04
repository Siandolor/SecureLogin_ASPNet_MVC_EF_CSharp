namespace PasswordSolution.Utility;

public static class InterleaveUtility
{
    public static string InterleaveSalt(string input, byte[] salt)
    {
        var chars = input.ToCharArray();
        var saltChars = Convert.ToBase64String(salt).ToCharArray();
        var mixed = new StringBuilder();

        int saltIndex = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            mixed.Append(chars[i]);
            if ((i + 1) % 2 == 0 && saltIndex < saltChars.Length)
                mixed.Append(saltChars[saltIndex++]);
        }

        if (saltIndex < saltChars.Length)
            mixed.Append(saltChars.AsSpan(saltIndex));

        return mixed.ToString();
    }
}
