#if UNITY_EDITOR //Google plugin not used in any builds outside of the editor
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Sheets.v4.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Ambition
{
    public static class GoogleUtil
    {
        private const string APPLICATION_NAME = "Ambition Settings v1.0";
        private const string CREDENTIALS_PATH = "credentials.json";
        private const string CLIENT_ID = "947666797364-kadfo4g92tgaijaogege5v7v7gpq1fqb.apps.googleusercontent.com";
        private const string API_KEY = "AIzaSyBiPP72Sv-GTTUEK-2vv6dpkUwz3zu0JgI";
        private const string DIRECTORY = "https://docs.google.com/spreadsheets/d/{0}/edit#gid=0";
        private const string SCOPES = "https://www.googleapis.com/auth/spreadsheets.readonly";

        private static void InitService()
        {/*
            if (_service != null) return;
            UserCredential cred;
            using (FileStream stream = new FileStream(CREDENTIALS_PATH, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                cred = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new string[] { SCOPES },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Debug.Log("Credential file saved to: " + credPath);
            }
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = APPLICATION_NAME
            });
        */}

        //       https://docs.google.com/forms/d/e/1FAIpQLScY4mhmB5hFxDOx8U6qM8xzTVr0p3LomxZmlr0ofDUaRwedVw/viewform?usp=sf_link

        public static void Get(string fileId, string filter, Action<bool, string> onReceiveJSON)
        {
            InitService();
            UnityWebRequest request = new UnityWebRequest(string.Format(DIRECTORY, fileId));

                            //gapi.load('client:auth2', initClient);

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

        public static void Post(string fileId, WWWForm form, Action<bool> OnResponse = null)
        {
            //byte[] data = form.
            Thread thread = new Thread(new ThreadStart(UploadProc));
            thread.Start();
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

        public static void UploadProc()
        {
            //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

            //UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", formData);
            //yield return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            //{
            //    Debug.Log(www.error);
            //}
            //else
            //{
            //    Debug.Log("Form upload complete!");
            //}
        }

        struct GoogleRequest
        {
            private UnityWebRequest _request;
            private Thread _thread;
            private byte[] _data;
            private Action<bool> _onResponse;

            GoogleRequest(string fileId, byte[] data, Action<bool> OnResponse=null)
            {
                _request = new UnityWebRequest(fileId, UnityWebRequest.kHttpVerbPOST, null, null);
                _data = data;
                _onResponse = OnResponse;
                _thread = null;
                _request.SendWebRequest();
                Start();
            }

            private void Start()
            {
                if (_thread == null)
                {
                    _thread = new Thread(new ThreadStart(_ProcessThread));
                    _thread.Start();
                }
            }

            private void _ProcessThread()
            {
                Thread.Sleep(0);
            }
        }
    }
}
#endif
