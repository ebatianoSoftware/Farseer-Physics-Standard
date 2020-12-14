using Samples.Core.Demos;
using Samples.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Samples.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IRedrawRequest
    {
        private DemoScene currentScene;

        public event Action Redraw;

        public DemoScene CurrentScene { get => currentScene; set => SetProperty(ref currentScene, value); }

        public int CurrentDemoIndex 
        { 
            get => currentDemoIndex;
            set
            {
                SetProperty(ref currentDemoIndex, value);
            }
        }

        public List<DemoScene> DemoList { get; } = new List<DemoScene>();

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int currentDemoIndex;

        public MainWindowViewModel()
        {
            CurrentScene = new SingleFixtureDemo();
            DemoList.Add(CurrentScene);
            DemoList.Add(new SingleFixtureDemo());

            Task.Run(() => Loop(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        protected override void OnPropertyChanged(string name)
        {
            if (name == nameof(CurrentDemoIndex))
            {
                CurrentScene = DemoList[CurrentDemoIndex];
                CurrentScene.Reset();
            }
        }

        private async Task Loop(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Redraw?.Invoke();
            await Task.Delay(1000);

            var lastFrame = stopwatch.Elapsed;

            while (!cancellationToken.IsCancellationRequested)
            {
                var frame = stopwatch.Elapsed;
                var delta = (frame - lastFrame).TotalSeconds;
                lastFrame = frame;

                CurrentScene?.Update((float)delta);
                Redraw?.Invoke();
                await Task.Delay(15);
            }
        }
    }
}
