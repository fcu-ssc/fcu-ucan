namespace fcu_ucan.Models
{
    /// <summary>
    /// NID 錯誤
    /// </summary>
    public class NIDErrorViewModel : ErrorViewModel
    {
        public int HttpCode { get; set; }
                
        public string Message { get; set; }
    }
}