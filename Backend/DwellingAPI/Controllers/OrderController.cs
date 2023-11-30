using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DwellingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public OrderController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ResponseWrapper<OrderModel>>> GetOrder(string orderId)
        {
            return Ok(await _serviceRepo.OrderService.GetByIdAsync(orderId));
        }

        [HttpPut("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<OrderModel>>> UpdateOrder(string orderId, [FromBody] OrderModel model)
        {
            return Ok(await _serviceRepo.OrderService.UpdateAsync(orderId, model));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseWrapper<OrderModel>>> InsertOrder([FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<OrderModel>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _serviceRepo.OrderService.InsertAsync(model));
        }

        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<OrderModel>>> DeleteOrder(string orderId)
        {
            return Ok(await _serviceRepo.OrderService.DeleteAsync(orderId));
        }

        [HttpGet]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<OrderModel>>>> GetAllOrders()
        {
            return Ok(await _serviceRepo.OrderService.GetAllAsync());
        }

        [HttpGet("ByAccountId/{accountId}")]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<OrderModel>>>> GetAllOrdersByAccountId(string accountId)
        {
            return Ok(await _serviceRepo.OrderService.GetAllByAccountIdAsync(accountId));
        }
    }
}
