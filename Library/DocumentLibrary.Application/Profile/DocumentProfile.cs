using DocumentLibrary.Application.Helper;
using DocumentLibrary.Data.Dto;
using DocumentLibrary.MySql.Models;

namespace DocumentLibrary.Application.Profile
{
    public class DocumentProfile : AutoMapper.Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, AddDocumentFileDto>();
            CreateMap<AddDocumentFileDto, Document>();
            CreateMap<SetDocumentFileDto, Document>();
            CreateMap<ChangeDocumentVersionDto, Document>();
            CreateMap<Document, DocumentVersionHistory>();
            CreateMap<Document, CancelVoidDocumentDto>();
            

            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}