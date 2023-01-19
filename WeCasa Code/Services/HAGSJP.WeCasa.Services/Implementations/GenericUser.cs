using HAGSJP.WeCasa.Models;
using HAGSJP.WeCasa.Models.Security;
using HAGSJP.WeCasa.sqlDataAccess;

namespace HAGSJP.WeCasa.Services.Implementations
{
    public class GenericUser : UserManager
    {   
        public GenericUser() : base() {}
        public GenericUser(AccountMariaDAO dao) {}

        public new Result DeleteUser(UserAccount userAccount) {
            AuthorizationService authService = new AuthorizationService(new AuthorizationDAO());
            AccountMariaDAO mariaDAO = new AccountMariaDAO();
            var authenticationResult = mariaDAO.ValidateUserInfo(userAccount);
            AuthResult authorizationResult = authService.ValidateClaim(userAccount, new Claim("Account", "Delete Account"));
            bool isAuthenticated = authenticationResult.IsAuth;
            bool isAuthorized = authorizationResult.ReturnedObject != null? (bool)authorizationResult.ReturnedObject : false;
            Result deleteUserResult = new Result();
            if (isAuthenticated && isAuthorized) 
            {
                UserManager um = new UserManager();
                deleteUserResult = um.DeleteUser(userAccount);
                return deleteUserResult;
            }
            else {
                deleteUserResult.IsSuccessful = false;
                deleteUserResult.Message = "Account Deletion Unsuccessful";
                return deleteUserResult;
            }
        }
    }
}