using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Logoinowanie.Data;
using Logoinowanie.Models;
using Logoinowanie.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Logoinowanie.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public PasswdModel PasswdModel { get; set; }


        public IConfiguration Configuration { get; }
        public WalletController(IConfiguration configuration, ApplicationDbContext db)
        {
            Configuration = configuration;
            _db = db;
        }

        // GET: Wallet
        public ActionResult Index()
        {
            return View();
        }
        
        
        // GET: Wallet
        public ActionResult Detail(int Id)
        {
            List<PasswdModel> passwdlist = new List<PasswdModel>();
            string connectionString = Configuration["ConnectionStrings:MySQLConnection"];

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                //MySqlDataReader
                connection.Open();

                string sql = $"Select * From PWallet Where ID='{Id}'";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        PasswdModel passmodel = new PasswdModel
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            Login = Convert.ToString(dataReader["Login"]),
                            UrlP = Convert.ToString(dataReader["UrlP"]),
                            Email = Convert.ToString(dataReader["Email"]),
                            Description = Convert.ToString(dataReader["Description"]),

                            Passwd = Encryption.DecryptString(
                            Convert.ToString(dataReader["SaltKey"]),
                            Convert.ToString(dataReader["Passwd"]
                            ))
                        };

                        passwdlist.Add(passmodel);
                    }
                }
                connection.Close();
            }
            return View(passwdlist);
        }

        public IActionResult Upsert(int? id)
        {
            PasswdModel = new PasswdModel();
            if (id == null)
            {
                //Przekazywanie do Insert ---->
                return View(PasswdModel);
            }

            //Pobieranie do Update ---->
            PasswdModel = _db.PWallet.FirstOrDefault(u => u.ID == id);
            if (PasswdModel == null)
            {
                return NotFound();
            }
            return View(PasswdModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(PasswdModel passmodel)
        {//funkcja tworzenia edycji ------>
            if (ModelState.IsValid)
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string connectionString = Configuration["ConnectionStrings:MySQLConnection"];

                if (PasswdModel.ID == 0)
                {
                    //Tworzenie Create ---->
                    using (MySqlConnection  connection = new MySqlConnection (connectionString))
                    {
                        string Salt = "salt";
                        string sql = "";

                        sql = $"Select Salt From AspNetUsers Where Id='{userId}'";
                        MySqlCommand commandS = new MySqlCommand(sql, connection);
                        connection.Open();
                        using (MySqlDataReader dataReader = commandS.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Salt = Convert.ToString(dataReader["Salt"]);
                            }
                        }
                        commandS.ExecuteNonQuery();
                        connection.Close();

                        string EncryptPass = Encryption.EncryptString(Salt, passmodel.Passwd);

                        sql = $"Insert Into PWallet (Email, UrlP, Login, Passwd, UserId, Description, SaltKey) Values (" +
                            $"'{passmodel.Email}', '{passmodel.UrlP}', '{passmodel.Login}', '{EncryptPass}'," +
                            $"'{userId}','{passmodel.Description}','{Salt}')";

                        using MySqlCommand command = new MySqlCommand(sql, connection)
                        {
                            CommandType = CommandType.Text
                        };
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    //Aktualizacja Update ---->
                    using MySqlConnection connection = new MySqlConnection(connectionString);
                    string Salt = "salt";
                    string sql = "";

                    sql = $"Select Salt From AspNetUsers Where Id='{userId}'";
                    MySqlCommand commandS = new MySqlCommand(sql, connection);
                    connection.Open();
                    using (MySqlDataReader dataReader = commandS.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Salt = Convert.ToString(dataReader["Salt"]);
                        }
                    }
                    commandS.ExecuteNonQuery();
                    connection.Close();

                    string EncryptPass = Encryption.EncryptString(Salt, passmodel.Passwd);

                    sql = $"Update PWallet SET Email='{passmodel.Email}', UrlP='{passmodel.UrlP}'," +
                        $" Login='{passmodel.Login}', Description='{passmodel.Description}', Passwd='{EncryptPass}', SaltKey='{Salt}' " +
                        $"Where ID='{passmodel.ID}'";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    return RedirectToAction("Index");
                }
            }
                return View(PasswdModel);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<PasswdModel> passwdlist = new List<PasswdModel>();
            string connectionString = Configuration["ConnectionStrings:MySQLConnection"];

            MySqlConnection connection = new MySqlConnection(connectionString);
            
                //MySqlDataReader
                connection.Open();
            
                string sql = $"Select * From PWallet Where UserId='{userId}'";
                MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader dataReader = command.ExecuteReader();
                
                    while (dataReader.Read())
                    {
                        PasswdModel passmodel = new PasswdModel
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            Login = Convert.ToString(dataReader["Login"]),
                            UrlP = Convert.ToString(dataReader["UrlP"]),
                            Email = Convert.ToString(dataReader["Email"]),
                            Passwd = Encryption.DecryptString(
                            Convert.ToString(dataReader["SaltKey"]),
                            Convert.ToString(dataReader["Passwd"]
                            )),
                            UserId = Convert.ToString(dataReader["UserId"])
                        };

                        passwdlist.Add(passmodel);
                    }
                
                connection.Close();
            await Task.Delay(1);
            return Json(new { data = passwdlist });

            /*string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Json(new { data = await _db.PWallet.Where(e => e.UserId == userId).ToListAsync() });*/
        }

        // GET: Wallet/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var PasswdModelFromDb = await _db.PWallet.FirstOrDefaultAsync(u => u.ID == id);
            if (PasswdModelFromDb == null)
            {
                return Json(new { success = false, message = "Błąd w trakcie usuwania" });
            }
            _db.PWallet.Remove(PasswdModelFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Usunieto z Powodzeniem" });
        }
        #endregion
    }
}