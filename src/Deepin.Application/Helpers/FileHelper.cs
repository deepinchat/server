namespace Deepin.Application.Helpers;
public static class FileHelper
{
    public static string GetCRC32Checksum(Stream stream, CancellationToken cancellationToken = default)
    {
        stream.Position = 0;
        uint polynomial = 0xEDB88320;
        uint[] table = new uint[256];
        uint temp;

        for (uint i = 0; i < table.Length; i++)
        {
            temp = i;
            for (int j = 8; j > 0; j--)
            {
                if ((temp & 1) == 1)
                    temp = (temp >> 1) ^ polynomial;
                else
                    temp >>= 1;
            }
            table[i] = temp;
        }

        uint crc = 0xFFFFFFFF;
        int ch;
        while ((ch = stream.ReadByte()) != -1)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();

            crc = (crc >> 8) ^ table[(crc & 0xFF) ^ (byte)ch];
        }
        stream.Position = 0;
        return (~crc).ToString("X8");
    }

    public static async Task<string> GetSHA256HashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            stream.Position = 0;
            byte[] hashBytes = await sha256.ComputeHashAsync(stream, cancellationToken);
            stream.Position = 0;
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();
        }
    }

    public static bool VerifyCRC32ChecksumAsync(Stream stream, string checksum, CancellationToken cancellationToken = default)
    {
        if (checksum.Length != 8)
        {
            throw new ArgumentException("Invalid checksum format");
        }
        var currentChecksum = GetCRC32Checksum(stream, cancellationToken);
        return string.Equals(currentChecksum, checksum, StringComparison.OrdinalIgnoreCase);
    }

    public static async Task<bool> VerifySHA256HashAsync(Stream stream, string hash, CancellationToken cancellationToken = default)
    {
        if (hash.Length != 64)
        {
            return false; // Invalid hash length
        }
        var currentHash = GetSHA256HashAsync(stream, cancellationToken);
        return string.Equals(await currentHash, hash, StringComparison.OrdinalIgnoreCase);
    }
}