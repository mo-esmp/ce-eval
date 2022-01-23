using CeEval.Shared.Models;
using CeEval.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CeEval.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<ProductTopSoldDto> Products { get; set; }

        public async Task OnGet()
        {
            Products = await _mediator.Send(new ProductTopSoldListQuery(OrderStatus.InProgress, 5));
        }
    }
}