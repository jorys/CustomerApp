using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomerApp.RestApi.Common;

[Authorize]
public abstract class AuthenticatedControllerBase : InternalControllerBase
{
    protected AuthenticatedControllerBase(IOptions<ApiBehaviorOptions> apiBehaviorOptions) : base(apiBehaviorOptions)
    {
    }

    const string _customerIdClaimTypeEnd = "nameidentifier";

    Guid? _customerId;
    protected Guid CustomerId
    {
        get
        {
            _customerId ??= Guid.Parse(HttpContext.User.Claims.First(claim => claim.Type.EndsWith(_customerIdClaimTypeEnd)).Value);
            return _customerId.Value;
        }
    }
}
