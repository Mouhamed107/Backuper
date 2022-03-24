using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
namespace Backuper.NAS
{
    public class NAS_Operation
    {
      

        public static void UploadFileToServer(IFormFile formFile)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(NAS_Access.getDomaine()+formFile.FileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(NAS_Access.getUsername(), NAS_Access.getPasseword());

                FileStream fileStream = File.Open("Tmp/" + formFile.FileName, FileMode.Open, FileAccess.Read);
                byte[] fileContents=new byte[fileStream.Length];
                fileStream.Read(fileContents,0,fileContents.Length);
                fileStream.Close();
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                //FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                // Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }


            catch (Exception ex)
            {
                throw ex;
            }
}


public static byte[] DisplayFileFromServer(string file)
{
    WebClient request = new WebClient();
    string url = NAS_Access.getDomaine() + file;
    request.Credentials = new NetworkCredential(NAS_Access.getUsername(), NAS_Access.getPasseword());

    try
    {
        byte[] newFileData = request.DownloadData(url);
        return newFileData;
    }
    catch (WebException e)
    {

        return null;
    }
}


public static void DeleteFileInServer(string file)
{
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(NAS_Access.getDomaine() + file);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(NAS_Access.getUsername(), NAS_Access.getPasseword());

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    // return response.StatusDescription;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
    
    

}


public static void updateFileInServer(string oldFile, IFormFile formFile)
{
    DeleteFileInServer(oldFile);
    UploadFileToServer(formFile);
    Console.WriteLine("Fichier modifié");
}

public static void backupFile(string filePath, string DestinationFoler)
   
        {
            string fileName = Path.GetFileName(filePath);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(DestinationFoler + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(NAS_Access.getUsername(), NAS_Access.getPasseword());

            FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            byte[] fileContents = new byte[fileStream.Length];
            fileStream.Read(fileContents, 0, fileContents.Length);
            fileStream.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();


            /*
                        try
                        {
                            FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                            byte[] fileContents = new byte[fileStream.Length];
                            fileStream.Read(fileContents, 0, fileContents.Length);
                            fileStream.Close();
                            ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
                            var handler = new HttpClientHandler();
                            handler.Credentials = new NetworkCredential(NAS_Access.getUsername(), NAS_Access.getPasseword());
                            var client = new HttpClient(handler);
                            HttpResponseMessage response= await client.PostAsync(DestinationFoler + fileName, byteArrayContent);
                            response.EnsureSuccessStatusCode();
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }*/
        }


    }
}
