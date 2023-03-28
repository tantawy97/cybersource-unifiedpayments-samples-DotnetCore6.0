using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace UnifiedCheckout
{
    public class CaptureContext
    {
        private readonly string requestUri = "https://apitest.cybersource.com/up/v1/capture-contexts";
        private readonly string merchantId = "testrest";
        private readonly string merchantKeyId = "08c94330-f618-42a3-b09d-e1e43be5efda";
        private readonly string merchantSecretKey = "yBJxy6LjM2TmcPGu+GaJrHtkke25fPpUX+UY6/L/1tE=";
        public string ObjectContext()
        {
            var captureContext = new
            {
                targetOrigins = new[] { "https://localhost:7241" },
                clientVersion = "0.8",
                allowedCardNetworks = new[] { "VISA", "MASTERCARD", "AMEX" },
                allowedPaymentTypes = new[] { "PANENTRY", "SRC" },
                country = "US",
                locale = "en_US",
                captureMandate = new
                {
                    billingType = "FULL",
                    requestEmail = true,
                    requestPhone = true,
                    requestShipping = true,
                    shipToCountries = new[] { "US", "GB" },
                    showAcceptedNetworkIcons = true
                },
                orderInformation = new
                {
                    amountDetails = new
                    {
                        totalAmount = "21.00",
                        currency = "USD"
                    },
                    billTo = new
                    {
                        address1 = "277 Park Avenue",
                        address2 = "50th Floor",
                        address3 = "Desk NY-50110",
                        address4 = "address4",
                        administrativeArea = "NY",
                        buildingNumber = "buildingNumber",
                        country = "US",
                        district = "district",
                        locality = "New York",
                        postalCode = "10172",
                        company = new
                        {
                            name = "Visa Inc",
                            address1 = "900 Metro Center Blvd",
                            address2 = "address2",
                            address3 = "address3",
                            address4 = "address4",
                            administrativeArea = "CA",
                            buildingNumber = "1",
                            country = "US",
                            district = "district",
                            locality = "Foster City",
                            postalCode = "94404"
                        },
                        email = "john.doe@visa.com",
                        firstName = "John",
                        lastName = "Doe",
                        middleName = "F",
                        nameSuffix = "Jr",
                        title = "Mr",
                        phoneNumber = "1234567890",
                        phoneType = "phoneType"
                    },
                    shipTo = new
                    {
                        address1 = "CyberSourcetest",
                        address2 = "Victoria House",
                        address3 = "15-17 Gloucester Street",
                        address4 = "string",
                        administrativeArea = "CA",
                        buildingNumber = "string",
                        country = "GB",
                        district = "string",
                        locality = "Belfast",
                        postalCode = "BT1 4LS",
                        firstName = "Joe",
                        lastName = "Soap"
                    }
                }
            };

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(captureContext);
            return payload;
        }

        public async Task<string> RequestCaptureContext()
        {
            var payload = ObjectContext();
            var digest = GenerateDigest(payload);

            var dateHeader = DateTime.UtcNow.ToString("r");
            var requestTarget = $"post {new Uri(requestUri).AbsolutePath}";

            var signatureString = $"(request-target): {requestTarget}\n";
            signatureString += $"date: {dateHeader}\n";
            signatureString += $"digest: SHA-256={digest}\n";
            signatureString += $"v-c-merchant-id: {merchantId}";

            var signature = GenerateSignature(signatureString, merchantSecretKey);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Date", dateHeader);
                client.DefaultRequestHeaders.Add("Digest", $"SHA-256={digest}");
                client.DefaultRequestHeaders.Add("v-c-merchant-id", merchantId);
                client.DefaultRequestHeaders.Add("Signature", $"keyid=\"{merchantKeyId}\", algorithm=\"HmacSHA256\", headers=\"(request-target) date digest v-c-merchant-id\", signature=\"{signature}\"");

                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                try
                {
                    var response =await client.PostAsync(requestUri, content);

                    var data =await response.Content.ReadAsStringAsync();
                    return data;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    static string GenerateDigest(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        static string GenerateSignature(string data, string secretKey)
        {
            var keyBytes = Convert.FromBase64String(secretKey);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
