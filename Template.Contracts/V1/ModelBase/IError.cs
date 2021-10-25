namespace Template.Contracts.V1.ModelBase
{
    // public class ErrorResponse
    // {
    //     public ErrorResponse()
    //     {
    //         
    //     }
    //     public ErrorResponse(string message)
    //     {
    //         Error = message;
    //     }
    //     public string Error { get; set; }
    //     public static bool IsError(ErrorResponse error)
    //     {
    //         if (error == default)
    //             return false;
    //
    //         if (string.IsNullOrWhiteSpace(error.Error))
    //             return false;
    //
    //         return true;
    //     }
    //
    //     public static implicit operator ErrorResponse(string val)
    //     {
    //         return new()
    //         {
    //             Error = val
    //         };
    //     }
    //
    //     public override string ToString()
    //     {
    //         return Error;
    //     }
    // }
}