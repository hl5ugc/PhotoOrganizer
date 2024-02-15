using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Bytes;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using PhotoOrganizer.Interfaces;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PhotoOrganizer.Services;

public class MetadataSerice : IMetadataSerice
{
    public async Task<string?> GetHumanizedFileSize(StorageFile file)
    {
        BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
        return new ByteSize(basicProperties.Size).Humanize();
    }

    public DateTime? GetTakendDate(StorageFile file)
    {
        IReadOnlyList<MetadataExtractor.Directory>? directories = ImageMetadataReader.ReadMetadata(file.Path);
        ExifSubIfdDirectory? directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        if (directory?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTime) is true)
        {
            return dateTime;
        }

        return null;
    }
}
