using ApiTemplate.Core.Abstracts;
using ApiTemplate.Core.Constants;
using ApiTemplate.Core.Entities.ValidationsAggregate;
using ApiTemplate.Core.Entities.ValidationsAggregate.Enums;
using ApiTemplate.Core.Entities.ValidationsAggregate.Specifications;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ValidationsEndpoints;

public class SendOtp : EndpointBaseAsync
      .WithRequest<SendOtpRequest>
      .WithActionResult<GeneralResponse>
{

  private readonly IRepository<VerificationCode> _repository;
  private readonly IOtpService _otpService;
  public SendOtp(
    IRepository<VerificationCode> repository,
    IOtpService otpService)
  {
    _repository = repository;
    _otpService = otpService;
  }

  [HttpPost("/api/Validation/Otp/Send")]
  [SwaggerOperation(
      Summary = "Otp send",
      Description = "Otp Code send",
      OperationId = "Validation.Send",
      Tags = new[] { "ValidationEndpoints" })
  ]
  public override async Task<ActionResult<GeneralResponse>> HandleAsync([FromBody] SendOtpRequest request,
      CancellationToken cancellationToken)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(request.SentTo))
        throw new Exception("The PhoneNumber or Email is required");

      var spec = new GetLastVerificationCodeSpec(request.SentTo);
      var verificationCodeLastSent = await _repository.GetBySpecAsync(spec, cancellationToken);

      if (verificationCodeLastSent is not null)
        _otpService.RunAllValidationsBeforeSendTheOtpCode(verificationCodeLastSent);

      var verificationCode = new VerificationCode(DateTime.Now.AddMinutes(OtpConstant.LimiteMinutes), VerificationCodeStatus.Sent, request.SentTo);

      await _otpService.SendOtpViaEmailOrPhoneNumber(request.SentTo, verificationCode.Code);

      var result = await _repository.AddAsync(verificationCode, cancellationToken);
      var response = new GeneralResponse(true, "Code Sended Successfully");

      return Ok(Result<GeneralResponse>.Success(response));
    }
    catch (Exception exception)
    {
      return BadRequest(Result<GeneralResponse>.Error(exception.Message));
    }
  }
}
