namespace Matrip.Domain.Models.AccountModels
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string ResetPasswordToken { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
