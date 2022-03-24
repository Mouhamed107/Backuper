using System.IO;
using Backuper.Models.DAL;
using Backuper.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backuper.NAS;
using System.Timers;
namespace Backuper.Models.BLL
{
public class BLL_BackupPeriodique
{
        

public static int Add(BackupPeriodique backupperiodique)
{
return DAL_BackupPeriodique.Add(backupperiodique);
}
 public static void Update(int id, BackupPeriodique backupperiodique)
{
 DAL_BackupPeriodique.Update(id, backupperiodique);
}
 public static void Delete(int id)
{
 DAL_BackupPeriodique.Delete(id);
}
public static BackupPeriodique GetBackupPeriodique(int id)
{
 return DAL_BackupPeriodique.GetBackupPeriodique(id);
}
public static List<BackupPeriodique> GetAll()
{
return DAL_BackupPeriodique.SelectAll();
}

        public static void RunBackup(Object source, ElapsedEventArgs e)
        {
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
                    backup.Id = BLL_Backup.Add(backup);

                }
                catch (Exception ex)
                {
                    Backup backup = new Backup();
                    backup.Etat = "Erreur";
                    backup.Message = ex.Message;
                    backup.DateBackup = "Date: " + DateTime.Now.ToString();
                    backup.Id = BLL_Backup.Add(backup);
                }
            });

        }
    }
}