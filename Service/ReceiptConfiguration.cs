namespace UnifiedCheckout.Service
{
    public class ReceiptConfiguration
    {
        public static Dictionary<string, string> ReceiptData()
        {
            string AuthenticationType = "http_signature";
            string RunEnvironment = "apitest.cybersource.com";
            string MerchantId = "testrest";

            // http_signature parameters
            string merchantKeyId = "08c94330-f618-42a3-b09d-e1e43be5efda";
            string merchantSecretKey = "yBJxy6LjM2TmcPGu+GaJrHtkke25fPpUX+UY6/L/1tE=";
            // jwt parameters
            string KeysDirectory = "Resource";
            string KeyFileName = "testrest";
            string KeyAlias = "testrest";
            string KeyPass = "testrest";

            // logging parameters
            bool EnableLog = true;
            string LogFileName = "cybs";
            string LogDirectory = "../log";
            string LogfileMaxSize = "5242880"; //10 MB In Bytes

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"authenticationType" ,AuthenticationType },
                {"runEnvironment" , RunEnvironment },

               {"merchantID" , MerchantId},
               {"merchantKeyId" , merchantKeyId},
               {"merchantsecretKey" , merchantSecretKey},

               {"keyAlias" , KeyAlias},
               {"keyPass" , KeyPass},
               {"keyFileName" , KeyFileName},
               {"keysDirectory" , KeysDirectory},

               {"enableLog" , EnableLog.ToString()},
               {"logFilename" , LogFileName},
               {"logDirectory" , LogDirectory},
               { "logFileMaxSize" , LogfileMaxSize}
            };
        return parameters;
        }
    }
}
