using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace API
{
    public class Map
    {
        string URL = "http://mywarmplace.tk:8001/api/";

        public static Map Instance = new Map();

        public Map()
        {
        }

        public Map(string url)
        {
            URL = url;
        }

        static readonly HttpClient client = new HttpClient();

        public async Task UpdatePlaysByPK(string pk, string success)
        {
            try
            {
                var url = URL + "mapfile/update/pk/" + pk + "?success=" + success;
                HttpResponseMessage response = await client.PostAsync(url, null);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task UpdatePlaysByTitle(string title, string success)
        {
            try
            {
                var url = URL + "mapfile/update/title/" + title + "?success=" + success;
                HttpResponseMessage response = await client.PostAsync(url, null);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task<List<MetaData>> ReadAllMetaData()
        {
            try
            {
                var url = URL + "mapmeta";
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                List<MetaData> metaDatas = JsonConvert.DeserializeObject<List<MetaData>>(responseBody);
                return metaDatas;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }

            return null;
        }

        public async Task UploadFileToServer(string filename, string src,
            string duration, string difficulty)
        {
            try
            {
                var url = URL + "mapfile/upload/" + filename +
                          "?duration=" + duration + "&difficulty=" + difficulty;


                byte[] imgdata = System.IO.File.ReadAllBytes(src);
                var imageContent = new ByteArrayContent(imgdata);
                var requestContent = new MultipartFormDataContent();
                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("image/jpeg");

                requestContent.Add(imageContent);


                HttpResponseMessage response = await client.PostAsync(url, requestContent);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task DownloadFileFromServer(string PK, string path)
        {
            try
            {
                var url = URL + "mapfile/download/" + PK;

                HttpResponseMessage response = await client.GetAsync(url);
                byte[] content = await response.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(path, content);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task<MetaData> ReadMetaDataByPK(string pk)
        {
            try
            {
                var url = URL + "mapmeta/pk/" + pk;
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                MetaData metaData = JsonConvert.DeserializeObject<MetaData>(responseBody);
                return metaData;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }

            return null;
        }

        public async Task<MetaData> ReadMetaDataByTitle(string title)
        {
            try
            {
                var url = URL + "mapmeta/title/" + title;
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                MetaData metaData = JsonConvert.DeserializeObject<MetaData>(responseBody);
                return metaData;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }

            return null;
        }

        public async Task DeletePlaysByPK(string pk)
        {
            try
            {
                var url = URL + "mapmeta/pk/" + pk;
                HttpResponseMessage response = await client.DeleteAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task DeletePlaysByTitle(string title)
        {
            try
            {
                var url = URL + "mapmeta/title/" + title;
                HttpResponseMessage response = await client.DeleteAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }

        public async Task DeletePlays()
        {
            try
            {
                var url = URL + "mapmeta";
                HttpResponseMessage response = await client.DeleteAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Exception Caught!");
                Debug.LogError("Message: {e.Message}");
            }
        }
    }
}