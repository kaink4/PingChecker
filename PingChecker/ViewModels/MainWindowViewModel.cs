using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PingChecker.Infrastructure;

using System.Threading;
using System.Media;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Windows.Media;
using System.IO;
using PingChecker.ViewModelsDesign;
using PingChecker.Enums;

namespace PingChecker.ViewModels;

public class MainWindowViewModel : MainWindowViewModelDesign
{
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

    private int _pingThreshold = 133;
    public override int PingThreshold
    {
        get => _pingThreshold;
        set
        {
            _pingThreshold = value;
            OnPropertyChanged(nameof(PingThreshold));
            OnPropertyChanged(nameof(ExpPingThreshold));
        }
    }

    public override int ExpPingThreshold
    {
        get => (int)Math.Pow(1.03, Convert.ToDouble(_pingThreshold));
    }

    private AlarmMode _alarmMode = AlarmMode.Lower;
    public override AlarmMode AlarmMode
    {
        get => _alarmMode;
        set
        {
            _alarmMode = value;
            OnPropertyChanged(nameof(AlarmMode));
        }
    }
    //private string _alarmMode = "None";
    //public override AlarmM AlarmMode
    //{
    //    get => _alarmMode;
    //    set
    //    {
    //        _alarmMode = value;
    //        OnPropertyChanged(nameof(AlarmMode));
    //    }
    //}

    private readonly SoundPlayer _soundPlayer = new (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/BELLLrg_Church bell (ID 0135)_BSB.wav"));
    private bool _alarm = false;


    public MainWindowViewModel()
    {

        _soundPlayer.LoadAsync();

        StartPinging();
    }

        

    private void StartPinging()
    {
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

                    var roundTripTime = result.Status == IPStatus.Success
                        ? result.RoundtripTime
                        : (long?)null;

                    results.Enqueue((result.Status.ToString(), roundTripTime));

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

                Func<long?, bool> alarmOnFunc = AlarmMode switch
                {
                    AlarmMode.None => x => false,
                    AlarmMode.Lower => x => x < ExpPingThreshold,
                    AlarmMode.Higher => x => x > ExpPingThreshold,
                    _ => throw new NotImplementedException(),
                };


                if (!lastResult.Any(x => x == null) && !lastResult.Any(x => !alarmOnFunc(x)))
                {
                    if (!_alarm)
                    {
                        _soundPlayer.PlayLooping();
                        _alarm = true;
                    }
                }
                else
                {
                    _soundPlayer.Stop();
                    _alarm = false;
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

