using Catalog.API.Controllers;
using Catalog.API.Dtos;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random random = new();
    private Item CreateRandomItem()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = random.Next(1000),
            CreatedDate = DateTime.UtcNow
        };
    }
    // [Fact]
    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    // {
    //     // Arrange
    //     // Act
    //     // Assert
    // }

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                      .ReturnsAsync((Item?)null);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        // Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>(); // using FluentAssertions
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Arrange
        Item expectedItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                      .ReturnsAsync(expectedItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());
        // Assert
        // Assert.IsType<ItemDto>(result.Value);
        // var dto = (result as ActionResult<ItemDto>).Value;
        // Assert.Equal(expectedItem.Id, dto?.Id);
        // As Item is a Record Type, we need to compare first the name of the properties and then the values
        result.Value.Should().BeEquivalentTo(
            expectedItem,
            options => options.ComparingByMembers<Item>());
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        // Arrange
        var actualItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

        repositoryStub.Setup(repo => repo.GetItemsAsync())
                      .ReturnsAsync(actualItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(
            actualItems,
            options => options.ComparingByMembers<Item>());
    }

    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        // Arrange
        CreateItemDto itemToCreate = new(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            random.Next(1000)
        );

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.CreateItemAsync(itemToCreate);

        // Assert
        var createdItem = (result.Result as CreatedAtActionResult)?.Value as ItemDto;
        itemToCreate.Should().BeEquivalentTo(createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());
        createdItem?.Id.Should().NotBeEmpty();
        createdItem?.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, new TimeSpan(0, 0, 1));
    }

    [Fact]
    public async Task UpdateItemAsync_WithItemToUpdate_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                      .ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        UpdateItemDto itemToUpdate = new(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            existingItem.Price + 4
        );

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        // Assert
        result.Result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                      .ReturnsAsync(existingItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        // Assert
        result.Result.Should().BeOfType<NoContentResult>();
    }
}