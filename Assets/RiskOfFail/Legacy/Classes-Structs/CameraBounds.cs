using System;

[Serializable]
public class CameraBounds
{
    public float min, max;

    public static CameraBounds ConvertLocalToWorldBounds(CameraBounds c, float pos)
    {
        c.min += pos;
        c.max += pos;
        return c;
    }
}