using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Hexenstein.Framework.Services
{
    public interface IFileDialogs
    {
        string GetFileOpenPath(string title, string filter);

        string GetFileSavePath(string title, string defaultExt, string filter);
    }

    internal class FileDialogs : IFileDialogs
    {
        private static IEnumerable<CommonFileDialogFilter> ConvertFilterToNewType(string filter)
        {
            return filter
                .Split('|')
                .Zip(filter.Split('|').Skip(1), (l, r) => new { l, r })
                .Select(f => new CommonFileDialogFilter(f.l, f.r) { ShowExtensions = false });
        }

        public string GetFileOpenPath(string title, string filter)
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                using (var cfd = new CommonOpenFileDialog())
                {
                    cfd.Title = title;
                    cfd.EnsureFileExists = true;
                    cfd.RestoreDirectory = true;
                    var newFilters = ConvertFilterToNewType(filter);
                    foreach (var f in newFilters)
                        cfd.Filters.Add(f);
                    if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
                        return cfd.FileName;
                }
            }
            else
            {
                var ofd = new OpenFileDialog();
                ofd.Title = title;
                ofd.CheckFileExists = true;
                ofd.RestoreDirectory = true;

                ofd.Filter = filter;

                if (ofd.ShowDialog() == true)
                    return ofd.FileName;
            }

            return "";
        }

        public string GetFileSavePath(string title, string defaultExt, string filter)
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                using (var cfd = new CommonSaveFileDialog())
                {
                    cfd.Title = title;
                    cfd.DefaultExtension = defaultExt;
                    cfd.EnsurePathExists = false;
                    cfd.RestoreDirectory = true;
                    var newFilters = ConvertFilterToNewType(filter);
                    foreach (var f in newFilters)
                        cfd.Filters.Add(f);
                    if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
                        return cfd.FileName;
                }
            }
            else
            {
                var ofd = new SaveFileDialog();
                ofd.Title = title;
                ofd.DefaultExt = defaultExt;
                ofd.CheckFileExists = false;
                ofd.RestoreDirectory = true;

                ofd.Filter = filter;

                if (ofd.ShowDialog() == true)
                    return ofd.FileName;
            }

            return "";
        }
    }
}