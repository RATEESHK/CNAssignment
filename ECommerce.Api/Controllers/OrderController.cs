using ECommerce.Api.Models;
using ECommerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpPost]
        public async Task<IActionResult> GetRecentOrderDetails([FromBody] CustomerRequest request)
        {
            try
            {
                var orderDetails = await _orderService.GetMostRecentOrderAsync(request.User, request.CustomerId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching order details");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching order details");
            }
        }
    }
}
