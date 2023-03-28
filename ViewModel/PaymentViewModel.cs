namespace UnifiedCheckout.ViewModel
{
    public class PaymentViewModel
    {
        public string authenticationType { get; set; }
        public string runEnvironment { get; set; }
        public string merchantID { get; set; }
        public string merchantKeyId { get; set; }
        public string merchantsecretKey { get; set; }
        public string keyAlias { get; set; }
        public string keyPass { get; set; }
        public string keyFileName { get; set; }
        public string keysDirectory { get; set; }
        public bool enableLog { get; set; }
        public string logFilename { get; set; }
        public string logDirectory { get; set; }
        public string logFileMaxSize { get; set; }
    }
}
