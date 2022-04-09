using System.Windows;

namespace PingChecker.Infrastructure;

public interface IWindowFactory
{
    T CreateWindow<T>() where T : Window;
}
