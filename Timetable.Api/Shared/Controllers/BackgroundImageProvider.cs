namespace TimeTableBackend.Shared.Controllers;

public sealed class BackgroundImageProvider
{
    private readonly string[] _springImagesPaths = Directory.GetFiles("BackgroundImages/Spring", "*.jpg");
    private readonly string[] _summerImagesPaths = Directory.GetFiles("BackgroundImages/Summer", "*.jpg");
    private readonly string[] _autumnImagesPaths = Directory.GetFiles("BackgroundImages/Autumn", "*.jpg");
    private readonly string[] _winterImagesPaths = Directory.GetFiles("BackgroundImages/Winter", "*.jpg");

    public async Task<byte[]> GetRandomBackgroundImageBytes()
    {
        var paths = GetPathsAccordingToCurrentMonth();
        var randomPath = paths[Random.Shared.Next(0, paths.Length)];
        return await File.ReadAllBytesAsync(randomPath);
    }

    private string[] GetPathsAccordingToCurrentMonth()
    {
        var month = DateTime.Now.Month;

        return month switch
        {
            >= 3 and <= 5 => _springImagesPaths,
            >= 6 and <= 8 => _summerImagesPaths,
            >= 9 and <= 11 => _autumnImagesPaths,
            12 or 1 or 2 => _winterImagesPaths,
            _ => throw new ArgumentOutOfRangeException(nameof(month), month, null)
        };
    }
}