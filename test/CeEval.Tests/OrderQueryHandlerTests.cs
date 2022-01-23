using Bogus;
using CeEval.Shared.Models;
using CeEval.Shared.Queries;
using CeEval.Shared.Repositories;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CeEval.Tests
{
    public class OrderQueryHandlerTests
    {
        [Fact]
        public async Task TopFiveSoldProductTest()
        {
            // Arrange
            var orders = GenerateOrders(5);
            var index = 4;
            foreach (var line in orders.Select(order => order.Lines.First()))
            {
                line.MerchantProductNo = index.ToString();
                line.Gtin = index.ToString();
                line.Description = index.ToString();

                index--;
                index = index == 0 ? 1 : index;
            }

            var orderRepository = Substitute.For<IOrderRepository>();
            orderRepository
                .GetOrdersWithLinesByStatusAsync(Arg.Any<OrderStatus>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orders.AsEnumerable()));

            var queryHandler = new OrderQueryHandler(orderRepository);

            // Act
            var topSold = await queryHandler.Handle(new ProductTopSoldListQuery(OrderStatus.InProgress, 5), CancellationToken.None);

            // Assert
            Assert.Equal(4, topSold.Count());
            Assert.Equal("1", topSold.First().Gtin);
            Assert.Equal(2, topSold.First().Quantity);
        }

        private List<Order> GenerateOrders(int count)
        {
            var id = 1;
            var productNo = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            var testOrderLine = new Faker<OrderLine>()
                .StrictMode(false)
                .RuleFor(p => p.Quantity, _ => 1)
                .RuleFor(p => p.Gtin, f => f.Commerce.Ean8())
                .RuleFor(p => p.MerchantProductNo, f => f.PickRandom(productNo))
                .RuleFor(p => p.Description, f => f.Commerce.ProductName());

            var testOrder = new Faker<Order>()
                .StrictMode(false)
                .RuleFor(p => p.Id, _ => id++)
                .RuleFor(p => p.ChannelId, f => f.Random.Int())
                .RuleFor(p => p.ChannelName, f => f.Commerce.Department())
                .RuleFor(p => p.OrderDate, f => f.Date.Past())
                .RuleFor(p => p.Status, _ => OrderStatus.InProgress)
                .RuleFor(p => p.Lines, f => testOrderLine.Generate(1));

            return testOrder.Generate(count);
        }
    }
}