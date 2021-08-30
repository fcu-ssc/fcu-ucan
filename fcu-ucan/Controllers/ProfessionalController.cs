using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [Route("intro/professional")]
    public class ProfessionalController : Controller
    {
        private readonly ILogger<ProfessionalController> _logger;

        public ProfessionalController(ILogger<ProfessionalController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index() => View();
        
        /// <summary>
        /// 建築營造
        /// </summary>
        [HttpGet("construction")]
        public IActionResult Construction() => View();

        /// <summary>
        /// 製造
        /// </summary>
        [HttpGet("manufacturing")]
        public IActionResult Manufacturing() => View();

        /// <summary>
        /// 科學、技術、工程、數學
        /// </summary>
        [HttpGet("professional-scientific-and-technical-activities")]
        public IActionResult ProfessionalScientificAndTechnicalActivities() => View();

        /// <summary>
        /// 物流運輸
        /// </summary>
        [HttpGet("transportation-and-storage")]
        public IActionResult TransportationAndStorage() => View();

        /// <summary>
        /// 天然資源、食品與農業
        /// </summary>
        [HttpGet("natural")]
        public IActionResult Natural() => View();

        /// <summary>
        /// 醫療保健
        /// </summary>
        [HttpGet("human-health")]
        public IActionResult HumanHealth() => View();

        /// <summary>
        /// 藝文與影音傳播
        /// </summary>
        [HttpGet("arts-entertainment-and-recreation")]
        public IActionResult ArtsEntertainmentAndRecreation() => View();
        
        /// <summary>
        /// 資訊科技
        /// </summary>
        [HttpGet("information-and-communication")]
        public IActionResult InformationAndCommunication() => View();

        /// <summary>
        /// 金融財務
        /// </summary>
        [HttpGet("financial-and-insurance-activities-and-real-estate-activities")]
        public IActionResult FinancialAndInsuranceActivitiesAndRealEstateActivities() => View();

        /// <summary>
        /// 企業經營管理
        /// </summary>
        [HttpGet("business-management")]
        public IActionResult BusinessManagement() => View();
        
        /// <summary>
        /// 行銷與銷售
        /// </summary>
        [HttpGet("wholesale-and-retail-trade")]
        public IActionResult WholesaleAndRetailTrade() => View();

        /// <summary>
        /// 政府公共事務
        /// </summary>
        [HttpGet("public-administration-and-defence")]
        public IActionResult PublicAdministrationAndDefence() => View();

        /// <summary>
        /// 教育與訓練
        /// </summary>
        [HttpGet("education")]
        public IActionResult Education() => View();

        /// <summary>
        /// 個人及社會服務
        /// </summary>
        [HttpGet("social-work-and-other-service-activities")]
        public IActionResult SocialWorkAndOtherServiceActivities() => View();

        /// <summary>
        /// 休閒與觀光旅遊
        /// </summary>
        [HttpGet("accommodation-and-food-service-activities")]
        public IActionResult AccommodationAndFoodServiceActivities() => View();
        
        /// <summary>
        /// 司法、法律與公共安全
        /// </summary>
        [HttpGet("compulsory-social-security")]
        public IActionResult CompulsorySocialSecurity() => View();
    }
}