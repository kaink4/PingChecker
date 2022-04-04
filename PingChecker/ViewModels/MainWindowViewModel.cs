using Microsoft.Extensions.Options;
using PingChecker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PingChecker.Infrastructure;
using PingChecker.Views;
using System.Threading;
using System.Media;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Windows.Media;
using System.IO;
using PingChecker.ViewModelsDesign;

namespace PingChecker.ViewModels
{
    public class MainWindowViewModel : MainWindowViewModelDesign
    {
        private readonly ISampleService _sampleService;
        private readonly IOptions<AppSettings> _options;
        private readonly IWindowFactory _windowFactory;

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);

       

        private string _results = "";
        public override string Results
        {
            get => _results;
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }

        private string _site = "www.google.pl";
        public override string Site
        {
            get => _site;
            set
            {
                _site = value;
                OnPropertyChanged(nameof(Site));
            }
        }

        private int _pingThreshold = 40;
        public override int PingThreshold
        {
            get => _pingThreshold;
            set
            {
                _pingThreshold = value;
                OnPropertyChanged(nameof(PingThreshold));
            }
        }

        private readonly SoundPlayer _soundPlayer = new (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/BELLLrg_Church bell (ID 0135)_BSB.wav"));
        private bool _alarm = false;


        public MainWindowViewModel(ISampleService sampleService, IOptions<AppSettings> options, IWindowFactory windowFactory)
        {
            _sampleService = sampleService;
            _options = options;
            _windowFactory = windowFactory;
            _soundPlayer.LoadAsync();

            StartPinging();
        }

        

        private void StartPinging()
        {
            //Task.Run(() =>
            //{
            //    var player = new SoundPlayer();
            //    player.SoundLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notification_Chime.wav");

            //    while (true)
            //    {
            //        if (_playAlarm)
            //        {
            //            player.PlayLooping();
            //        }
            //        else
            //        {
            //            player.Stop();
            //        }

            //        Thread.Sleep(1000);
            //    }
            //});



            //Task.Run(async () =>
            //{
            //    var player = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/BELLLrg_Church bell (ID 0135)_BSB.wav"));
            //    player.Load();

            //    while(true)
            //    {
            //        if(_alarm)
            //        {
            //            player.PlayLooping();
            //        }
                    

            //        Thread.Sleep(_timeout/2);
            //    }
            //});

            Task.Run(async () =>
            {
                

                var results = new Queue<(string Status, long? Ping)>();
                var ping = new Ping();


                var stringBuilder = new StringBuilder();


                while (true)
                {
                    var sleepTime = _timeout;

                    try
                    {
                        var result = await ping.SendPingAsync(Site, (int)_timeout.TotalMilliseconds);

                        if (result.Status == IPStatus.Success)
                        {
                            results.Enqueue((result.Status.ToString(), result.RoundtripTime));

                        }
                        else
                        {
                            results.Enqueue((result.Status.ToString(), null));
                        }

                        sleepTime = _timeout - TimeSpan.FromMilliseconds(result.RoundtripTime);
                    }
                    catch (PingException)
                    {
                        results.Enqueue(("Exception", null));
                    }

                    if(results.Count > 14)
                    {
                        results.Dequeue();
                    }

                    var lastResult = results.TakeLast(5).Select(x => x.Ping);
                    if(lastResult.Any(x => x == null) || lastResult.Any(x => x > PingThreshold))
                    {
                        _soundPlayer.Stop();
                        _alarm = false;
                    }
                    else
                    {
                        if (!_alarm)
                        {
                            _soundPlayer.PlayLooping();
                            _alarm = true;
                        }
                    }

                    foreach (var result in results)
                    {
                        if (result.Ping != null)
                        {
                            stringBuilder.AppendLine($"{result.Status} {result.Ping} ms");
                        }
                        else 
                        {
                            stringBuilder.AppendLine($"{result.Status}");
                        }
                    }
              
                    Results = stringBuilder.ToString();
                    stringBuilder.Clear();

                    Thread.Sleep(sleepTime);
                }
            });
        }



        //public ICommand GetSettingsCommand => new RelayCommand(_ => SomeText = JsonSerializer.Serialize(_options.Value));
        //public ICommand ShowSampleWindowCommand => new RelayCommand<string?>(mode =>
        //{
        //    var sampleWindow = _windowFactory.CreateWindow<SampleWindow>();
        //    if (mode == "modal")
        //    {
        //        sampleWindow.ShowDialog();
        //    }
        //    else
        //    {
        //        sampleWindow.Show();
        //    }
        //});
    }
}
