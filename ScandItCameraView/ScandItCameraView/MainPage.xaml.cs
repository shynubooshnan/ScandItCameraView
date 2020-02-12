using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace ScandItCameraView
{
    public partial class MainPage : ContentPage
    {
        #region Bindable property
        public static BindableProperty DidScannedCommandProperty = BindableProperty.Create("DidScannedCommand",
        typeof(Command<List<string>>),
        typeof(MainPage),
        null);

        public Command<List<string>> DidScannedCommand
        {
            get { return (Command<List<string>>)GetValue(DidScannedCommandProperty); }
            set { SetValue(DidScannedCommandProperty, value); }
        }
        #endregion

        public MainPage()
        {
            InitializeComponent();
            //initialize command
            //Note:Can be add as a command in viewmodel and bind the same in xaml
            DidScannedCommand = new Command<List<string>>(OnDidScanned);
            scanedCamera.DidScannedCommand = DidScannedCommand;

            //check device camera permission
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await CrossMedia.Current.Initialize();

                        //Checking device camera permission
                        var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                        if (cameraStatus != PermissionStatus.Granted)
                        {
                            var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                            if (results != null)
                                cameraStatus = results[Permission.Camera];
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }

            //stop scanning and hide scanner
            scanedCamera.StopScanning?.Invoke();
            scanedCamera.IsVisible = false;
            //assign text to scan result label
            ScanResultLabel.Text = "Click scan to start camera";
        }

        #region private methods

        /// <summary>
        /// Did scanned method
        /// </summary>
        /// <param name="scannedCodes"></param>
        void OnDidScanned(List<string> scannedCodes)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ScanResultLabel.Text = scannedCodes?.LastOrDefault();
            });
        }

        /// <summary>
        /// Start Scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Scan_Clicked(object sender, System.EventArgs e)
        {
            scanedCamera.IsVisible = true;
            //assign text to scan result label
            ScanResultLabel.Text = string.Empty;
            scanedCamera.StartScanning?.Invoke();
        }

        /// <summary>
        /// Stop Scan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StopScan_clicked(object sender, System.EventArgs e)
        {
            //assign text to scan result label
            ScanResultLabel.Text = "Click scan to start camera";
            scanedCamera.StopScanning?.Invoke();
            scanedCamera.IsVisible = false;
        }
        #endregion


    }
}
