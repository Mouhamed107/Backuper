using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backuper.Models.BLL;
using Backuper.Models.Entities;
using Backuper.NAS;
using System.IO;
namespace Backuper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : Controller
    {
        
        // GET: api/<BackupController>
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                /*Task.Run(async () =>
                {
                    string[] files = Directory.GetFiles("wwwroot/");
                    foreach (string file in files)
                    {
                        try
                        {
                            NAS_Operation.backupFile(file, NAS_Access.getBackupFolder());
                        }
                        catch
                        {

                        }
                    }

                    Backup backup = new Backup();
                    backup.Etat = "Terminee";
                    backup.Message = "Backup effectué avec succes";
                    backup.DateBackup = "Date: " + DateTime.Now.ToString();
                    //BLL_Backup.Add(backup);
                });*/
                List<Backup> backups = BLL_Backup.GetAll();
                return Json(new { success = true, message = "Backups trouvés", data = backups });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        // GET api/<BackupController>/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            try
            {
                
                // Backup backup = BLL_Backup.GetBackup(id);

                Task.Run(async () =>
               {
                   try
                   {
                       string[] files = Directory.GetFiles("wwwroot/");
                       foreach (string file in files)
                       {
                           NAS_Operation.backupFile(file, NAS_Access.getBackupFolder());
                       }
                       Backup backup = new Backup();
                       backup.Etat = "Terminee";
                       backup.Message = "Backup effectué avec succes";
                       backup.DateBackup = "Date: " + DateTime.Now.ToString();
                       BLL_Backup.Add(backup);
                   }
                   catch(Exception ex)
                   {
                       Backup backup = new Backup();
                       backup.Etat = "Erreur";
                       backup.Message = ex.Message;
                       backup.DateBackup = "Date: " + DateTime.Now.ToString();
                       BLL_Backup.Add( backup);
                   }
                   
               });
                return Json(new { success = true, message = "Backup trouvé"});
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        // POST api/<BackupController>
        [HttpPost]
        public JsonResult Post([FromForm] Backup backup)
        {
            
            try
            {
                
                if (backup.Id == 0)
                {                  
                    
                    Task.Run(async () =>
                            {
                                var startTimeSpan = TimeSpan.Zero;
                                var periodTimeSpan = TimeSpan.FromMinutes(BLL_BackupConfig.GetBackupConfig(1).Interval);
                                var timer = new System.Threading.Timer((e) =>
                                {
                                    try
                                    {
                                        string[] files = Directory.GetFiles("wwwroot/");
                                        foreach (string file in files)
                                        {
                                            try
                                            {
                                                NAS_Operation.backupFile(file, NAS_Access.getBackupFolder());
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        Backup backup = new Backup();
                                        backup.Etat = "Terminee";
                                        backup.Message = "Backup effectué avec succes";
                                        backup.DateBackup = "Date: " + DateTime.Now.ToString();
                                        BLL_Backup.Add(backup);
                                    }
                                    catch (Exception ex)
                                    {
                                        Backup backup = new Backup();
                                        backup.Etat = "Erreur";
                                        backup.Message = ex.Message;
                                        backup.DateBackup = "Date: " + DateTime.Now.ToString();
                                        BLL_Backup.Add(backup);
                                    }

                                }, null, startTimeSpan, periodTimeSpan);
                            });

                        return Json(new { success = true, message = "Ajouté avec succès", data = backup });
                }



                else
                {
                    BLL_Backup.Update(backup.Id, backup);
                    return Json(new { success = true, message = "Modifié avec succès", data = backup });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // DELETE api/<BackupController>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                BLL_Backup.Delete(id);
              
                return Json(new { success = true, message = "Supprimé avec succès" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        /*[Route("BackupPlanifier")]
        [HttpGet("BackupPlanifier")]*/


        /*public JsonResult BackupPlanifier([FromForm] int Interval)
        {
            try
            {

                BackupConfig backupConfig = new BackupConfig();
                backupConfig.Interval = Interval;
                backupConfig.Heure = 0;
                BLL_BackupConfig.Update(1, backupConfig);

                Task.Run(async () =>
                {
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromMinutes(BLL_BackupConfig.GetBackupConfig(1).Interval);
                    var timer = new System.Threading.Timer((e) =>
                    {
                        try
                        {
                            string[] files = Directory.GetFiles("wwwroot/");
                            foreach (string file in files)
                            {
                                try
                                {
                                    NAS_Operation.backupFile(file, NAS_Access.getBackupFolder());
                                }
                                catch
                                {

                                }
                            }

                            Backup backup = new Backup();
                            backup.Etat = "Terminee";
                            backup.Message = "Backup effectué avec succes";
                            backup.DateBackup = "Date: " + DateTime.Now.ToString();
                            BLL_Backup.Add(backup);
                        }
                        catch (Exception ex)
                        {
                            Backup backup = new Backup();
                            backup.Etat = "Erreur";
                            backup.Message = ex.Message;
                            backup.DateBackup = "Date: " + DateTime.Now.ToString();
                            BLL_Backup.Add(backup);
                        }

                    }, null, startTimeSpan, periodTimeSpan);
                });

                return Json(new { success = true, message = "Backup planifié" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }*/


    }
}