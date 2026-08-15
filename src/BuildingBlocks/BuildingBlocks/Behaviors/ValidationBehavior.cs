using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<IRequest, TResponse>
    (IEnumerable<IValidator<IRequest>> validators)
    : IPipelineBehavior<IRequest, TResponse>
    where IRequest:ICommand<TResponse>
{
    public async Task<TResponse> Handle(IRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<IRequest>(request);
        var validationResults = 
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failuers= validationResults
                   .Where(r=>r.Errors.Any())
                   .SelectMany(r=>r.Errors)
                   .ToList();
        if (failuers.Any())
        {
            throw new ValidationException(failuers);
        }
        return await next();
    }
}
