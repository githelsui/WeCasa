using System.Net;
using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class GenericUser : UserManager
    {   
        public GenericUser() : base() {}
        public GenericUser(AccountMariaDAO dao) {}

        public Result DeleteUser(UserAccount userAccount, UserStatus userStatus) {
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO()); 
            AuthResult authorizationResult = authService.ValidateClaim(userStatus.Claims, new Claim("Account", "Delete Account"));
            bool isAuthenticated = userStatus.IsAuth;
            bool isAuthorized = userStatus.IsEnabled && (Boolean) authorizationResult.ReturnedObject;
            Result deleteUserResult = new Result();
            
            if (isAuthenticated && isAuthorized) 
            {
                UserManager um = new UserManager();
                deleteUserResult = um.DeleteUser(userAccount);
                return deleteUserResult;
            }
            else {
                deleteUserResult.IsSuccessful = false;
                deleteUserResult.ErrorStatus = HttpStatusCode.Unauthorized;
                deleteUserResult.Message = "Account Deletion Unsuccessful";
                return deleteUserResult;
            }
        }
    }
}