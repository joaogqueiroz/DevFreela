using System;
using System.Collections.Generic;
namespace DevFreela.Infrastructure.CloudServices.Interfaces
{
  public interface IFileStorageService
  {
    void UploadFile(byte[] bytes, string fileName);
  }
}