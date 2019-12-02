#if UNITY_EDITOR //Google plugin not used in any builds outside of the editor
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Sheets.v4.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
using System;
using UnityEngine.Networking;

namespace Ambition
{
    public static class GoogleUtil
    {
        private const string CLIENT_ID = "947666797364-kadfo4g92tgaijaogege5v7v7gpq1fqb.apps.googleusercontent.com";
        private const string API_KEY = "AIzaSyBiPP72Sv-GTTUEK-2vv6dpkUwz3zu0JgI";
        private const string DIRECTORY = "https://docs.google.com/spreadsheets/d/{0}/edit#gid=0";
        private const string SCOPES = "https://www.googleapis.com/auth/spreadsheets.readonly";

        public static void Get(string fileId, string filter, Action<bool, string> onReceiveJSON)
        {
            UnityWebRequest request = new UnityWebRequest(string.Format(DIRECTORY, fileId));

            //                gapi.load('client:auth2', initClient);

            /*
*             //       gapi.auth2.getAuthInstance().signIn();
           
                gapi.client.init({
                apiKey: API_KEY,
                          clientId: CLIENT_ID,
                          discoveryDocs: DISCOVERY_DOCS,
                          scope: SCOPES
                        }).then(function() {
                    // Listen for sign-in state changes.
                    gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);

                    // Handle the initial sign-in state.
                    updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
            */
        }

        public static void Post(string fileId, string JSON, Action<bool> OnResponse = null)
        {

        }

        private static void OnSignIn(bool success, UnityWebRequest request, Action<bool, string> onReceiveJSON)
        {
            if (!success)
            {
                onReceiveJSON(false, "ERROR: Unable to sign into Google client!");
            }
            else
            {
                request.SendWebRequest();
                // OnReceiveResponse(onReceiveJSON);
            }
        }

        private static void OnReceiveResponse(UnityWebRequest request, Action<bool, string> onReceiveJSON)
        {
            onReceiveJSON(!request.isHttpError, request.isHttpError
                ? request.error
                : request.downloadHandler.text);
        }
    }
}
#endif
