// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using GoShared;
// using System.Linq;
// using MiniJSON;
// namespace GoMap
// {
//     public class GOPlaces_new : MonoBehaviour
//     {
//         public GOMap goMap;

//         public string googleAPIkey;

//         string nearbySearchUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
//         private object gt;

//         // // Start is called before the first frame update
//         // void Awake()
//         // {
//         //     goMap.OnTileLoad.AddListener (GOTile = &gt; {
//         //     OnLoadTile (GOTile);

//         //     });
//         // }

//         // Update is called once per frame
//         void Update()
//         {
            
//         }
//         void OnLoadTile (GOTile tile) {

//             StartCoroutine (NearbySearch(tile));

//         }

//         IEnumerator NearbySearch (GOTile tile) {
//             //Center of the map tile
//             Coordinates tileCenter = tile.goTile.tileCenter;

//             //radius of the request, equals the tile diagonal /2
//             float radius = tile.goTile.diagonalLenght / 2;

//             //The complete nearby search url, api key is added at the end
//             //string url = nearbySearchUrl + "location="+tile.goTile.tileCenter.latitude+","+tile.goTile.tileCenter.longitude+"&amp;radius="+tile.goTile.diagonalLenght/2+"&amp;type="+type+"&amp;key="+googleAPIkey;

//             //Perform the request
//             var www = new WWW(url);

//             yield return www;

//             //Check for errors
//             if (string.IsNullOrEmpty (www.error)) {
//                 string response = www.text;

//                 //Deserialize the json response

//                 IDictionary deserializedResponse = (IDictionary)Json.Deserialize (response);

//                 Debug.Log(string.Format("[GO Places] Tile center: {0} - Request Url {1} - response {2}",tileCenter.toLatLongString(),url,response));
//             }
//         }
//     }

// }