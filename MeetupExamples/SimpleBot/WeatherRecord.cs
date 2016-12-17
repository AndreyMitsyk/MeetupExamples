﻿using System;

namespace SimpleBot
{
    public class WeatherRecord
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public DateTime When { get; set; }
        public string Date => $"{When.Day:D2}.{When.Month:D2}";

        public string FullDate => When.ToString("D");
    }
}