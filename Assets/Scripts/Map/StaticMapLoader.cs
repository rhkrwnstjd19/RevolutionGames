using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Cerberus_Platform_API
{
    public class StaticMapLoader : MonoBehaviour
    {
        public RawImage mapRawImage;

        [Header("맵 정보 입력")]
        public string strBaseURL = "https://api.vworld.kr/req/image?service=image&request=getmap&key=";
        public string latitude = "";
        public string longitude = "";
        public int zoomLevel = 14;
        public int mapWidth;
        public int mapHeight;
        public string strAPIKey = "";

        private void Start()
        {
            StartCoroutine(WorldMapLoad());
        }

        void Update()
        {
            
        }
        IEnumerator WorldMapLoad()
        {
            while(true){
                yield return new WaitForSeconds(1.0f);

                StringBuilder str = new StringBuilder();
                str.Append(strBaseURL.ToString());
                str.Append(strAPIKey.ToString());
                str.Append("&format=png");
                str.Append("&basemap=GRAPHIC");
                str.Append("&center=");
                str.Append(longitude.ToString());
                str.Append(",");
                str.Append(latitude.ToString());
                str.Append("&crs=epsg:4326");
                str.Append("&zoom=");
                str.Append(zoomLevel.ToString());
                str.Append("&size=");
                str.Append(mapWidth.ToString());
                str.Append(",");
                str.Append(mapHeight.ToString());

                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    Debug.Log($"{i} : {c}");
                }
            
                Debug.Log(str.ToString());
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(str.ToString());

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
                }
                try
                {
                    // longitude = GPS_Manager.Instance.longitude.ToString();
                    // latitude = GPS_Manager.Instance.latitude.ToString();
                    double tmplongitude = double.Parse(longitude);
                    double tmplatitude = double.Parse(latitude);
                    tmplongitude +=0.00001;
                    tmplatitude  +=0.00001;
                    longitude = tmplongitude.ToString();
                    latitude = tmplatitude.ToString();
                }
                catch (Exception ex)
                {
                    latitude = "37.791231";
                    longitude = "127.123242";
                    // Optionally, you can log additional information from the exception, such as ex.StackTrace
                }
            }
        }
    }
}
