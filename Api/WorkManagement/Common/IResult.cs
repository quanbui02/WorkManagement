namespace WorkManagement.Common
{
    public interface IResult<T>
    {
        bool Status { get; set; }

        T Data { get; set; }

        string Message { get; set; }

        int? TotalRecord { get; set; }
    }
}
