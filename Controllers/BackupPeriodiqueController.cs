
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backuper.Models.BLL;
using Backuper.Models.Entities;
using Backuper.TimeManagement;
namespace Backuper.Controllers{
[Route("api/[controller]")]
 [ApiController]
 public class BackupPeriodiqueController : Controller
{
// GET: api/<BackupPeriodiqueController>
[HttpGet]
 public JsonResult Get()
{
try
{
 List<BackupPeriodique> backupperiodiques = BLL_BackupPeriodique.GetAll();
return Json(new { success = true, message = "BackupPeriodiques trouvés", data = backupperiodiques });
 }
catch (Exception e)
{
return Json(new { success = false, message = e.Message });
}
}
// GET api/<BackupPeriodiqueController>/5
[HttpGet("{id}")]
public JsonResult Get(int id)
{
 try
 {
 BackupPeriodique backupperiodique = BLL_BackupPeriodique.GetBackupPeriodique(id);
 return Json(new { success = true, message = "BackupPeriodique trouvé", data = backupperiodique });
}
 catch (Exception e)
 {
return Json(new { success = false, message = e.Message });
}
}
// POST api/<BackupPeriodiqueController>
[HttpPost]
public JsonResult Post([FromForm] BackupPeriodique backupperiodique)
{
try
{
if(backupperiodique.Id == 0)
{
 backupperiodique.Id = BLL_BackupPeriodique.Add(backupperiodique);
 return Json(new { success = true, message = "Ajouté avec succès", data = backupperiodique });
}
else
{
 BLL_BackupPeriodique.Update(backupperiodique.Id, backupperiodique);
                    int Interval = 0;
                    string TypeInterval = BLL_BackupPeriodique.GetBackupPeriodique(1).TypeInterval;
                    switch (TypeInterval)
                    {
                        case "jour":
                            Interval = 3600 * 1000 * 24 * BLL_BackupPeriodique.GetBackupPeriodique(1).Interval;
                            break;
                        case "heure":
                                Interval = 3600 * 1000  * BLL_BackupPeriodique.GetBackupPeriodique(1).Interval;
                            break;
                        case "minute":
                                Interval = 60 * 1000 * BLL_BackupPeriodique.GetBackupPeriodique(1).Interval;
                            break;
                    }
                    MyTimer.ReuinitialiseTimer();
                    System.Timers.Timer aTimer = MyTimer.getTimer(Interval);
                    aTimer.Elapsed += BLL_BackupPeriodique.RunBackup;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;
                    return Json(new { success = true, message = "Modifié avec succès", data=backupperiodique });
 }
}
catch (Exception ex)
{
return Json(new { success = false, message = ex.Message });
}
}
// DELETE api/<BackupPeriodiqueController>/5
[HttpDelete("{id}")]
public JsonResult Delete(int id)
 {
try
{
BLL_BackupPeriodique.Delete(id);
return Json(new { success = true, message = "Supprimé avec succès" });
}
catch (Exception ex)
{
 return Json(new { success = false, message = ex.Message });
}
}
}
}