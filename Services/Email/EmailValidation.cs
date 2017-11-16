using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Services.Email
{
  public class EmailValidation : IEmailValidation
  {
    bool IEmailValidation.IsValidEmail(string email)
    {
      if (this.IsImagePath(email)) return false;

      try
      {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
      }
      catch
      {
        return false;
      }
    }

    private bool IsImagePath(string email)
    {
      return email.EndsWith(".bmp") || email.EndsWith(".gif") || email.EndsWith(".png") ||
        email.EndsWith(".tiff") || email.EndsWith(".jpeg");
    }
  }
}
