using DiscountgRPC.Protos;

namespace BasketAPI.gRPCServices
{
    public class DiscountgRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;

        public DiscountgRPCService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _client = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _client.GetDiscountAsync(discountRequest);
        }

    }
}
