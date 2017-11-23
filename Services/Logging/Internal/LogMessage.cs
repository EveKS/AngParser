using System;

namespace AngParser.Services.Logging.Internal
{
  public class LogMessage
  {
    public DateTimeOffset Timestamp { get; set; }
    public string Message { get; set; }
  }
}
