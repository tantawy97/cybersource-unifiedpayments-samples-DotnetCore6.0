
using CyberSource.Api;
using CyberSource.Client;
using CyberSource.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UnifiedCheckout.Service;
using UnifiedCheckout.ViewModel;

namespace UnifiedCheckout.Controllers
{
    public class PaymentController : Controller
    {
        public async Task<IActionResult> Checkout()
        {
            try
            
            {
                var cc = new CaptureContext();
                var capturecontext = await cc.RequestCaptureContext();
                return View("checkout", capturecontext);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        public IActionResult Token()
        {
            try
            {
                var transientToken = Request.Form["transientToken"].ToString();

                return View("token", transientToken);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        public async Task<IActionResult> Receipt()
        {
            var tokenResponse = Request.Form["transientToken"];
            var parameters = ReceiptConfiguration.ReceiptData();
            var config = new Configuration(merchConfigDictObj: parameters);

            try
            {
                var instance = new PaymentsApi(config);

                var clientReferenceInformation = new Ptsv2paymentsClientReferenceInformation
                {
                    Code = "test_flex_payment"
                };

                var processingInformation = new Ptsv2paymentsProcessingInformation
                {
                    CommerceIndicator = "internet"
                };

                var amountDetails = new Ptsv2paymentsOrderInformationAmountDetails
                {
                    TotalAmount = "102.21",
                    Currency = "USD"
                };

                var billTo = new Ptsv2paymentsOrderInformationBillTo
                {
                    Country = "US",
                    FirstName = "John",
                    LastName = "Deo",
                    PhoneNumber = "4158880000",
                    Address1 = "test",
                    PostalCode = "94105",
                    Locality = "San Francisco",
                    AdministrativeArea = "MI",
                    Email = "test@cybs.com",
                    Address2 = "Address 2",
                    District = "MI",
                    BuildingNumber = "123"
                };

                var orderInformation = new Ptsv2paymentsOrderInformation
                {
                    AmountDetails = amountDetails,
                    BillTo = billTo
                };

                // EVERYTHING ABOVE IS JUST NORMAL PAYMENT INFORMATION
                // THIS IS WHERE YOU PLUG IN THE MICROFORM TRANSIENT TOKEN
                var tokenInformation = new Ptsv2paymentsTokenInformation
                {
                    TransientTokenJwt = tokenResponse
                };

                var request = new CreatePaymentRequest
                {
                    ClientReferenceInformation = clientReferenceInformation,
                    ProcessingInformation = processingInformation,
                    OrderInformation = orderInformation,
                    TokenInformation = tokenInformation
                };

                //Console.WriteLine("\n****** Process Payment ******** ");

                var result = await instance.CreatePaymentAsync(request);


                return View("receipt", new
                {
                    paymentResponse = JsonConvert.SerializeObject(result)
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
      
    }
}

