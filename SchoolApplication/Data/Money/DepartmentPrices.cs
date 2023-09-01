using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using SchoolApplication.Models;
using System;
using System.Reflection.PortableExecutable;

namespace SchoolApplication.Data.Money
{
    public static class DepartmentPrices
    {
        public static decimal machineeng { get; set; } = 100000m;
        public static decimal eleceng { get; set; } = 120000m;
        public static decimal compeeng { get; set; } = 140000m;
        public static decimal chemeng { get; set; } = 110000m;
        public static decimal indeng { get; set; } = 120000m;
        public static decimal medicine { get; set; } = 200000m;
        public static decimal psychology { get; set; } = 120000m;
        public static decimal economy { get; set; } = 100000m;
        public static decimal mediaanddesign { get; set; } = 110000m;
        public static decimal business { get; set; } = 130000m;
    };
}
  