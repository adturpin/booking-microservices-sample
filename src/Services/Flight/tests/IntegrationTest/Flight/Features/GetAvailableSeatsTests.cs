﻿using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Contracts.Grpc;
using Flight.Flights.Features.GetAvailableFlights;
using FluentAssertions;
using Grpc.Net.Client;
using Integration.Test.Fakes;
using MagicOnion.Client;
using Xunit;

namespace Integration.Test.Flight.Features;

[Collection(nameof(TestFixture))]
public class GetAvailableSeatsTests
{
    private readonly TestFixture _fixture;
    private readonly GrpcChannel _channel;

    public GetAvailableSeatsTests(TestFixture fixture)
    {
        _fixture = fixture;
        _channel = fixture.Channel;
    }

    [Fact]
    public async Task should_return_available_seats_from_grpc_service()
    {
        // Arrange
        var flightCommand = new FakeCreateFlightCommand().Generate();
        var flightEntity = FakeFlightCreated.Generate(flightCommand);

        await _fixture.InsertAsync(flightEntity);

        var seatCommand1 = new FakeCreateSeatCommand(flightEntity.Id).Generate();
        var seatCommand2 = new FakeCreateSeatCommand(flightEntity.Id).Generate();
        var seatEntity1 = FakeSeatCreated.Generate(seatCommand1);
        var seatEntity2 = FakeSeatCreated.Generate(seatCommand2);

        await _fixture.InsertAsync(seatEntity1, seatEntity2);

        var flightGrpcClient = MagicOnionClient.Create<IFlightGrpcService>(_channel);

        // Act
        var response = await flightGrpcClient.GetAvailableSeats(flightEntity.Id);

        // Assert
        response?.Should().NotBeNull();
        response?.Count().Should().BeGreaterOrEqualTo(2);
    }
}
