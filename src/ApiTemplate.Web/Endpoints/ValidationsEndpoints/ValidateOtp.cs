using ApiTemplate.Core.Abstracts;
using ApiTemplate.Core.Entities.ValidationsAggregate.Enums;
using ApiTemplate.Core.Entities.ValidationsAggregate.Specifications;
using ApiTemplate.Core.Entities.ValidationsAggregate;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;

namespace ApiTemplate.Web.Endpoints.ValidationsEndpoints;

public class ValidateOtp : EndpointBaseAsync
      .WithRequest<ValidateOtpRequest>
      .WithActionResult<GeneralResponse>
{

  private readonly IRepository<VerificationCode> _repository;
  private readonly IRepository<User> _userRepository;
  private readonly IOtpService _otpService;
  public ValidateOtp(
    IRepository<VerificationCode> repository,
    IRepository<User> userRepository,
    IOtpService otpService)
  {
    _repository = repository;
    _userRepository = userRepository;
    _otpService = otpService;
  }

  [HttpPost("/api/Validation/Otp/Validate")]
  [SwaggerOperation(
      Summary = "Otp validation",
      Description = "Validate an Otp",
      OperationId = "Validation.Validate",
      Tags = new[] { "ValidationEndpoints" })
  ]
  public override async Task<ActionResult<GeneralResponse>> HandleAsync([FromBody] ValidateOtpRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var spec = new GetLastVerificationCodeSpec(request.SentTo);
      var result = await _repository.GetBySpecAsync(spec, cancellationToken);

      if (result is null) throw new Exception("Verification code to validate was not found");

      if (result.Status == VerificationCodeStatus.Validated) throw new Exception("The code has already been validated");

      var validationCode = _otpService.ValidateCode(result, request.Code);
      await _repository.UpdateAsync(validationCode, cancellationToken);

      if(validationCode.Status == VerificationCodeStatus.Validated)
      {
        var user = await _userRepository.GetBySpecAsync(new GetUserByEmailSpec(request.SentTo), cancellationToken) 
          ?? throw new Exception("User was not found");

        user.UserAction!.IsEmailValidated = true;
        await _userRepository.UpdateAsync(user, cancellationToken);
      }

      var response = new GeneralResponse(
        validationCode.Status == VerificationCodeStatus.Validated,
        _otpService.GetMessageForMFAValidation(validationCode.Status));

      return Ok(Result<GeneralResponse>.Success(response));
    }
    catch (Exception exception)
    {
      return BadRequest(Result<GeneralResponse>.Error(exception.Message));
    }
  }
}
