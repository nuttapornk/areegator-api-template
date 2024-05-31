using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetCountries.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.UnitTests.Process.Weather.Queries;

public class GetCountriesHandlerTest 
{
    public GetCountriesHandlerTest()
    {
                
    }

    [Fact]
    public async Task Handle_ReturnsListOfCountries()
    {
        //Arrange
        Mock<IWeatherRepository> mockRepository = new();
        List<Country> countries =
        [
            new() { Id = Guid.NewGuid(), Name = "ThaiLand" },
            new() { Id = Guid.NewGuid(), Name = "Japan" },
            new() { Id = Guid.NewGuid(), Name = "England" }
        ];

        mockRepository.Setup(r => r.GetCountriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(countries);

        var handler = new GetCountriesHandler(mockRepository.Object);
        var request = new GetCountriesQuery();

        //Act
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(3,result.Count());
        Assert.Equal("ThaiLand", result.ElementAt(0).Name);
        Assert.Equal("Japan", result.ElementAt(1).Name);
        Assert.Equal("England", result.ElementAt(2).Name);

    }

    [Fact]
    public async Task Handle_ReturnsEmptyOfCountries()
    {
        //Arrange
        Mock<IWeatherRepository> mockRepository = new();

        mockRepository.Setup(r => r.GetCountriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Country>());

        var handler = new GetCountriesHandler(mockRepository.Object);
        var request = new GetCountriesQuery();

        //Act
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Empty(result);

    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        Mock<IWeatherRepository> mockRepository = new();
        mockRepository.Setup(r => r.GetCountriesAsync(It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new Exception("An error occurred while fetching countries."));

        var handler = new GetCountriesHandler(mockRepository.Object);
        var request = new GetCountriesQuery();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
    }
}
