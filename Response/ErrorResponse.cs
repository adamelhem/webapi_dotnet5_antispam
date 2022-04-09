namespace IsPrimeNumber.Response
{
    public class ErrorResponse
    {
        public string HasError => "true";
        public string ErrorMessage { get; set; }
    }
}