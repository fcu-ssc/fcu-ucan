namespace fcu_ucan.Models
{
    public class NIDErrorViewModel : ErrorViewModel
    {
        public int HttpCode { get; set; }
                
        public string Message { get; set; }
    }
}