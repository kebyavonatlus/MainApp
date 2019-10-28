using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using MainApp.Models;
using System.Data.Entity;
using System.Text;
using MainApp.Models.UserModel;

namespace MainApp.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            using (ConnectionContext db = new ConnectionContext())
            {
                var uRole = from user in db.Users
                    join userRole in db.UserRoles on user.UserId equals userRole.UserId
                    join role in db.Roles on userRole.RoleId equals role.RoleId
                    where user.Login == username
                    select role.RoleName;
                return uRole.ToArray();
            }
        }
        public override void CreateRole(string roleName)
        {
            using (ConnectionContext db = new ConnectionContext())
            {
                if (roleName == null) return;
                db.Roles.Add(new Role
                {
                    RoleName = roleName
                });
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            using (ConnectionContext db = new ConnectionContext())
            {
                // Получаем пользователя
                User user = db.Users.FirstOrDefault(u => u.Login == username);
                // Получаем роли поль
                var role = from roles in db.Roles
                    join userRole in db.UserRoles on roles.RoleId equals userRole.RoleId
                    join u in db.Users on userRole.UserId equals u.UserId
                    where roles.RoleName == roleName
                    select new
                    {
                        roles.RoleName
                    };

                foreach (var r in role)
                {
                    if (user != null && role != null && r.RoleName == roleName)
                        return true;
                }
                return false;
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using (ConnectionContext db = new ConnectionContext())
            {
                var uRole = from user in db.Users
                    join userRole in db.UserRoles on user.UserId equals userRole.UserId
                    join role in db.Roles on userRole.RoleId equals role.RoleId
                    select role.RoleName;
                return uRole.ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (ConnectionContext db = new ConnectionContext())
            {
                var uRole = from user in db.Users
                    join userRole in db.UserRoles on user.UserId equals userRole.UserId
                    join role in db.Roles on userRole.RoleId equals role.RoleId
                    where role.RoleName == roleName
                    select user.Login;
                return uRole.ToArray();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}