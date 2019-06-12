using Serilog.Core;
using Serilog.Events;
using System;
using System.Linq;

namespace WebApi.Configurations
{
    public class TransactionIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }
            var context = HttpContext.Current;

            var userId = logEvent.Properties.ContainsKey("UserId") 
                ? logEvent.Properties["UserId"].ToString() 
                : context?.User.Claims?.First(c => c.Type == "UserId")?.Value;

            var userIdProperty = new LogEventProperty("UserId", new ScalarValue(userId));
            logEvent.AddPropertyIfAbsent(userIdProperty);

            var transactionId = logEvent.Properties.ContainsKey("TransactionId") 
                ? logEvent.Properties["TransactionId"].ToString() 
                : context?.User.Claims?.First(c => c.Type == "TransactionId")?.Value;

            var transactionIdProperty = new LogEventProperty("TransactionId", new ScalarValue(transactionId));
            logEvent.AddPropertyIfAbsent(transactionIdProperty);
        }
    }
}
