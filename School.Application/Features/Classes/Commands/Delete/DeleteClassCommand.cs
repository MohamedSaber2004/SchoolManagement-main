using MediatR;

namespace School.Application.Features.Classes.Commands.Delete
{
    public record DeleteClassCommand(int Id): IRequest<bool>;
}
