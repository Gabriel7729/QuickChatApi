using ApiTemplate.Core.Entities.ValidationsAggregate;
using ApiTemplate.Core.Entities.ValidationsAggregate.Enums;

namespace ApiTemplate.Core.Interfaces;
public interface IOtpService
{
  VerificationCode ValidateCode(VerificationCode lastCodeByUser, string code);
  Task SendOtpViaEmailOrPhoneNumber(string sentTo, string verificationCode);
  string GetMessageForMFAValidation(VerificationCodeStatus status);
  void RunAllValidationsBeforeSendTheOtpCode(VerificationCode verificationCode);
}
