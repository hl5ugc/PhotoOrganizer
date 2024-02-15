using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace PhotoOrganizer.Interfaces;

public interface IThumbNailService
{
    Task<BitmapImage?> GetThumbNail(StorageFile file);
}
