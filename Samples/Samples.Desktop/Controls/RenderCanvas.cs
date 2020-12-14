using Samples.Core.Rendering;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Samples.Desktop.Controls
{
    public class RenderCanvas : IRenderCanvas
    {
        private readonly SKPaint skPaint = new SKPaint { IsAntialias = true };

        public SizeF Size { get; set; }

        public Color StrokeColor { get; set; }
        public Color FillColor { get; set; }
        public Color TextColor { get; set; }
        public float StrokeThickness { get; set; }
        public string FontFamily { get; set; }
        public float FontSize { get; set; }
        public SKCanvas Canvas { get; set; }
        public Vector2 Center { get; set; } = Vector2.Zero;
        public float Scale { get; set; } = 1;

        private SKPath path = new SKPath();

        private Dictionary<int, SKPoint[]> pointsBuffer = new Dictionary<int, SKPoint[]>();
        private Dictionary<string, SKTypeface> typefaces = new Dictionary<string, SKTypeface>();

        private SKColor ToSKColor(Color color) => new SKColor(color.R, color.G, color.B, color.A);
        private SKPoint ToSKPoint(Vector2 vec) => new SKPoint(Size.Width / 2 + (vec.X - Center.X) * Scale, Size.Height / 2 + (vec.Y - Center.Y) * Scale);
        private SKRect ToSKRect(RectangleF rect) => new SKRect(Size.Width / 2 + (rect.X - Center.X) * Scale, Size.Height / 2 + (rect.Y - Center.Y) * Scale, 
            Size.Width / 2 + (rect.Right - Center.X) * Scale, Size.Height / 2 + (rect.Bottom - Center.Y) * Scale);

        private SKPoint[] GetPointsBuffer(int count)
        {
            if (pointsBuffer.TryGetValue(count, out var buffer)) return buffer;
            buffer = new SKPoint[count];
            pointsBuffer.Add(count, buffer);
            return buffer;
        }

        private SKTypeface GetTypeFace(string name)
        {
            if (typefaces.TryGetValue(name ?? "", out var tf)) return tf;

            tf = string.IsNullOrEmpty(name) ? SKTypeface.FromFamilyName(name) : SKTypeface.Default;
            typefaces.Add(name ?? "", tf);
            return tf;
        }

        public void Clear(Color color)
        {
            Canvas.Clear(ToSKColor(color));
        }

        private bool SetStrokePaint()
        {
            if (StrokeColor.A == 0 || StrokeThickness < 0.0001f) return false;

            skPaint.Color = ToSKColor(StrokeColor);
            skPaint.IsStroke = true;
            skPaint.StrokeWidth = StrokeThickness;
            skPaint.StrokeJoin = SKStrokeJoin.Round;
            skPaint.StrokeCap = SKStrokeCap.Round;
            return true;
        }

        private bool SetFillPaint()
        {
            if (FillColor.A == 0) return false;
            skPaint.Color = ToSKColor(FillColor);
            skPaint.IsStroke = false;
            return true;
        }

        private bool PrepareTextRender()
        {
            var face = GetTypeFace(FontFamily);
            skPaint.Typeface = face;
            skPaint.TextSize = FontSize;
            skPaint.Color = ToSKColor(TextColor);
            skPaint.IsStroke = false;
            return true;
        }

        public void DrawCircle(Vector2 center, float radius)
        {
            if (SetFillPaint())
            {
                Canvas.DrawCircle(ToSKPoint(center), radius * Scale, skPaint);
            }
            
            if (SetStrokePaint())
            {
                Canvas.DrawCircle(ToSKPoint(center), radius * Scale, skPaint);
            }
        }

        public void DrawLine(Vector2 p1, Vector2 p2)
        {
            if (SetStrokePaint())
            {
                Canvas.DrawLine(ToSKPoint(p1), ToSKPoint(p2), skPaint);
            }
        }

        public void DrawOval(RectangleF rect)
        {
            if (SetFillPaint())
            {
                Canvas.DrawOval(ToSKRect(rect), skPaint);
            }

            if (SetStrokePaint())
            {
                Canvas.DrawOval(ToSKRect(rect), skPaint);
            }
        }

        public void DrawPolygon(Vector2[] points, int count)
        {
            path.Reset();
            path.MoveTo(ToSKPoint(points[0]));

            for (var idx = 1; idx < count; ++idx)
            {
                path.LineTo(ToSKPoint(points[idx % count]));
            }
            path.Close();
            

            if (SetFillPaint())
            {
                Canvas.DrawPath(path, skPaint);
            }

            if (SetStrokePaint())
            {
                Canvas.DrawPath(path, skPaint);
            }
        }

        public void DrawPolyline(Vector2[] points, int count)
        {
            path.Reset();
            path.MoveTo(ToSKPoint(points[0]));

            for (var idx = 1; idx < count; ++idx)
            {
                path.LineTo(ToSKPoint(points[idx % count]));
            }

            if (SetStrokePaint())
            {
                Canvas.DrawPath(path, skPaint);
            }
        }

        public void DrawRect(RectangleF rect)
        {
            if (SetFillPaint())
            {
                Canvas.DrawRect(ToSKRect(rect), skPaint);
            }

            if (SetStrokePaint())
            {
                Canvas.DrawRect(ToSKRect(rect), skPaint);
            }
        }

        public void DrawText(string text, Vector2 position)
        {
            if (PrepareTextRender())
            {
                Canvas.DrawText(text, position.X, position.Y, skPaint);
            }
        }

        public Vector2 MeasureText(string text)
        {
            if (PrepareTextRender())
            {
                return new Vector2(skPaint.MeasureText(text), skPaint.TextSize);
            }
            return Vector2.Zero;
        }
    }
}
