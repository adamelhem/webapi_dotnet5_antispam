namespace IsPrimeNumber.Response
{
    public class SuccessResponse
    {
        public string HasError => "false";
        public int Number { get; set; }
        public bool IsPrime { get; set; }
    }
}