using Ardalis.Specification;

namespace ApiTemplate.Core.Entities.ValidationsAggregate.Specifications;
public class GetLastVerificationCodeSpec : Specification<VerificationCode>, ISingleResultSpecification
{
  public GetLastVerificationCodeSpec(string sentTo)
  {
    Query.Where(x => x.SentTo == sentTo).OrderByDescending(x => x.CreatedDate).Take(1);
  }
}
