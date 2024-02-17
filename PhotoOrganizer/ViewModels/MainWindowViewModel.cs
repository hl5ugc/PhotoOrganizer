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
    //private readonly IThumbNailService _thumbNailService;

    [ObservableProperty]
    private StorageFolder? _inputFolder;

    [ObservableProperty]
    private StorageFolder? _outputFolder;

    [ObservableProperty]
    private ObservableCollection<PhotoViewModel> _photos = new();

    [ObservableProperty]
    private string? _outputFolderFormat = string.Empty ;

    [ObservableProperty]
    private bool _hasPhotos;

    [ObservableProperty]
    private int _foundFilesCount;

    [ObservableProperty]
    private int _loadedFilesCount;

    //public MainWindowViewModel(IThumbNailService thumbNailService)
    //{
    //    _thumbNailService = thumbNailService;
    //}
    public MainWindowViewModel()
    {
        
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
                HasPhotos = false;

                List<string> fileTypeFilter = new() { ".jpg", ".jpeg", ".bmp", };
                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery,fileTypeFilter);
                queryOptions.FolderDepth = FolderDepth.Deep;
                StorageFileQueryResult? results = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyCollection<StorageFile>? files = await results.GetFilesAsync();

                if(files is not null)
                {
                    List<PhotoViewModel> photoViewModels = new();
                    IProgress<int> progress = new Progress<int>(x => LoadedFilesCount = x);
                    FoundFilesCount = files.Count;
                    int reportingInterval = Math.Max(files.Count / 100, 1);

                    foreach (StorageFile file in files)
                    {
                        if (cancellationToken.IsCancellationRequested is not true)
                        {
                            string outputFolderPath = OutputFolder is not null ? OutputFolder.Path : string.Empty;
                            
                            PhotoViewModel photoViewModel = await new PhotoViewModelBuilder(file)
                                .WithThumbNailService()
                                .WithMetadata()
                                .WithOutputFolderPath(outputFolderPath,OutputFolderFormat)
                                .BuildAsync();

                            photoViewModels.Add(photoViewModel);

                            if ((photoViewModels.Count % reportingInterval) == 0)
                            {
                                progress.Report(photoViewModels.Count);
                            }

                            // await Task.Delay(1000);
                        }
                    }
                    if (photoViewModels.Count > 0)
                    {
                        LoadedFilesCount = photoViewModels.Count;
                        Photos = new ObservableCollection<PhotoViewModel>(photoViewModels);
                        HasPhotos = Photos.Count > 0;
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
    private void UpdateOutputFolderFormat(string folderFormat) => OutputFolderFormat = folderFormat;

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
