using System;

namespace Samples.Core.Rendering
{
    public interface IRedrawRequest
    {
        event Action Redraw;
    }
}
