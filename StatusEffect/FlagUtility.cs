public class FlagUtility
{

    /// <summary>
    /// <paramref name="value"/>에서 <paramref name="flag"/>에 해당 되는 값을 true로 변환 후 반환
    /// </summary>
    public static int OnFlag(int value, int flag)
    {
        return value |= flag;
    }

    /// <summary>
    /// <paramref name="value"/>에서 <paramref name="flag"/>에 해당 되는 값을 false로 변환 후 반환
    /// </summary>
    public static int OffFlag(int value, int flag)
    {
        return value &= ~flag;
    }


    /// <summary>
    /// <paramref name="value"/>에서 <paramref name="flag"/>에 해당 되는 값이 true인지 false인지 반환
    /// </summary>
    public static bool IsFlag(int value, int flag)
    {
        return (value & flag) == flag;
    }
}