using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PhotoOrganizer.Interfaces;

public interface IMetadataSerice
{
    Task<string?> GetHumanizedFileSize(StorageFile file);

    DateTime? GetTakendDate(StorageFile file);
}
