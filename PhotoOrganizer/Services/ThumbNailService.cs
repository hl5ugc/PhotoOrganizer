using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.UI.Xaml.Media.Imaging;
using PhotoOrganizer.Interfaces;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PhotoOrganizer.Services;

public class ThumbNailService : IThumbNailService
{
    private readonly IMemoryCache _memoryCache;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public ThumbNailService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<BitmapImage?> GetThumbNail(StorageFile file)
    {
        BitmapImage? thumbNail = null;
        await _semaphore.WaitAsync();

        try
        {
            if (_memoryCache.TryGetValue(file, out thumbNail) is not true)
            {
                StorageItemThumbnail source = await file.GetThumbnailAsync(ThumbnailMode.PicturesView);
                thumbNail = new();
                await thumbNail.SetSourceAsync(source);
                _memoryCache.Set(file.Path, thumbNail);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return thumbNail ;
    }
}
