using Samples.Core.Input;
using System.Numerics;

namespace Samples.Desktop.Input
{
    public class SimpleInput : ISimpleInput
    {
        public static readonly SimpleInput Instance = new SimpleInput();

        public Vector2 Stick
        {
            get
            {
                var stick = Vector2.Zero;
                stick.X -= Left ? 1 : 0;
                stick.X += Right ? 1 : 0;

                stick.Y -= Up ? 1 : 0;
                stick.Y += Down ? 1 : 0;

                if (stick.Length() > 0) return Vector2.Normalize(stick);
                return Vector2.Zero;
            }
        }

        public BtnState A { get; set; }
        public BtnState B { get; set; }

        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }

        public void Reset()
        {
            A &= ~BtnState.JustChanged;
            B &= ~BtnState.JustChanged;
        }
    }
}
