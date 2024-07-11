using System.Text.RegularExpressions;
using ApiTemplate.Core.Constants;
using ApiTemplate.Core.Entities.ValidationsAggregate;
using ApiTemplate.Core.Entities.ValidationsAggregate.Enums;
using ApiTemplate.Core.Extensions;
using ApiTemplate.Core.Interfaces;

namespace ApiTemplate.Core.Services;
public class OtpService : IOtpService
{
  private readonly IEmailSender _emailSenderService;

  public OtpService(
    IEmailSender emailSenderService)
  {
    _emailSenderService = emailSenderService;
  }

  public VerificationCode ValidateCode(VerificationCode lastCodeByUser, string code)
  {
    if (!ValidateAttemps(lastCodeByUser.Attemps))
    {
      lastCodeByUser.Status = VerificationCodeStatus.Failed;
      return lastCodeByUser;
    }

    if (ValidateExpirationDate(lastCodeByUser.ExpirationDate))
    {
      lastCodeByUser.Status = VerificationCodeStatus.Expired;
      return lastCodeByUser;
    }

    if (lastCodeByUser.Code != code)
    {
      lastCodeByUser.Attemps++;
      return lastCodeByUser;
    }

    lastCodeByUser.Status = VerificationCodeStatus.Validated;
    return lastCodeByUser;
  }
  public async Task SendOtpViaEmailOrPhoneNumber(string sentTo, string verificationCode)
  {
    string emailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    string phoneRegex = @"^8(29|49|09)\d{7}$";

    ContactMethod contactMethod = Regex.IsMatch(sentTo, emailRegex) ? ContactMethod.Email
      : Regex.IsMatch(sentTo, phoneRegex) ? ContactMethod.PhoneNumber : ContactMethod.Unknown;

    switch (contactMethod)
    {
      case ContactMethod.Email:
        string htmlContent = EmailOtpTemplateToSend(verificationCode);
        await _emailSenderService.SendEmailAsync(new List<string>() { sentTo }, "Otp Verification Code", htmlContent);
        break;

      case ContactMethod.PhoneNumber:
        //string isSmsVerificationCodeSended = _twilioService.SendMessage(verificationCode, sentTo);
        string? isSmsVerificationCodeSended = null;
        if (isSmsVerificationCodeSended == null) throw new Exception("Error sending sms");
        break;

      default:
        throw new Exception("The email or phone number is invalid");
    }
  }
  public string GetMessageForMFAValidation(VerificationCodeStatus status)
  {
    return status switch
    {
      VerificationCodeStatus.Sent => "Code Sent Successfully",
      VerificationCodeStatus.Validated => "Code Validated Successfully",
      VerificationCodeStatus.Expired => "Code Expired",
      VerificationCodeStatus.Failed => "Code Invalid",
      _ => "",
    };
  }
  public void RunAllValidationsBeforeSendTheOtpCode(VerificationCode verificationCodeLastSent)
  {
    if (verificationCodeLastSent.Status == VerificationCodeStatus.Sent)
    {
      var timeSpan = DateTime.UtcNow - verificationCodeLastSent.CreatedDate;
      if (timeSpan.TotalSeconds < 20)
        throw new Exception("Please wait 20 seconds before requesting a new code");
    }
  }

  private static string EmailOtpTemplateToSend(string verificationCode)
  {
    string template = FileManagment.ReadEmailTemplate(RouteConstans.EmailTemplateRoute, TemplateConstants.OtpTemplate);

    string htmlContent = template.Replace("{{OTP_CODE}}", verificationCode);
    return htmlContent;
  }
  private static bool ValidateAttemps(int attemps)
  {
    return attemps != OtpConstant.MaxAttemps;
  }
  private static bool ValidateExpirationDate(DateTime expirationDate)
  {
    return expirationDate < DateTime.Now;
  }
}
