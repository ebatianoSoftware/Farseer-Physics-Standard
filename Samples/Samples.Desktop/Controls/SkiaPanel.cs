using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Skia;
using Avalonia.Threading;
using Samples.Core.Rendering;

namespace Samples.Desktop.Controls
{
    public class SkiaPanel : Control
    {
        public static readonly StyledProperty<IWindowRenderer> WindowRendererProperty = AvaloniaProperty.Register<SkiaPanel, IWindowRenderer>(nameof(WindowRenderer));
        public static readonly StyledProperty<IRedrawRequest> RedrawRequestProperty = AvaloniaProperty.Register<SkiaPanel, IRedrawRequest>(nameof(RedrawRequest));

        private RenderTargetBitmap renderTarget;
        private ISkiaDrawingContextImpl skiaDrawingContext;

        private readonly RenderCanvas renderCanvas = new RenderCanvas();

        public IWindowRenderer WindowRenderer
        {
            get => GetValue(WindowRendererProperty);
            set => SetValue(WindowRendererProperty, value);
        }

        public IRedrawRequest RedrawRequest
        {
            get => GetValue(RedrawRequestProperty);
            set => SetValue(RedrawRequestProperty, value);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            renderTarget?.Dispose();
            renderTarget = null;

            skiaDrawingContext?.Dispose();
            skiaDrawingContext = null;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.Property == RedrawRequestProperty)
            {
                if(args.OldValue is IRedrawRequest rr)
                {
                    rr.Redraw -= RedrawRequest_Redraw;
                }

                if(args.NewValue is IRedrawRequest rr2)
                {
                    rr2.Redraw += RedrawRequest_Redraw;
                }
            }
        }

        private void RedrawRequest_Redraw()
        {
            Dispatcher.UIThread.Post(() => InvalidateVisual());
        }

        public override void Render(DrawingContext context)
        {
            PrepareRenderTarget();

            if (skiaDrawingContext == null) return;

            var canvas = skiaDrawingContext.SkCanvas;

            if (WindowRenderer != null)
            {
                var scale = App.Current.DpiScale;
                canvas.Scale((float)scale);

                renderCanvas.Canvas = canvas;
                renderCanvas.Size = new System.Drawing.SizeF((float)Bounds.Width, (float)Bounds.Height);

                WindowRenderer.Render(renderCanvas);
                canvas.ResetMatrix();
            }
            else
            {
                canvas.Clear(new SkiaSharp.SKColor(128, 128, 128));
            }

            context.DrawImage(renderTarget, 1.0,
                new Rect(0, 0, renderTarget.PixelSize.Width, renderTarget.PixelSize.Height),
                new Rect(0, 0, Bounds.Width, Bounds.Height));
        }

        private void PrepareRenderTarget()
        {
            if (double.IsNaN(Bounds.Width) || Bounds.Width < 1) return;
            if (double.IsNaN(Bounds.Height) || Bounds.Height < 1) return;

            var scale = App.Current.DpiScale;
            var pixelSize = new PixelSize((int)(scale * Bounds.Width), (int)(scale * Bounds.Height));

            if (renderTarget == null || renderTarget.PixelSize != pixelSize)
            {
                renderTarget?.Dispose();
                renderTarget = new RenderTargetBitmap(pixelSize);

                skiaDrawingContext?.Dispose();
                skiaDrawingContext = (ISkiaDrawingContextImpl)renderTarget.CreateDrawingContext(null);
            }
        }
    }
}
