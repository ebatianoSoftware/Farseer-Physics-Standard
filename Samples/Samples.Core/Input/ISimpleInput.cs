using System;
using System.Numerics;

namespace Samples.Core.Input
{
    [Flags]
    public enum BtnState
    {
        Up = 0,
        Down = 1,
        Released = Up | JustChanged,
        Pressed = Down | JustChanged,

        JustChanged = 0x80
    }
    public interface ISimpleInput
    {
        Vector2 Stick { get; }
        BtnState A { get; }
        BtnState B { get; }
    }
}
