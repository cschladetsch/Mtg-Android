#if PLATFORM_ANDROID
#else
#endif

using System;
using System.IO;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using App.MtgService;

namespace App.Camera {
    public class WebCamViewer : MonoBehaviour {
        public string RelativePathToImage;
        public string AbsolutePathToImage;
        public string WebCamName; // USB Video Device
        public byte[] Data;

        private WebCamTexture webCamTexture;
        private static Logger Log = new Logger(typeof(WebCamViewer));

        private void Start() {
            WebCamDevice device = WebCamTexture.devices[GetCamera()];
            webCamTexture = new WebCamTexture(device.name);
            GetComponent<Renderer>().material.mainTexture = webCamTexture;
            webCamTexture.Play();
        }

        private int GetCamera() {
            if (WebCamTexture.devices.Length == 0) {
                Log.Error("Cannot find any camera");
                return 0;
            }

            for (int i = 0; i < WebCamTexture.devices.Length; i++) {
                var cameraName = WebCamTexture.devices[i].name;
                if (cameraName.Contains(WebCamName)) {
                    Log.Info("Found device, index=" + i);
                    return i;
                }
            }

            Log.Error("Can not find your camera name. Here's a list of all cameras:");
            for (int i = 0; i < WebCamTexture.devices.Length; i++) {
                Log.Info("Device: " + WebCamTexture.devices[i].name);
            }

            return 0;
        }

        internal void SetImageArt(Card card, Button button) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(SampleCard);
            LoadImage(card, "art-crop");
        }

        internal void SetImage(Card card, Button button) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(SampleCard);
            LoadImage(card, "normal");
        }

        void SampleCard() {
            TurnOnCamera();
        }

        void LoadImage(Card card, string type) {
            TurnOffCamera();
            var path = Path.Combine(UnityEngine.Application.persistentDataPath, "images", $"{card.ScryfallId}-{type}.jpg");
            Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
            texture.LoadImage(File.ReadAllBytes(path));
            GetComponent<Renderer>().material.mainTexture = texture;
        }

        void TurnOnCamera() {
            webCamTexture.Play();
            webCamTexture.autoFocusPoint = new Vector2(0.5f, 0.3f);
            GetComponent<Renderer>().material.mainTexture = webCamTexture;
        }

        void TurnOffCamera() {
            webCamTexture.Stop();
        }

        public IEnumerator TakePhotoCoro(Action<WebCamViewer> completed) {
            yield return new WaitForEndOfFrame();

            Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
            texture.SetPixels(webCamTexture.GetPixels());
            texture.Apply();

            Data = texture.EncodeToPNG();
            AbsolutePathToImage = Path.Combine(UnityEngine.Application.persistentDataPath, RelativePathToImage, "image.png");
            File.WriteAllBytes(AbsolutePathToImage, Data);
            completed(this);
        }
    }
}

