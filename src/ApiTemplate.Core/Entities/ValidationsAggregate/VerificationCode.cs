using ApiTemplate.Core.Entities.ValidationsAggregate.Enums;
using ApiTemplate.SharedKernel;
using ApiTemplate.SharedKernel.Interfaces;

namespace ApiTemplate.Core.Entities.ValidationsAggregate;
public class VerificationCode : EntityBase, IAggregateRoot
{
  public VerificationCode(DateTime expirationDate, VerificationCodeStatus status, string sentTo)
  {
    ExpirationDate = expirationDate;
    Status = status;
    SentTo = sentTo;

    GenerateCode();
  }
  public string Code { get; set; } = string.Empty;
  public string SentTo { get; set; } = string.Empty;
  public DateTime ExpirationDate { get; set; }
  public int Attemps { get; set; }
  public VerificationCodeStatus Status { get; set; }
  public void GenerateCode()
  {
    Random _random = new Random();
    string code = _random.Next(100000, 999999).ToString();
    Code = code;
  }

}
