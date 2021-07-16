using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;

namespace PingChecker.Services
{
    public class SampleService : ISampleService
    {


        public SampleService()
        {
 
        }

        public string GetCurrentDate() => DateTime.Now.ToLongDateString();


    }
}