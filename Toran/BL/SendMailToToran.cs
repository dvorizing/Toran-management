using System;
using System.Threading.Tasks;
using Toran.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace Toran.BL
{
    public class SendMailToToran
    {
        private readonly ToranDutyCalculator _calculator;
        private readonly string _sendGridApiKey;
        private readonly string _fromEmail = "Shavkim01@ISRBANK.onmicrosoft.com";
        private readonly string _fromName = "מערכת תורנויות";

        public SendMailToToran(ToranDutyCalculator calculator, string sendGridApiKey)
        {
            _calculator = calculator;
            _sendGridApiKey = sendGridApiKey;
        }

        public async Task<string> SendMailToNextToran()
        {
            DateTime today = DateTime.Today;
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextFriday = today.AddDays(daysUntilFriday == 0 ? 7 : daysUntilFriday);

            ToranInfo toranResult = await _calculator.GetToranForDateAsync(nextFriday);
            string name = toranResult.Name;
            string email = toranResult.Email;

            string subject = "תורנות צוות שווקים";
            string htmlBody = $@"
                    <div dir='rtl' style='font-family: Segoe UI, sans-serif; font-size: 16px; line-height: 1.6;'>
                      <p>שלום וברכה, {name},</p>
                      <p>ביום שישי הקרוב, בתאריך {nextFriday:dd/MM/yyyy},</p>
                      <p>הנך משובץ/ת כתורן/נית בצוות שווקים.</p>
                      <p style=' font-weight: bold;'>אנחנו מעריכים את תרומתך לצוות ומאחלים בהצלחה!</p>
                      <p>בברכה,<br />מערכת ניהול התורנויות</p>
                    </div>
                    ";

            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(email, name);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlBody);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
                return $"המייל נשלח בהצלחה אל {name} ({email})";
            else
                return $"שגיאה בשליחת מייל: קוד סטטוס {response.StatusCode}";
        }
    }
}
