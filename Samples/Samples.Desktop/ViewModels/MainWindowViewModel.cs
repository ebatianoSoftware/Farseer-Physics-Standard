using Samples.Core.Demos;
using Samples.Core.Rendering;
using Samples.Desktop.Input;
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
        private int currentDemoIndex = -1;

        public MainWindowViewModel()
        {
            DemoList.Add(new SimpleBoxesDemo());
            DemoList.Add(new RestitutionDemo());
            DemoList.Add(new JumpySpiderDemo());
            DemoList.Add(new TheoJansenWalkerDemo());
            DemoList.Add(new SimplePlatformerDemo());

            CurrentDemoIndex = 0;

            Task.Run(() => Loop(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        protected override void OnPropertyChanged(string name)
        {
            if (name == nameof(CurrentDemoIndex))
            {
                DemoList[CurrentDemoIndex].Reset();
                CurrentScene = DemoList[CurrentDemoIndex];
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

                if (CurrentScene != null)
                {
                    CurrentScene.Input = SimpleInput.Instance;
                    CurrentScene.Update((float)delta);
                }
                Redraw?.Invoke();

                SimpleInput.Instance.Reset();
                await Task.Delay(15);
            }
        }
    }
}
