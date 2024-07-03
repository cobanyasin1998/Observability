using Common.Shared.DTOs;
using Common.Shared.DTOs.Order;
using Common.Shared.DTOs.Stock;
using Observability.Shared;
using Order.API.Models;
using Order.API.StockServices;
using System.Diagnostics;

namespace Order.API.OrderServices
{
    public class OrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly StockService _stockService;

        public OrderService(AppDbContext appDbContext, StockService stockService)
        {
            _appDbContext = appDbContext;
            _stockService = stockService;
        }

        public async Task<ResponseDto<OrderCreateResponseDto>> CreateAsync(OrderCreateRequestDto orderCreateRequestDto)
        {
            Activity.Current?.SetTag("Asp.NetCore Tag1", "Tag1 value");
            using var activity = ActivitySourceProvider.Source.StartActivity();

            activity?.AddEvent(new("OrderService.CreateAsync started"));

            var newOrder = new Order
            {
                OrderCode = new Guid().ToString(),
                Created = DateTime.UtcNow,
                UserId = new Random().Next(0, 500),
                Status = EOrderStatus.Success,
                Items = orderCreateRequestDto.Items.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };
            _appDbContext.Orders.Add(newOrder);
            int result = await _appDbContext.SaveChangesAsync();
            activity?.SetTag("order User Id: ", newOrder.UserId);

            var res= await _stockService.CheckAndPaymentProcess(new StockCheckAndPaymentProcessRequestDto
            {
                OrderCode = newOrder.OrderCode,
                Items = newOrder.Items.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            });
            if (!res.isSuccess)
            {
                return ResponseDto<OrderCreateResponseDto>.Fail(400, res.failMessage);
            }
            else
            {
                newOrder.Status = EOrderStatus.Success;
                await _appDbContext.SaveChangesAsync();
                activity?.AddEvent(new("OrderService.CreateAsync finished"));
                return ResponseDto<OrderCreateResponseDto>.Success(200, new OrderCreateResponseDto { Id = newOrder.Id });

            }


        }
    }
}
