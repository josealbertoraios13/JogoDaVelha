using model.requests;
using model.responses;

namespace model.controller;

public record WebSocketRouteModel
{
    public Type? request {get; init;} 
    public Type? response {get; init;} 
    public Func<IRequest, IResponse>? func {get; init;}
    public WebSocketRouteModel(Type request, Type response, Func<IRequest, IResponse> func)
    {
        this.request = request;
        this.response = response;
        this.func = func;
    }
}