using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        #region Properties

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        #endregion

        #region Validate Credentials

        public bool ValidateUserCredentials(string email, string password, out User loggedUser)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    cm.CommandText = "SELECT * FROM User WHERE Email = @Email AND Password = @Password";
                    cm.Parameters.AddWithValue("@Email", email);
                    cm.Parameters.AddWithValue("@Password", password);
                    using (var reader = cm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Fetch(reader);
                            loggedUser = this;
                            return true;
                        }
                        else
                        {
                            loggedUser = null;
                            return false;
                        }
                    }
                }
            }
        }

        #endregion

        #region Register | Update Object 

        public string RegisterUser(User user)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    PassingUserToProperties(user);
                    cm.CommandText = "INSERT INTO User VALUES (@Id, @Name, @Email, @BirthDate, @Country, @Password, @Role)";
                    AddUpdateUser(cm);
                    var affectedRows = cm.ExecuteNonQuery();

                    if (affectedRows.Equals(1)) return $"User { user.Name } added!";
                    else return "Failed to add user";
                }
            }
        }

        public string UpdateUser(User user)
        {
            using (var conn = new SQLiteConnection(Database.Database.Connect))
            {
                using (var cm = new SQLiteCommand(conn))
                {
                    PassingUserToProperties(user);
                    cm.CommandText = "UPDATE TABLE User SET (Id = @Id, Name = @Name, Email = @Email, BirthDate = @BirthDate, Country = @Country, Password = @Password, Role = @Role)";
                    AddUpdateUser(cm);
                    var affectedRows = cm.ExecuteNonQuery();

                    if (affectedRows.Equals(1)) return $"User { user.Name } added!";
                    else return "Failed to add user";
                }
            }
        }

        #endregion

        #region Fecth | Add | Update : Auxilixar functions

        private void PassingUserToProperties(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            BirthDate = user.BirthDate;
            Country = user.Country;
            Password = user.Password;
            Role = user.Role;
        }

        private void AddUpdateUser(SQLiteCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", Guid.NewGuid());
            cm.Parameters.AddWithValue("@Name", Name);
            cm.Parameters.AddWithValue("@Email", Email);
            cm.Parameters.AddWithValue("@BirthDate", BirthDate);
            cm.Parameters.AddWithValue("@Country", Country);
            cm.Parameters.AddWithValue("@Password", Password);
            cm.Parameters.AddWithValue("@Role", Role);
        }

        private void Fetch(SQLiteDataReader reader)
        {
            while(reader.Read())
            {
                Id = reader.GetString(reader.GetOrdinal("Id"));
                Name = reader.GetString(reader.GetOrdinal("Name"));
                Email = reader.GetString(reader.GetOrdinal("Email"));
                BirthDate = reader.GetString(reader.GetOrdinal("BirthDate"));
                Country = reader.GetString(reader.GetOrdinal("Country"));
                Password = reader.GetString(reader.GetOrdinal("Password"));
                Role = reader.GetString(reader.GetOrdinal("Role"));
            }
        }

        #endregion

    }
}
