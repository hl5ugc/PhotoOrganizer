using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using PhotoOrganizer.Interfaces;
using PhotoOrganizer.Services;
using Windows.Storage;

namespace PhotoOrganizer.ViewModels;

public partial class PhotoViewModel : ObservableObject
{
    #region Fields
    private readonly StorageFile _file;
    private readonly IThumbNailService? _thumbNailService;

    [ObservableProperty]
    private BitmapImage? _thumbNail;
    [ObservableProperty]
    private string? _inputFileName;
    [ObservableProperty]
    private string? _inputFilePath;
    [ObservableProperty]
    private string? _filseSize;
    [ObservableProperty]
    private DateTime? _dateTaken;
    [ObservableProperty]
    private string? _outputFilePath;
    #endregion

    public PhotoViewModel(StorageFile file,IThumbNailService? ithumbNailService)
    {
        _file = file;
        _thumbNailService = ithumbNailService;
        InputFileName = _file.Name;
        InputFilePath = _file.Path.ToString();

    }

    public async Task LoadThumbnailAync()
    {
        if (ThumbNail is null && _thumbNailService is not null)
        {
            ThumbNail = await _thumbNailService.GetThumbNail(_file);
        }
    }
}
