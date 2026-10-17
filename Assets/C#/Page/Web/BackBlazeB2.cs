using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;

public class BackBlazeB2 : MonoBehaviour
{
    public void urlupload()
    {
        String apiUrl = "API_URL"; //Provided by b2_authorize_account 
        String accountAuthorizationToken = "ACCOUNT_AUTHORIZATION_TOKEN"; //Provided by b2_authorize_account
        String bucketId = "BUCKET_ID"; //The unique bucket ID
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiUrl + "/b2api/v2/b2_get_upload_url");
        string body = "{\"bucketId\":\"" + bucketId + "\"}";
        var data = Encoding.UTF8.GetBytes(body);
        webRequest.Method = "POST";
        webRequest.Headers.Add("Authorization", accountAuthorizationToken);
        webRequest.ContentType = "application/json; charset=utf-8";
        webRequest.ContentLength = data.Length;
        using (var stream = webRequest.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
        WebResponse response = (HttpWebResponse)webRequest.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        response.Close();
        Console.WriteLine(responseString);
    }
    private void upload()
    {
        String uploadAuthorizationToken = "UPLOAD_AUTHORIZATION_TOKEN"; //Provided by b2_get_upload_url
        String contentType = "text/plain"; //Type of file i.e. image/jpeg, audio/mpeg...
        String filePath = "FILE_PATH"; //File path of desired upload 
        String fileName = "typing_test.txt"; //Desired name for the file
        String sha1Str = ""; //Sha1 verification for the file

        // Read the file into memory and take a sha1 of the data.
        FileInfo fileInfo = new FileInfo(filePath);
        byte[] bytes = File.ReadAllBytes(filePath);
        SHA1 sha1 = SHA1.Create();
        // NOTE: Loss of precision. You may need to change this code if the file size is larger than 32-bits.
        byte[] hashData = sha1.ComputeHash(bytes, 0, (int)fileInfo.Length);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashData)
        {
            sb.Append(b.ToString("x2"));
        }
        sha1Str = sb.ToString();

        // Send over the wire
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
        webRequest.Method = "POST";
        webRequest.Headers.Add("Authorization", uploadAuthorizationToken);
        webRequest.Headers.Add("X-Bz-File-Name", fileName);
        webRequest.Headers.Add("X-Bz-Content-Sha1", sha1Str);
        webRequest.Headers.Add("X-Bz-Info-Author", "unknown");
        webRequest.Headers.Add("X-Bz-Server-Side-Encryption", "AES256");
        webRequest.ContentType = contentType;
        using (var stream = webRequest.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }
        WebResponse response = (HttpWebResponse)webRequest.GetResponse();
        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        response.Close();
        Console.WriteLine(responseString);
    }
    
}