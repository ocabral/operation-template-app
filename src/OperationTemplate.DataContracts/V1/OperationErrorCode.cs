namespace StoneCo.Buy4.OperationTemplate.DataContracts.V1
{
    public static class OperationErrorCode
    {
        public static string InternalError => "50";
        public static string RequestValidationError => "51";
        public static string UnauthorizedError => "52";
        public static string ServiceUnavailable => "53";
    }
}
