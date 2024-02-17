using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using PhotoOrganizer.Interfaces;
using Windows.Storage;

namespace PhotoOrganizer.ViewModels;

public class PhotoViewModelBuilder
{
    private readonly StorageFile _file;
    private IMetadataSerice?    _metadataSerice;
    private IThumbNailService?  _thumbNailService;
    private string? _outputBaseFolderPath;
    private string? _outputFolderFormat;
 

    public PhotoViewModelBuilder(StorageFile file)
    {
        _file = file;
    }

    public async Task<PhotoViewModel> BuildAsync()
    {
        PhotoViewModel photoViewModel = new(_file, _thumbNailService);
        if (_metadataSerice is not null)
        {
            photoViewModel.DateTaken = _metadataSerice.GetTakendDate(_file);
            photoViewModel.FilseSize = await _metadataSerice.GetHumanizedFileSize(_file);
        }

        if (_outputBaseFolderPath is not null &&
           _outputFolderFormat is not null &&
           photoViewModel.DateTaken is not null)
        {
            string? outputFilePath = CreateDateTimeFormatedFolderPath(
                photoViewModel.DateTaken,
                _outputBaseFolderPath,
                _outputFolderFormat);

            if (outputFilePath is not null)
            {
                outputFilePath += $"\\{photoViewModel.InputFileName}";
            }
            photoViewModel.OutputFilePath = outputFilePath;
        }

        return photoViewModel;
    }

    private string? CreateDateTimeFormatedFolderPath(DateTime? dateTaken, string outputBaseFolderPath, string outputFolderFormat)
    {
        StringBuilder stringBuilder = new StringBuilder();

        return stringBuilder
            .Append(outputBaseFolderPath)
            .Append(dateTaken?.ToString(outputFolderFormat))
            .ToString();
    }

    public PhotoViewModelBuilder WithMetadata()
    {
        _metadataSerice = Ioc.Default.GetService<IMetadataSerice>();
        return this;
    }

    public PhotoViewModelBuilder WithOutputFolderPath(string outputBaseFolderPath, string? folderformat)
    {
        _outputBaseFolderPath = outputBaseFolderPath;
        _outputFolderFormat = folderformat;
        return this;
    }

    public PhotoViewModelBuilder WithThumbNailService()
    {
        _thumbNailService = Ioc.Default.GetService <IThumbNailService>();

        return this;
    }
}
