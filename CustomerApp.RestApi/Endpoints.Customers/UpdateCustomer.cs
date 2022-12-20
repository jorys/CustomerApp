// TODO: uncomment and implement

//using CustomerApp.Application.Handlers.Customers;
//using CustomerApp.Application.Handlers.Customers.Models;
//using CustomerApp.RestApi.Common;
//using CustomerApp.RestApi.Endpoints.Customers.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Net;

//namespace CustomerApp.RestApi.Endpoints.Customers;

//[ApiController]
//[Route("api/customer")]
//public class UpdateCustomer : AuthenticatedControllerBase
//{
//    readonly UpdateCustomerHandler _commandHandler;
//    readonly ErrorsHandler _errorsHandler;

//    public UpdateCustomer(UpdateCustomerHandler commandHandler, ErrorsHandler errorsHandler)
//    {
//        _commandHandler = commandHandler;
//        _errorsHandler = errorsHandler;
//    }

//    [HttpPut]
//    [Tags("CustomerInformation")]
//    [SwaggerResponse((int)HttpStatusCode.OK, type: typeof(CustomerResponse))]
//    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
//    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
//    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
//    public async Task<ActionResult<CustomerResponse>> Handle(UpdateCustomerRequest resquest)
//    {
//        var command = new UpdateCustomerCommand(request.ToCommand(CustomerId));
//        var errorOrCustomer = await _commandHandler.Handle(command);

//        return errorOrCustomer.Match(
//            customer => Ok(CustomerResponse.From(customer)),
//            errors => _errorsHandler.ToObjectResult(errors, HttpContext));
//    }
//}