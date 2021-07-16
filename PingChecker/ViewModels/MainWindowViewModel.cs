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

namespace PingChecker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISampleService _sampleService;
        private readonly IOptions<AppSettings> _options;
        private readonly IWindowFactory _windowFactory;

        private string _someText = "";
        private bool _playAlarm = false;

        public string SomeText
        {
            get => _someText;
            set
            {
                _someText = value;
                OnPropertyChanged(nameof(SomeText));
            }
        }

        public MainWindowViewModel(ISampleService sampleService, IOptions<AppSettings> options, IWindowFactory windowFactory)
        {
            _sampleService = sampleService;
            _options = options;
            _windowFactory = windowFactory;
        }

        private void StartPinging()
        {
            Task.Run(() =>
            {
                var player = new SoundPlayer();
                player.SoundLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notification_Chime.wav");

                while (true)
                {
                    if (_playAlarm)
                    {
                        player.PlayLooping();
                    }
                    else
                    {
                        player.Stop();
                    }

                    Thread.Sleep(1000);
                }
            });

            Task.Run(async () =>
            {
                
                
                //var results = new List<string>();
                //var ping = new Ping();
                //var stopWatch = Stopwatch.StartNew();

                //while (true)
                //{
                //    long sleepTime = 500;

                //    try
                //    {
                //        var result = await ping.SendPingAsync("www.google.pl", 2000);

                //        if (result.Status == IPStatus.Success)
                //        {
                //            if (stopWatch.Elapsed.TotalSeconds > trackBar1.Value)
                //            {
                //                var alarmMode = GetAlarmMode();
                //                //  var pingSetting =  (int)textBox1.Invoke()

                //                if (alarmMode == AlarmMode.WhenPingLower && result.RoundtripTime < trackBar2.Value)
                //                {
                //                    _playAlarm = true;
                //                }
                //                else if (alarmMode == AlarmMode.WhenPingHigher && result.RoundtripTime > trackBar2.Value)
                //                {
                //                    _playAlarm = true;
                //                }
                //                else
                //                {
                //                    stopWatch.Restart();
                //                    _playAlarm = false;
                //                }
                //            }

                //        }
                //        else
                //        {
                //            stopWatch.Restart();
                //            _playAlarm = false;
                //        }



                //        results.Add($"{result.Status} {result.RoundtripTime} ms");

                //        sleepTime = Math.Max(sleepTime, 500 - result.RoundtripTime);
                //    }
                //    catch (PingException)
                //    {
                //        results.Add("Error");
                //        stopWatch.Stop();
                //    }


                //    var text = string.Join(Environment.NewLine, results);

                //    SetTextBox(text);



                //    Thread.Sleep(TimeSpan.FromMilliseconds(sleepTime));
              //  }
            });
        }



        public ICommand GetSettingsCommand => new RelayCommand(_ => SomeText = JsonSerializer.Serialize(_options.Value));
        public ICommand ShowSampleWindowCommand => new RelayCommand<string?>(mode =>
        {
            var sampleWindow = _windowFactory.CreateWindow<SampleWindow>();
            if (mode == "modal")
            {
                sampleWindow.ShowDialog();
            }
            else
            {
                sampleWindow.Show();
            }
        });
    }
}
