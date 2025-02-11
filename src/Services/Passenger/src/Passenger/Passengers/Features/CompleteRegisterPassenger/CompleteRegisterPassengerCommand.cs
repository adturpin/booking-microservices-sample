using BuildingBlocks.IdsGenerator;
using MediatR;
using Passenger.Passengers.Dtos;
using Passenger.Passengers.Models;

namespace Passenger.Passengers.Features.CompleteRegisterPassenger;

public record CompleteRegisterPassengerCommand(string PassportNumber, PassengerType PassengerType, int Age) : IRequest<PassengerResponseDto>
{
    public long Id { get; set; } = SnowFlakIdGenerator.NewId();
}
