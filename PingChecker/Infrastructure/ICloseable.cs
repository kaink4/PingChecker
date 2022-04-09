using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingChecker.Infrastructure;

public interface ICloseable
{
    void Close();
}
