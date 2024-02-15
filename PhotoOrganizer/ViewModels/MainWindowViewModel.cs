using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using Microsoft.UI.Xaml.Controls;
using PhotoOrganizer.Interfaces;
using Windows.Storage;
using Windows.Storage.Search;

namespace PhotoOrganizer.ViewModels;

 
public partial class MainWindowViewModel  : ObservableObject
{
    private readonly IThumbNailService _thumbNailService;

    [ObservableProperty]
    private StorageFolder? _inputFolder;
    [ObservableProperty]
    private StorageFolder? _outputFolder;
    [ObservableProperty]
    private ObservableCollection<PhotoViewModel> _photos = new();
 

    public MainWindowViewModel(IThumbNailService thumbNailService)
    {
        _thumbNailService = thumbNailService;
    }

    [RelayCommand]
    private async Task LoadPhotosAsync(string? inputFolderPath,CancellationToken cancellationToken)
    {
        if (inputFolderPath is not null && LoadPhotosCommand.IsRunning is false)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(inputFolderPath);
            if(folder is not null)
            {
                Photos.Clear();

                List<string> fileTypeFilter = new() { ".jpg", ".jpeg", ".bmp", };
                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery,fileTypeFilter);
                queryOptions.FolderDepth = FolderDepth.Deep;
                StorageFileQueryResult? results = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyCollection<StorageFile>? files = await results.GetFilesAsync();

                if(files is not null)
                {
                    List<PhotoViewModel> photoViewModels = new();
                    foreach (StorageFile file in files)
                    {
                        if (cancellationToken.IsCancellationRequested is not true)
                        {
                            PhotoViewModel photoViewModel = new(file, _thumbNailService);
                            photoViewModels.Add(photoViewModel);
                        }
                    }
                    if (photoViewModels.Count > 0)
                    {
                        Photos = new ObservableCollection<PhotoViewModel>(photoViewModels);
                    }
                     
                }
            }
        }
    }

    [RelayCommand]
    private async Task UpdateInputFolderPath(string? folderPath)
    {
        if (folderPath is not null)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            if (folder is not null)
            {
                InputFolder = folder;
            }
        }
    }
    [RelayCommand]
    private async Task UpdateOutputFolderPath(string? folderPath)
    {
        if (folderPath is not null)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            if (folder is not null)
            {
                OutputFolder = folder;
            }
        }
    }
    [RelayCommand]
    private Task PreparePhotoAsync(int photoIndex)
    {
        PhotoViewModel photoViewModel = Photos[photoIndex];

        return photoViewModel.LoadThumbnailAync();

    }
}
