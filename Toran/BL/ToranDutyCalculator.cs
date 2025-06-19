using Toran.Models;
using Microsoft.EntityFrameworkCore;

namespace Toran.BL
{
    public class ToranDutyCalculator
    {
        private readonly BoiappContext _context;

        public ToranDutyCalculator(BoiappContext context)
        {
            _context = context;
        }

        private async Task<(List<ToranInfo> fullRotation, DateTime lastKnownDate, int lastIndex)> BuildRotationAsync(DateTime referenceDate)
        {
            var toranim = await _context.Torans
                .Select(t => new ToranInfo { Name = t.Name, Email = t.Email })
                .ToListAsync();

            var statuses = await _context.ToranStatuses
                .Where(s => s.LastDutyDate.HasValue && s.LastDutyDate <= referenceDate)
                .ToListAsync();

            var assigned = toranim
                .Where(t => statuses.Any(s => s.EmployeeName == t.Name))
                .Select(t => new
                {
                    Info = t,
                    LastDate = statuses
                        .Where(s => s.EmployeeName == t.Name)
                        .Max(s => s.LastDutyDate!.Value)
                })
                .OrderBy(x => x.LastDate)
                .Select(x => x.Info)
                .ToList();

            var neverAssigned = toranim
                .Where(t => !assigned.Any(a => a.Name == t.Name))
                .OrderBy(t => t.Name)
                .ToList();

            var fullRotation = neverAssigned.Concat(assigned).ToList();

            var lastDutyInHistory = statuses
                .OrderByDescending(s => s.LastDutyDate)
                .FirstOrDefault();

            DateTime lastKnownDate = lastDutyInHistory?.LastDutyDate?.Date ?? referenceDate;
            int lastIndex = lastDutyInHistory != null
                ? fullRotation.FindIndex(t => t.Name == lastDutyInHistory.EmployeeName)
                : -1;

            return (fullRotation, lastKnownDate, lastIndex);
        }

        private ToranInfo PredictToranForDate(DateTime date, List<ToranInfo> rotation, DateTime lastKnownDate, int lastIndex)
        {
            int weeksSince = (int)((date - lastKnownDate).TotalDays / 7);
            int index = (lastIndex + weeksSince) % rotation.Count;
            if (index < 0) index += rotation.Count;
            return rotation[index];
        }

        public async Task<dynamic> GetToranForDateAsync(DateTime date)
        {
            date = date.Date;

            if (date.DayOfWeek != DayOfWeek.Friday)
                return $"תאריך {date:dd/MM/yyyy} אינו יום שישי. תורנות מתקיימת רק בימי שישי.";

            var status = await _context.ToranStatuses
                .Where(s => s.LastDutyDate.HasValue && s.LastDutyDate.Value.Date == date)
                .FirstOrDefaultAsync();

            if (status != null)
                return status.EmployeeName;

            if (date < DateTime.Today)
                return $":אין תיעוד במערכת לתאריך {date:dd/MM/yyyy}";

            var (rotation, lastKnownDate, lastIndex) = await BuildRotationAsync(date);
            ToranInfo toran = PredictToranForDate(date, rotation, lastKnownDate, lastIndex);

            return toran;
        }

        public async Task<List<ToransRow>> GetToranDatesForRangeAsync(DateTime fromDate, DateTime toDate, string? name = null)
        {
            var toranim = await _context.Torans
                .Select(t => new ToranInfo { Name = t.Name, Email = t.Email })
                .ToListAsync();

            var statuses = await _context.ToranStatuses
                .Where(s => s.LastDutyDate.HasValue && s.LastDutyDate <= toDate)
                .ToListAsync();

            var actualDates = statuses
                .Where(s => s.LastDutyDate >= fromDate)
                .Select(s => new ToransRow { Date = s.LastDutyDate!.Value.Date, Name = s.EmployeeName })
                .ToList();

            DateTime firstFriday = fromDate.AddDays((DayOfWeek.Friday - fromDate.DayOfWeek + 7) % 7);
            var allFridays = new List<DateTime>();
            for (DateTime dt = firstFriday; dt <= toDate; dt = dt.AddDays(7))
                allFridays.Add(dt);

            var (rotation, lastKnownDate, lastIndex) = await BuildRotationAsync(toDate);

            var predicted = new List<ToransRow>();
            foreach (var date in allFridays)
            {
                if (actualDates.Any(x => x.Date == date))
                    continue;

                ToranInfo toran = PredictToranForDate(date, rotation, lastKnownDate, lastIndex);
                predicted.Add(new ToransRow { Date = date, Name = toran.Name });
            }

            return actualDates
                .Concat(predicted)
                .Where(x => name == null || x.Name == name)
                .OrderBy(x => x.Date)
                .ToList();
        }
    }
}
