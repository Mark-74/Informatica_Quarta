using System.Runtime.ExceptionServices;

namespace Esercizio_3
{
    internal class Program
    {
        struct DateTimePair
        {
            public DateTime first;
            public DateTime second;
            public TimeSpan elapsed = TimeSpan.MaxValue;
            public DateTimePair(DateTime first, DateTime second)
            {
                this.first = first;
                this.second = second;
            }
        }
        static void Main(string[] args)
        {
            DateTime[] dates = {
                new DateTime(2014, 6, 14, 6, 32, 0),
                new DateTime(2014, 7, 10, 23, 49, 0),
                new DateTime(2015, 1, 10, 1, 16, 0),
                new DateTime(2014, 12, 20, 21, 45, 0),
                new DateTime(2014, 6, 2, 15, 14, 0)
            };

            DateTimePair minTimeSpanWithDates = new DateTimePair(DateTime.MaxValue, DateTime.MinValue);

            for (int i = 0; i < dates.Length; i++)
            {
                for(int j = 0; j < dates.Length; j++)
                {
                    if (j == i) continue;

                    TimeSpan current = TimeSpan.FromSeconds(Math.Abs((dates[i] - dates[j]).TotalSeconds));

                    if (current < minTimeSpanWithDates.elapsed){
                        minTimeSpanWithDates.first = dates[i];
                        minTimeSpanWithDates.second = dates[j];
                        minTimeSpanWithDates.elapsed = current;
                    }
                }
            }

            Console.WriteLine($"Dates:\n{minTimeSpanWithDates.first}\n{minTimeSpanWithDates.second}\nDistance: {minTimeSpanWithDates.elapsed.Days} days, {minTimeSpanWithDates.elapsed.Hours} hours, {minTimeSpanWithDates.elapsed.Minutes} minutes, {minTimeSpanWithDates.elapsed.Seconds} seconds, {minTimeSpanWithDates.elapsed.Milliseconds} milliseconds");
        }
    }
}
