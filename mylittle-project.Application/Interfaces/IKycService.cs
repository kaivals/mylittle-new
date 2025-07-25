using mylittle_project.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylittle_project.Application.Interfaces
{
    public interface IKycService
    {
        Task AddDocumentRequestAsync(KycDocumentRequestDto dto);
        Task<List<KycDocumentRequestDto>> GetRequestedDocumentsAsync(Guid Dealerid);
        Task<string> UploadDocumentAsync(KycDocumentUploadDto dto);
    }
}
