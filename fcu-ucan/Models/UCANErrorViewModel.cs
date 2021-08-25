namespace fcu_ucan.Models
{
    /// <summary>
    /// UCAN 錯誤
    /// </summary>
    public class UCANErrorViewModel : ErrorViewModel
    {
        public int HttpCode { get; set; }

        public string Message { get; set; }
    }
}