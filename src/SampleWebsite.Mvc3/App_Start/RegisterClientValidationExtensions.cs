using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SampleWebsite.Mvc3.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace SampleWebsite.Mvc3.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}