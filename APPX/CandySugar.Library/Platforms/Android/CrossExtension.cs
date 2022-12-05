using Android.Content;
using Android.OS;
using CandySugar.Library.Common;
using Env = Android.OS.Environment;
using FileProvider = AndroidX.Core.Content.FileProvider;
using Settings = Android.Provider.Settings;
using Uri = Android.Net.Uri;


namespace CandySugar.Library.Platforms.Android
{
    public class CrossExtension: ICrossExtension
    {
        public string AndriodPath => Env.GetExternalStoragePublicDirectory(Env.DirectoryDownloads).AbsoluteFile + "";

        public void InstallApk()
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.GrantPersistableUriPermission);
            var jfile = new Java.IO.File(Path.Combine(AndriodPath, "CandySugar.apk"));
            var intentType = "application/vnd.android.package-archive";
        
            if (Build.VERSION.SdkInt < BuildVersionCodes.N)
                intent.SetDataAndType(Uri.FromFile(jfile), intentType);
            else
            {
                //安卓7.0以上的程序
                var contentUri = FileProvider.GetUriForFile(CrossCurrentActivity.Current.Activity.ApplicationContext, $"{CrossCurrentActivity.Current.Activity.PackageName}.fileprovider", jfile);
                intent.SetDataAndType(contentUri, intentType);
                //安卓8.0以上的程序
                //兼容8.0
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    bool hasInstallPermission = CrossCurrentActivity.Current.Activity.ApplicationContext.PackageManager.CanRequestPackageInstalls();
                    if (!hasInstallPermission)
                    {
                        //注意这个是8.0新API
                        Uri packageURI =Uri.Parse("package:" + CrossCurrentActivity.Current.Activity.PackageName);
                        Intent intents = new Intent(Settings.ActionManageUnknownAppSources, packageURI);
                        intents.AddFlags(ActivityFlags.NewTask);
                        intents.AddFlags(ActivityFlags.GrantWriteUriPermission);
                        intents.AddFlags(ActivityFlags.GrantReadUriPermission);
                        intents.AddFlags(ActivityFlags.GrantPersistableUriPermission);
                        CrossCurrentActivity.Current.Activity.ApplicationContext.StartActivity(intents);
                        return;
                    }
                }
            }

            CrossCurrentActivity.Current.Activity.ApplicationContext.StartActivity(intent);
        }
    }
}
