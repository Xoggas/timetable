namespace Timetable.Api.Tests.Integration.Extensions;

public static class ArrayExtensions
{
    public static bool StringArraysEqual(this string[][] a, string[][] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (var i = 0; i < a.Length; i++)
        {
            if (a[i].Length != b[i].Length)
            {
                return false;
            }
        }
        
        for (var i = 0; i < a.Length; i++)
        {
            for (var j = 0; j < a[i].Length; j++)
            {
                if (a[i][j] != b[i][j])
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}