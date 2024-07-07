using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiTemplate.Infrastructure.Dto;
public abstract class BaseResponseDto
{
  [JsonProperty(Order = -2)]
  public Guid Id { get; set; }
}
