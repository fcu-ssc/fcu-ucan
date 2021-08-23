namespace fcu_ucan.Models
{
    public class UCANErrorViewModel : ErrorViewModel
    {
        public int HttpCode { get; set; }

        public string Message { get; set; }
    }
}