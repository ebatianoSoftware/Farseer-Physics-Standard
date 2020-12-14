using System.Drawing;
using System.Numerics;

namespace Samples.Core.Rendering
{
    public interface IRenderCanvas
    {
        Vector2 Center { get; set; }
        float Scale { get;  set;}
        SizeF Size { get; }
        Color StrokeColor { get; set; }
        Color FillColor { get; set; }
        Color TextColor { get; set; }
        float StrokeThickness { get; set; }
        string FontFamily { get; set; }
        float FontSize { get; set; }
        void Clear(Color color);
        void DrawRect(RectangleF rect);
        void DrawOval(RectangleF rect);
        void DrawCircle(Vector2 center, float radius);
        void DrawLine(Vector2 p1, Vector2 p2);
        void DrawPolyline(Vector2[] points, int count);
        void DrawPolygon(Vector2[] points, int count);
        void DrawText(string text, Vector2 position);
        Vector2 MeasureText(string text);
    }
}
