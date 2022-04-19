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


using PingChecker.ViewModels;
using PingChecker.Enums;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace PingChecker.ViewModelsDesign;

public class MainWindowViewModelDesign : ViewModelBase
{
    public virtual string Results
    {
        get => "Result1\nResult2\nResult3\nResult4\nResult5\n";
        set  { }
    }

    public virtual string PingLabel
    {
        get => "40 ms";
        set { }
    }

    public virtual string Site
    {
        get => "www.google.pl";
        set { }
    }

    public virtual int PingThreshold
    {
        get => 40;
        set { }
    }

    public virtual int ExpPingThreshold
    {
        get => 40;
        set { }
    }

    public virtual AlarmMode AlarmMode
    {
        get => AlarmMode.Higher;
        set { }
    }

    public virtual BitmapFrame Icon
    {
        get => BitmapFrame.Create(new MemoryStream(Resources.Green));
        set { }
    }

    public ICommand GetSettingsCommand => new RelayCommand(_ => { });

    public ICommand ShowSampleWindowCommand => new RelayCommand<string?>(_ => { });

    public ICommand ChangeAlarmMode =>  new RelayCommand(_ => { });
}

