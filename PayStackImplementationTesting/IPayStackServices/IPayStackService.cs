using PayStackImplementationTesting.Dtos.RequestModel;
using PayStackImplementationTesting.Dtos.ResponseModel;

namespace PayStackImplementationTesting.IPayStackServices
{
    public interface IPayStackService
    {
        Task<BaseResponse<InitializePaymentResponseDto>> InitializePaymentAsync(InitializePaymentRequestDto requestDto);
        Task<BaseResponse<VerifyPaymentRequestDto>> VerifyPaymentAsync(string reference);
    }
}
