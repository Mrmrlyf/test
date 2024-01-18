﻿using System;

namespace Ux
{
    public interface IGameTime
    {
        long TimeStamp { get; }
        DateTime Now { get; }
        int Year { get; }
        DayOfWeek Week { get; }
        int Month { get; }
        int Day { get; }
        int Hour { get; }
        int Minute { get; }
        int Second { get; }
        int Millisecond { get; }
    }
    public class LocalTime : IGameTime
    {
        public long TimeStamp => Now.ToTimeStamp();
        public DateTime Now => DateTime.Now;

        public int Year => Now.Year;

        public DayOfWeek Week => Now.DayOfWeek;

        public int Month => Now.Month;

        public int Day => Now.Day;

        public int Hour => Now.Hour;

        public int Minute => Now.Minute;

        public int Second => Now.Second;

        public int Millisecond => Now.Millisecond;
    }

    public class ServerTime : IGameTime
    {
        long offset;
        public void SetOffset(long offset)
        {
            this.offset = offset;
        }
        public long TimeStamp => Now.ToTimeStamp();
        public DateTime Now => DateTime.Now.AddMilliseconds(offset);

        public int Year => Now.Year;

        public DayOfWeek Week => Now.DayOfWeek;

        public int Month => Now.Month;

        public int Day => Now.Day;

        public int Hour => Now.Hour;

        public int Minute => Now.Minute;

        public int Second => Now.Second;

        public int Millisecond => Now.Millisecond;
    }
}
