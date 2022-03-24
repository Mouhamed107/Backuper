using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backuper.Models.BLL;
using Backuper.Models.Entities;
using Backuper.NAS;
using Backuper.TimeManagement;
namespace Backuper.Controllers{
[Route("api/[controller]")]
 [ApiController]
 public class BackupPlanifierController : Controller
{
// GET: api/<BackupPlanifierController>
[HttpGet]
 public JsonResult Get()
{
try
{
 List<BackupPlanifier> backupplanifiers = BLL_BackupPlanifier.GetAll();
return Json(new { success = true, message = "BackupPlanifiers trouvés", data = backupplanifiers });
 }
catch (Exception e)
{
return Json(new { success = false, message = e.Message });
}
}
// GET api/<BackupPlanifierController>/5
[HttpGet("{id}")]
public JsonResult Get(int id)
{
 try
 {
 BackupPlanifier backupplanifier = BLL_BackupPlanifier.GetBackupPlanifier(id);
 return Json(new { success = true, message = "BackupPlanifier trouvé", data = backupplanifier });
}
 catch (Exception e)
 {
return Json(new { success = false, message = e.Message });
}
}
// POST api/<BackupPlanifierController>
[HttpPost]
public JsonResult Post([FromForm] BackupPlanifier backupplanifier)
{
try
{
if(backupplanifier.Id == 0)
{
 backupplanifier.Id = BLL_BackupPlanifier.Add(backupplanifier);
                    string dateEx = BLL_BackupPlanifier.GetBackupPlanifier(backupplanifier.Id).DateExecution.ToString();
                    string TimeToExe= BLL_BackupPlanifier.GetBackupPlanifier(backupplanifier.Id).TimeToExecute.ToString();
                    Backup backup = new Backup();
                    backup.Etat = "Attente";
                    backup.Message = "Backup sera executé le " + dateEx + " à " + TimeToExe; ;
                    backup.DateBackup = "Date: " + dateEx + " "+TimeToExe; ;
                    backup.Id = BLL_Backup.Add(backup);
                    Task.Run(async () =>
                    {
                        var startTimeSpan = TimeSpan.Zero;  
                    var periodTimeSpan = TimeSpan.FromMinutes(1);
                        
                    var timer = new System.Threading.Timer((e) =>
                    {
                        
                        string toDaye = DateOnly.FromDateTime(DateTime.Now).ToString();
                        string CurentTime = DateTime.Now.ToString("HH:mm");  // on peut aussi faire TimeOnly.FromDateTime(DateTime.Now).ToString();
                        if (toDaye == dateEx && CurentTime==TimeToExe  )
                        {
                            try
                            {
                                string[] files = Directory.GetFiles("wwwroot/");
                                foreach (string file in files)
                                {
                                    NAS_Operation.backupFile(file, NAS_Access.getBackupFolder());
                                }
                                backup.Etat = "Terminee";
                                backup.Message = "Backup effectué avec succes";
                                backup.DateBackup = "Date: " + DateTime.Now.ToString()+" à "+TimeToExe;
                                BLL_Backup.Update(backup.Id, backup);

                            }
                            catch (Exception ex)
                            {

                                backup.Etat = "Erreur";
                                backup.Message = ex.Message;
                                backup.DateBackup = "Date: " + DateTime.Now.ToString();
                                BLL_Backup.Update(backup.Id, backup);
                            }
                        }

                    }, null, startTimeSpan, periodTimeSpan);
                    
                       
                });
                return Json(new { success = true, message = "Ajouté avec succès", data = backupplanifier });
}
else
{
 BLL_BackupPlanifier.Update(backupplanifier.Id, backupplanifier);
 return Json(new { success = true, message = "Modifié avec succès", data=backupplanifier });
 }
}
catch (Exception ex)
{
return Json(new { success = false, message = ex.Message });
}
}
// DELETE api/<BackupPlanifierController>/5
[HttpDelete("{id}")]
public JsonResult Delete(int id)
 {
try
{
BLL_BackupPlanifier.Delete(id);
return Json(new { success = true, message = "Supprimé avec succès" });
}
catch (Exception ex)
{
 return Json(new { success = false, message = ex.Message });
}
}
}
}