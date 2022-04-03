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

        public MainWindowViewModel(ISampleService sampleService, IOptions<AppSettings> options, IWindowFactory windowFactory)
        {
            _sampleService = sampleService;
            _options = options;
            _windowFactory = windowFactory;

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

            Task.Run(async () =>
            {


                var results = new List<(IPStatus, long)>();
                var ping = new Ping();


                while (true)
                {
                    long sleepTime = 1000;

                    try
                    {
                        var result = await ping.SendPingAsync("www.google.pl", 1000);

                        if (result.Status == IPStatus.Success)
                        {
                            //if (stopWatch.Elapsed.TotalSeconds > this.trackBar1.Value)
                            //{
                            //    var alarmMode = GetAlarmMode();
                            //    //  var pingSetting =  (int)textBox1.Invoke()

                            //    if (alarmMode == AlarmMode.WhenPingLower && result.RoundtripTime < trackBar2.Value)
                            //    {
                            //        _playAlarm = true;
                            //    }
                            //    else if (alarmMode == AlarmMode.WhenPingHigher && result.RoundtripTime > trackBar2.Value)
                            //    {
                            //        _playAlarm = true;
                            //    }
                            //    else
                            //    {
                            //        stopWatch.Restart();
                            //        _playAlarm = false;
                            //    }
                            //}

                        }
                        else
                        {
                            //stopWatch.Restart();
                            //_playAlarm = false;
                        }



                        results.Add((result.Status, result.RoundtripTime));

                        sleepTime = 1000 - result.RoundtripTime;
                    }
                    catch (PingException)
                    {
                        //results.Add(());
                        //stopWatch.Stop();
                    }

                    if(results.Count > 14)
                    {
                        results.RemoveAt(0);
                    }

                    var stringBuilder = new StringBuilder();
                    foreach (var result in results)
                    {
                        string
                    }


                    var text = string.Join(Environment.NewLine, results.Select(x => {  }));
                    Results = text;
                   // SetTextBox(text);



                    Thread.Sleep(TimeSpan.FromMilliseconds(sleepTime));
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
